FeatureScript 3008;
import(path : "onshape/std/common.fs", version : "3008.0");
import(path : "onshape/std/extrude.fs", version : "3008.0");

/*
 * =====================================================================
 *  OpenFTC CAD -- FeatureScript generators
 * =====================================================================
 *  Custom Onshape features that generate FTC parts from a few parameters.
 *
 *  Source of truth: https://github.com/FTCTeam30401/openftc-cad
 *  All dimensions trace to standards/master-variables.yaml (v0.2.0, audited).
 *
 *  Features in this studio:
 *    1. OpenFTC Hole Pattern -- vendor-standard hole grid on any plane/face
 *    2. OpenFTC Plate        -- printable plate sized by its hole pattern,
 *                               with optional goBILDA bearing holes + fillets
 *
 *  Vendor data (verified -- see repo for sources & confidence tags):
 *    goBILDA          8 mm grid,  M4  -- native bore 4.0 mm (thread-forming),
 *                                        mount clearance 4.5 mm (ISO 273 normal)
 *    RoBits / REV ION 0.5 in grid, #10 -- 0.201 in clearance
 *    VEX EDR          0.5 in grid, #8  -- native holes are 0.182 in SQUARE;
 *                                        round #8 clearance 0.177 in used for bolt-through
 *    REV DUO          8 mm pitch,  M3  -- drilled dia unpublished; ISO 273 normal 3.4 mm
 *
 *  Guiding principle: if a team can describe the part they need in a few
 *  parameters, OpenFTC CAD should generate it.
 * =====================================================================
 */

export enum OpenFtcStandard
{
    annotation { "Name" : "goBILDA (8 mm grid, M4)" }
    GOBILDA,
    annotation { "Name" : "AndyMark RoBits / REV ION (0.5 in grid, #10)" }
    ROBITS,
    annotation { "Name" : "VEX EDR (0.5 in grid, #8)" }
    VEX,
    annotation { "Name" : "REV DUO (8 mm pitch, M3)" }
    REV_DUO
}

export enum OpenFtcHoleRole
{
    annotation { "Name" : "Bolt onto vendor structure (clearance)" }
    MOUNT,
    annotation { "Name" : "Replicate vendor member (native size)" }
    NATIVE
}

// Spacing + hole diameters per standard. Values from master-variables.yaml v0.2.0.
function openFtcStandardSpec(standard is OpenFtcStandard) returns map
{
    if (standard == OpenFtcStandard.GOBILDA)
        return { "spacing" : 8 * millimeter, "mount" : 4.5 * millimeter, "native" : 4.0 * millimeter };
    if (standard == OpenFtcStandard.ROBITS)
        return { "spacing" : 0.5 * inch, "mount" : 0.201 * inch, "native" : 0.201 * inch };
    if (standard == OpenFtcStandard.VEX)
        return { "spacing" : 0.5 * inch, "mount" : 0.177 * inch, "native" : 0.182 * inch };
    return { "spacing" : 8 * millimeter, "mount" : 3.4 * millimeter, "native" : 3.4 * millimeter };
}

const GOBILDA_BEARING_DIA = 14 * millimeter;   // verified from goBILDA STEP files

// Draw one grid hole: a circle, or (VEX native) a square of the vendor's
// true 0.182 in square-hole width (VEX drawing PN 276-2600).
function skGridHole(sketch is Sketch, name is string, center is Vector, dia is ValueWithUnits, square is boolean)
{
    if (square)
        skRectangle(sketch, name, {
                    "firstCorner" : center - vector(dia / 2, dia / 2),
                    "secondCorner" : center + vector(dia / 2, dia / 2)
                });
    else
        skCircle(sketch, name, { "center" : center, "radius" : dia / 2 });
}

// =====================================================================
//  1. OpenFTC Hole Pattern
// =====================================================================

annotation { "Feature Type Name" : "OpenFTC Hole Pattern",
             "Feature Type Description" : "Vendor-standard hole grid (goBILDA / RoBits-REV ION / VEX / REV DUO). Part of OpenFTC CAD -- github.com/FTCTeam30401/openftc-cad" }
export const openFtcHolePattern = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Plane or planar face", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1 }
        definition.plane is Query;

        annotation { "Name" : "Standard" }
        definition.standard is OpenFtcStandard;

        annotation { "Name" : "Columns (X)" }
        isInteger(definition.nx, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Rows (Y)" }
        isInteger(definition.ny, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Hole size" }
        definition.role is OpenFtcHoleRole;

        annotation { "Name" : "Center pattern on origin", "Default" : false }
        definition.centered is boolean;

        annotation { "Name" : "Cut through parts", "Default" : true }
        definition.cut is boolean;
    }
    {
        const spec = openFtcStandardSpec(definition.standard);
        const s = spec.spacing;
        const d = definition.role == OpenFtcHoleRole.MOUNT ? spec.mount : spec.native;
        const square = definition.standard == OpenFtcStandard.VEX && definition.role == OpenFtcHoleRole.NATIVE;

        const plane = evPlane(context, { "face" : definition.plane });
        var sketch = newSketchOnPlane(context, id + "sketch", { "sketchPlane" : plane });

        var x0 = 0 * millimeter;
        var y0 = 0 * millimeter;
        if (definition.centered)
        {
            x0 = -s * (definition.nx - 1) / 2;
            y0 = -s * (definition.ny - 1) / 2;
        }

        for (var i = 0; i < definition.nx; i += 1)
        {
            for (var j = 0; j < definition.ny; j += 1)
            {
                skGridHole(sketch, "hole_" ~ i ~ "_" ~ j, vector(x0 + i * s, y0 + j * s), d, square);
            }
        }
        skSolve(sketch);

        if (definition.cut)
        {
            if (size(evaluateQuery(context, qAllModifiableSolidBodies())) > 0)
            {
                extrude(context, id + "cut", {
                            "entities" : qSketchRegion(id + "sketch", true),
                            "endBound" : BoundingType.THROUGH_ALL,
                            "hasSecondDirection" : true,
                            "secondDirectionBound" : BoundingType.THROUGH_ALL,
                            "operationType" : NewBodyOperationType.REMOVE
                        });
            }
            else
            {
                reportFeatureWarning(context, id, "No solid bodies to cut -- hole-pattern sketch created only.");
            }
        }
    });

// =====================================================================
//  2. OpenFTC Plate
// =====================================================================
//  A printable plate sized by its hole pattern: width = columns x spacing,
//  height = rows x spacing, holes centered with a half-pitch margin --
//  matching vendor pattern-plate conventions.
//
//  Optional goBILDA bearing holes: 14 mm holes replacing grid holes every
//  third position (24 mm pitch, like goBILDA structure), skipping the outer
//  ring so a 14 mm hole never breaks the plate edge.

const OPENFTC_PLATE_THICKNESS_BOUNDS = { (millimeter) : [0.4, 4, 50] } as LengthBoundSpec;
const OPENFTC_FILLET_BOUNDS = { (millimeter) : [0, 3, 20] } as LengthBoundSpec;

annotation { "Feature Type Name" : "OpenFTC Plate",
             "Feature Type Description" : "Printable plate sized by its vendor-standard hole pattern, with optional goBILDA bearing holes and corner fillets. Part of OpenFTC CAD -- github.com/FTCTeam30401/openftc-cad" }
export const openFtcPlate = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Plane or planar face", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1 }
        definition.plane is Query;

        annotation { "Name" : "Standard" }
        definition.standard is OpenFtcStandard;

        annotation { "Name" : "Columns (X)" }
        isInteger(definition.nx, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Rows (Y)" }
        isInteger(definition.ny, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Thickness" }
        isLength(definition.thickness, OPENFTC_PLATE_THICKNESS_BOUNDS);

        annotation { "Name" : "Hole size" }
        definition.role is OpenFtcHoleRole;

        annotation { "Name" : "goBILDA bearing holes (14 mm)", "Default" : false }
        definition.bearingHoles is boolean;

        annotation { "Name" : "Corner fillet radius" }
        isLength(definition.filletRadius, OPENFTC_FILLET_BOUNDS);
    }
    {
        const spec = openFtcStandardSpec(definition.standard);
        const s = spec.spacing;
        const d = definition.role == OpenFtcHoleRole.MOUNT ? spec.mount : spec.native;
        const square = definition.standard == OpenFtcStandard.VEX && definition.role == OpenFtcHoleRole.NATIVE;
        const W = definition.nx * s;
        const H = definition.ny * s;

        const plane = evPlane(context, { "face" : definition.plane });

        // --- plate body -------------------------------------------------
        var outline = newSketchOnPlane(context, id + "outline", { "sketchPlane" : plane });
        skRectangle(outline, "rect", {
                    "firstCorner" : vector(-W / 2, -H / 2),
                    "secondCorner" : vector(W / 2, H / 2)
                });
        skSolve(outline);
        extrude(context, id + "plate", {
                    "entities" : qSketchRegion(id + "outline"),
                    "endBound" : BoundingType.BLIND,
                    "depth" : definition.thickness,
                    "operationType" : NewBodyOperationType.NEW
                });

        // --- corner fillets ----------------------------------------------
        if (definition.filletRadius > 0 * millimeter)
        {
            const cornerEdges = qNonCapEntity(id + "plate", EntityType.EDGE);
            opFillet(context, id + "fillet", {
                        "entities" : cornerEdges,
                        "radius" : definition.filletRadius
                    });
        }

        // --- holes --------------------------------------------------------
        var holes = newSketchOnPlane(context, id + "holes", { "sketchPlane" : plane });
        for (var i = 0; i < definition.nx; i += 1)
        {
            for (var j = 0; j < definition.ny; j += 1)
            {
                const x = -W / 2 + s / 2 + i * s;
                const y = -H / 2 + s / 2 + j * s;
                var dia = d;
                var isBearing = false;
                if (definition.bearingHoles &&
                    definition.standard == OpenFtcStandard.GOBILDA &&
                    i > 0 && i < definition.nx - 1 &&
                    j > 0 && j < definition.ny - 1 &&
                    (i - 1) % 3 == 0 && (j - 1) % 3 == 0)
                {
                    dia = GOBILDA_BEARING_DIA;
                    isBearing = true;
                }
                skGridHole(holes, "h_" ~ i ~ "_" ~ j, vector(x, y), dia, square && !isBearing);
            }
        }
        skSolve(holes);
        extrude(context, id + "cut", {
                    "entities" : qSketchRegion(id + "holes", true),
                    "endBound" : BoundingType.THROUGH_ALL,
                    "hasSecondDirection" : true,
                    "secondDirectionBound" : BoundingType.THROUGH_ALL,
                    "operationType" : NewBodyOperationType.REMOVE,
                    "defaultScope" : false,
                    "booleanScope" : qCreatedBy(id + "plate", EntityType.BODY)
                });

        // --- tidy: remove construction sketches ---------------------------
        opDeleteBodies(context, id + "cleanup", {
                    "entities" : qUnion([
                                qCreatedBy(id + "outline", EntityType.BODY),
                                qCreatedBy(id + "holes", EntityType.BODY)])
                });
    });

// =====================================================================
//  3. OpenFTC Adapter Plate  --  two standards, one part
// =====================================================================
//  The cross-vendor bridge: zone A carries standard A's hole grid, zone B
//  carries standard B's, side by side on one plate. Adapters bolt onto
//  both structures, so holes are always mount-clearance size.

annotation { "Feature Type Name" : "OpenFTC Adapter Plate",
             "Feature Type Description" : "Cross-vendor adapter: two hole-pattern zones (any two standards) on one printable plate. Part of OpenFTC CAD -- github.com/FTCTeam30401/openftc-cad" }
export const openFtcAdapterPlate = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Plane or planar face", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1 }
        definition.plane is Query;

        annotation { "Name" : "Standard A (left zone)" }
        definition.standardA is OpenFtcStandard;

        annotation { "Name" : "A: Columns" }
        isInteger(definition.nxA, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "A: Rows" }
        isInteger(definition.nyA, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Standard B (right zone)" }
        definition.standardB is OpenFtcStandard;

        annotation { "Name" : "B: Columns" }
        isInteger(definition.nxB, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "B: Rows" }
        isInteger(definition.nyB, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Thickness" }
        isLength(definition.thickness, OPENFTC_PLATE_THICKNESS_BOUNDS);

        annotation { "Name" : "Corner fillet radius" }
        isLength(definition.filletRadius, OPENFTC_FILLET_BOUNDS);
    }
    {
        const specA = openFtcStandardSpec(definition.standardA);
        const specB = openFtcStandardSpec(definition.standardB);
        const sA = specA.spacing;
        const sB = specB.spacing;
        const WA = definition.nxA * sA;
        const WB = definition.nxB * sB;
        const HA = definition.nyA * sA;
        const HB = definition.nyB * sB;
        const W = WA + WB;
        const H = max(HA, HB);

        const plane = evPlane(context, { "face" : definition.plane });

        // --- plate body ---------------------------------------------------
        var outline = newSketchOnPlane(context, id + "outline", { "sketchPlane" : plane });
        skRectangle(outline, "rect", {
                    "firstCorner" : vector(-W / 2, -H / 2),
                    "secondCorner" : vector(W / 2, H / 2)
                });
        skSolve(outline);
        extrude(context, id + "plate", {
                    "entities" : qSketchRegion(id + "outline"),
                    "endBound" : BoundingType.BLIND,
                    "depth" : definition.thickness,
                    "operationType" : NewBodyOperationType.NEW
                });

        if (definition.filletRadius > 0 * millimeter)
        {
            opFillet(context, id + "fillet", {
                        "entities" : qNonCapEntity(id + "plate", EntityType.EDGE),
                        "radius" : definition.filletRadius
                    });
        }

        // --- holes: zone A (left) then zone B (right), both clearance -----
        var holes = newSketchOnPlane(context, id + "holes", { "sketchPlane" : plane });
        for (var i = 0; i < definition.nxA; i += 1)
        {
            for (var j = 0; j < definition.nyA; j += 1)
            {
                skCircle(holes, "a_" ~ i ~ "_" ~ j, {
                            "center" : vector(-W / 2 + sA / 2 + i * sA, -HA / 2 + sA / 2 + j * sA),
                            "radius" : specA.mount / 2
                        });
            }
        }
        for (var i = 0; i < definition.nxB; i += 1)
        {
            for (var j = 0; j < definition.nyB; j += 1)
            {
                skCircle(holes, "b_" ~ i ~ "_" ~ j, {
                            "center" : vector(-W / 2 + WA + sB / 2 + i * sB, -HB / 2 + sB / 2 + j * sB),
                            "radius" : specB.mount / 2
                        });
            }
        }
        skSolve(holes);
        extrude(context, id + "cut", {
                    "entities" : qSketchRegion(id + "holes", true),
                    "endBound" : BoundingType.THROUGH_ALL,
                    "hasSecondDirection" : true,
                    "secondDirectionBound" : BoundingType.THROUGH_ALL,
                    "operationType" : NewBodyOperationType.REMOVE,
                    "defaultScope" : false,
                    "booleanScope" : qCreatedBy(id + "plate", EntityType.BODY)
                });

        opDeleteBodies(context, id + "cleanup", {
                    "entities" : qUnion([
                                qCreatedBy(id + "outline", EntityType.BODY),
                                qCreatedBy(id + "holes", EntityType.BODY)])
                });
    });

// =====================================================================
//  4. OpenFTC L Gusset
// =====================================================================
//  Two perpendicular legs sharing a corner, each carrying the standard's
//  hole grid. The base leg lies on the picked plane; the vertical leg
//  rises from its back edge. First hole row in each leg sits one
//  half-pitch beyond the other leg's inner face, so holes never collide
//  with the corner material.

annotation { "Feature Type Name" : "OpenFTC L Gusset",
             "Feature Type Description" : "L bracket with vendor-standard hole grids on both legs. Part of OpenFTC CAD -- github.com/FTCTeam30401/openftc-cad" }
export const openFtcLGusset = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Plane or planar face", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1 }
        definition.plane is Query;

        annotation { "Name" : "Standard" }
        definition.standard is OpenFtcStandard;

        annotation { "Name" : "Columns (width)" }
        isInteger(definition.nx, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Base leg: hole rows" }
        isInteger(definition.nyBase, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Vertical leg: hole rows" }
        isInteger(definition.nyVert, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Thickness" }
        isLength(definition.thickness, OPENFTC_PLATE_THICKNESS_BOUNDS);

        annotation { "Name" : "Hole size" }
        definition.role is OpenFtcHoleRole;
    }
    {
        const spec = openFtcStandardSpec(definition.standard);
        const s = spec.spacing;
        const d = definition.role == OpenFtcHoleRole.MOUNT ? spec.mount : spec.native;
        const square = definition.standard == OpenFtcStandard.VEX && definition.role == OpenFtcHoleRole.NATIVE;
        const t = definition.thickness;
        const W = definition.nx * s;
        const depthBase = t + definition.nyBase * s;
        const heightVert = t + definition.nyVert * s;

        const basePlane = evPlane(context, { "face" : definition.plane });
        const yDir = cross(basePlane.normal, basePlane.x);
        // Vertical sketch plane: on the corner line, facing outward (-y),
        // so its sketch-Y axis runs up the vertical leg (+normal).
        const vertPlane = plane(basePlane.origin, -yDir, basePlane.x);

        // --- base leg ------------------------------------------------------
        var baseSk = newSketchOnPlane(context, id + "baseSk", { "sketchPlane" : basePlane });
        skRectangle(baseSk, "rect", {
                    "firstCorner" : vector(-W / 2, 0 * millimeter),
                    "secondCorner" : vector(W / 2, depthBase)
                });
        skSolve(baseSk);
        extrude(context, id + "base", {
                    "entities" : qSketchRegion(id + "baseSk"),
                    "endBound" : BoundingType.BLIND,
                    "depth" : t,
                    "operationType" : NewBodyOperationType.NEW
                });

        // --- vertical leg ----------------------------------------------------
        var vertSk = newSketchOnPlane(context, id + "vertSk", { "sketchPlane" : vertPlane });
        skRectangle(vertSk, "rect", {
                    "firstCorner" : vector(-W / 2, 0 * millimeter),
                    "secondCorner" : vector(W / 2, heightVert)
                });
        skSolve(vertSk);
        extrude(context, id + "vert", {
                    "entities" : qSketchRegion(id + "vertSk"),
                    "endBound" : BoundingType.BLIND,
                    "depth" : t,
                    "oppositeDirection" : true,
                    "operationType" : NewBodyOperationType.NEW
                });

        opBoolean(context, id + "join", {
                    "tools" : qUnion([
                                qCreatedBy(id + "base", EntityType.BODY),
                                qCreatedBy(id + "vert", EntityType.BODY)]),
                    "operationType" : BooleanOperationType.UNION
                });

        // --- base-leg holes ---------------------------------------------------
        var baseHoles = newSketchOnPlane(context, id + "baseHoles", { "sketchPlane" : basePlane });
        for (var i = 0; i < definition.nx; i += 1)
        {
            for (var j = 0; j < definition.nyBase; j += 1)
            {
                skGridHole(baseHoles, "h_" ~ i ~ "_" ~ j, vector(-W / 2 + s / 2 + i * s, t + s / 2 + j * s), d, square);
            }
        }
        skSolve(baseHoles);
        extrude(context, id + "baseCut", {
                    "entities" : qSketchRegion(id + "baseHoles", true),
                    "endBound" : BoundingType.THROUGH_ALL,
                    "hasSecondDirection" : true,
                    "secondDirectionBound" : BoundingType.THROUGH_ALL,
                    "operationType" : NewBodyOperationType.REMOVE,
                    "defaultScope" : false,
                    "booleanScope" : qCreatedBy(id + "base", EntityType.BODY)
                });

        // --- vertical-leg holes ----------------------------------------------
        var vertHoles = newSketchOnPlane(context, id + "vertHoles", { "sketchPlane" : vertPlane });
        for (var i = 0; i < definition.nx; i += 1)
        {
            for (var k = 0; k < definition.nyVert; k += 1)
            {
                skGridHole(vertHoles, "h_" ~ i ~ "_" ~ k, vector(-W / 2 + s / 2 + i * s, t + s / 2 + k * s), d, square);
            }
        }
        skSolve(vertHoles);
        extrude(context, id + "vertCut", {
                    "entities" : qSketchRegion(id + "vertHoles", true),
                    "endBound" : BoundingType.THROUGH_ALL,
                    "hasSecondDirection" : true,
                    "secondDirectionBound" : BoundingType.THROUGH_ALL,
                    "operationType" : NewBodyOperationType.REMOVE,
                    "defaultScope" : false,
                    "booleanScope" : qCreatedBy(id + "base", EntityType.BODY)
                });

        opDeleteBodies(context, id + "cleanup", {
                    "entities" : qUnion([
                                qCreatedBy(id + "baseSk", EntityType.BODY),
                                qCreatedBy(id + "vertSk", EntityType.BODY),
                                qCreatedBy(id + "baseHoles", EntityType.BODY),
                                qCreatedBy(id + "vertHoles", EntityType.BODY)])
                });
    });

// =====================================================================
//  5. OpenFTC U Gusset
// =====================================================================
//  A channel bracket: base leg spanning between two parallel vertical
//  legs. Same corner-safe hole offsets as the L gusset, applied at both
//  ends. Wraps vendor channel (goBILDA 48 mm U, REV 45 mm U) when sized
//  to match.

annotation { "Feature Type Name" : "OpenFTC U Gusset",
             "Feature Type Description" : "U channel bracket with vendor-standard hole grids on all three legs. Part of OpenFTC CAD -- github.com/FTCTeam30401/openftc-cad" }
export const openFtcUGusset = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Plane or planar face", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1 }
        definition.plane is Query;

        annotation { "Name" : "Standard" }
        definition.standard is OpenFtcStandard;

        annotation { "Name" : "Columns (width)" }
        isInteger(definition.nx, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Base: hole rows (between legs)" }
        isInteger(definition.nyBase, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Legs: hole rows" }
        isInteger(definition.nyVert, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Thickness" }
        isLength(definition.thickness, OPENFTC_PLATE_THICKNESS_BOUNDS);

        annotation { "Name" : "Hole size" }
        definition.role is OpenFtcHoleRole;
    }
    {
        const spec = openFtcStandardSpec(definition.standard);
        const s = spec.spacing;
        const d = definition.role == OpenFtcHoleRole.MOUNT ? spec.mount : spec.native;
        const square = definition.standard == OpenFtcStandard.VEX && definition.role == OpenFtcHoleRole.NATIVE;
        const t = definition.thickness;
        const W = definition.nx * s;
        const D = 2 * t + definition.nyBase * s;      // base outer depth (leg to leg)
        const heightVert = t + definition.nyVert * s;

        const basePlane = evPlane(context, { "face" : definition.plane });
        const yDir = cross(basePlane.normal, basePlane.x);
        const legPlane1 = plane(basePlane.origin, -yDir, basePlane.x);
        const legPlane2 = plane(basePlane.origin + D * yDir, -yDir, basePlane.x);

        // --- base -----------------------------------------------------------
        var baseSk = newSketchOnPlane(context, id + "baseSk", { "sketchPlane" : basePlane });
        skRectangle(baseSk, "rect", {
                    "firstCorner" : vector(-W / 2, 0 * millimeter),
                    "secondCorner" : vector(W / 2, D)
                });
        skSolve(baseSk);
        extrude(context, id + "base", {
                    "entities" : qSketchRegion(id + "baseSk"),
                    "endBound" : BoundingType.BLIND,
                    "depth" : t,
                    "operationType" : NewBodyOperationType.NEW
                });

        // --- two vertical legs -----------------------------------------------
        var leg1Sk = newSketchOnPlane(context, id + "leg1Sk", { "sketchPlane" : legPlane1 });
        skRectangle(leg1Sk, "rect", {
                    "firstCorner" : vector(-W / 2, 0 * millimeter),
                    "secondCorner" : vector(W / 2, heightVert)
                });
        skSolve(leg1Sk);
        extrude(context, id + "leg1", {
                    "entities" : qSketchRegion(id + "leg1Sk"),
                    "endBound" : BoundingType.BLIND,
                    "depth" : t,
                    "oppositeDirection" : true,
                    "operationType" : NewBodyOperationType.NEW
                });

        var leg2Sk = newSketchOnPlane(context, id + "leg2Sk", { "sketchPlane" : legPlane2 });
        skRectangle(leg2Sk, "rect", {
                    "firstCorner" : vector(-W / 2, 0 * millimeter),
                    "secondCorner" : vector(W / 2, heightVert)
                });
        skSolve(leg2Sk);
        extrude(context, id + "leg2", {
                    "entities" : qSketchRegion(id + "leg2Sk"),
                    "endBound" : BoundingType.BLIND,
                    "depth" : t,
                    "operationType" : NewBodyOperationType.NEW
                });

        opBoolean(context, id + "join", {
                    "tools" : qUnion([
                                qCreatedBy(id + "base", EntityType.BODY),
                                qCreatedBy(id + "leg1", EntityType.BODY),
                                qCreatedBy(id + "leg2", EntityType.BODY)]),
                    "operationType" : BooleanOperationType.UNION
                });

        // --- base holes -------------------------------------------------------
        var baseHoles = newSketchOnPlane(context, id + "baseHoles", { "sketchPlane" : basePlane });
        for (var i = 0; i < definition.nx; i += 1)
        {
            for (var j = 0; j < definition.nyBase; j += 1)
            {
                skGridHole(baseHoles, "h_" ~ i ~ "_" ~ j, vector(-W / 2 + s / 2 + i * s, t + s / 2 + j * s), d, square);
            }
        }
        skSolve(baseHoles);
        extrude(context, id + "baseCut", {
                    "entities" : qSketchRegion(id + "baseHoles", true),
                    "endBound" : BoundingType.THROUGH_ALL,
                    "hasSecondDirection" : true,
                    "secondDirectionBound" : BoundingType.THROUGH_ALL,
                    "operationType" : NewBodyOperationType.REMOVE,
                    "defaultScope" : false,
                    "booleanScope" : qCreatedBy(id + "base", EntityType.BODY)
                });

        // --- leg holes (both legs share sketch-plane hole layout) -------------
        for (var legIdx = 1; legIdx <= 2; legIdx += 1)
        {
            const lp = legIdx == 1 ? legPlane1 : legPlane2;
            var legHoles = newSketchOnPlane(context, id + ("leg" ~ legIdx ~ "Holes"), { "sketchPlane" : lp });
            for (var i = 0; i < definition.nx; i += 1)
            {
                for (var k = 0; k < definition.nyVert; k += 1)
                {
                    skGridHole(legHoles, "h_" ~ i ~ "_" ~ k, vector(-W / 2 + s / 2 + i * s, t + s / 2 + k * s), d, square);
                }
            }
            skSolve(legHoles);
            extrude(context, id + ("leg" ~ legIdx ~ "Cut"), {
                        "entities" : qSketchRegion(id + ("leg" ~ legIdx ~ "Holes"), true),
                        "endBound" : BoundingType.BLIND,
                        "depth" : t * 1.01,
                        "oppositeDirection" : legIdx == 1,
                        "operationType" : NewBodyOperationType.REMOVE,
                        "defaultScope" : false,
                        "booleanScope" : qCreatedBy(id + "base", EntityType.BODY)
                    });
        }

        opDeleteBodies(context, id + "cleanup", {
                    "entities" : qUnion([
                                qCreatedBy(id + "baseSk", EntityType.BODY),
                                qCreatedBy(id + "leg1Sk", EntityType.BODY),
                                qCreatedBy(id + "leg2Sk", EntityType.BODY),
                                qCreatedBy(id + "baseHoles", EntityType.BODY),
                                qCreatedBy(id + "leg1Holes", EntityType.BODY),
                                qCreatedBy(id + "leg2Holes", EntityType.BODY)])
                });
    });

// =====================================================================
//  6. OpenFTC T Gusset
// =====================================================================
//  A tee bracket: vertical leg rising from the middle of the base, hole
//  rows on both sides of it. Free base edges keep a half-pitch margin;
//  rows adjacent to the leg keep a half-pitch off its faces.

annotation { "Feature Type Name" : "OpenFTC T Gusset",
             "Feature Type Description" : "Tee bracket: center vertical leg, vendor-standard hole grids on base (both sides) and leg. Part of OpenFTC CAD -- github.com/FTCTeam30401/openftc-cad" }
export const openFtcTGusset = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Plane or planar face", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1 }
        definition.plane is Query;

        annotation { "Name" : "Standard" }
        definition.standard is OpenFtcStandard;

        annotation { "Name" : "Columns (width)" }
        isInteger(definition.nx, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Base: hole rows per side" }
        isInteger(definition.nyEach, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Vertical leg: hole rows" }
        isInteger(definition.nyVert, POSITIVE_COUNT_BOUNDS);

        annotation { "Name" : "Thickness" }
        isLength(definition.thickness, OPENFTC_PLATE_THICKNESS_BOUNDS);

        annotation { "Name" : "Hole size" }
        definition.role is OpenFtcHoleRole;
    }
    {
        const spec = openFtcStandardSpec(definition.standard);
        const s = spec.spacing;
        const d = definition.role == OpenFtcHoleRole.MOUNT ? spec.mount : spec.native;
        const square = definition.standard == OpenFtcStandard.VEX && definition.role == OpenFtcHoleRole.NATIVE;
        const t = definition.thickness;
        const W = definition.nx * s;
        const Yc = definition.nyEach * s;             // leg front face position
        const D = 2 * Yc + t;                          // total base depth
        const heightVert = t + definition.nyVert * s;

        const basePlane = evPlane(context, { "face" : definition.plane });
        const yDir = cross(basePlane.normal, basePlane.x);
        const legPlane = plane(basePlane.origin + Yc * yDir, -yDir, basePlane.x);

        // --- base -----------------------------------------------------------
        var baseSk = newSketchOnPlane(context, id + "baseSk", { "sketchPlane" : basePlane });
        skRectangle(baseSk, "rect", {
                    "firstCorner" : vector(-W / 2, 0 * millimeter),
                    "secondCorner" : vector(W / 2, D)
                });
        skSolve(baseSk);
        extrude(context, id + "base", {
                    "entities" : qSketchRegion(id + "baseSk"),
                    "endBound" : BoundingType.BLIND,
                    "depth" : t,
                    "operationType" : NewBodyOperationType.NEW
                });

        // --- center vertical leg ----------------------------------------------
        var legSk = newSketchOnPlane(context, id + "legSk", { "sketchPlane" : legPlane });
        skRectangle(legSk, "rect", {
                    "firstCorner" : vector(-W / 2, 0 * millimeter),
                    "secondCorner" : vector(W / 2, heightVert)
                });
        skSolve(legSk);
        extrude(context, id + "leg", {
                    "entities" : qSketchRegion(id + "legSk"),
                    "endBound" : BoundingType.BLIND,
                    "depth" : t,
                    "oppositeDirection" : true,
                    "operationType" : NewBodyOperationType.NEW
                });

        opBoolean(context, id + "join", {
                    "tools" : qUnion([
                                qCreatedBy(id + "base", EntityType.BODY),
                                qCreatedBy(id + "leg", EntityType.BODY)]),
                    "operationType" : BooleanOperationType.UNION
                });

        // --- base holes: both sides of the leg ---------------------------------
        var baseHoles = newSketchOnPlane(context, id + "baseHoles", { "sketchPlane" : basePlane });
        for (var i = 0; i < definition.nx; i += 1)
        {
            for (var j = 0; j < definition.nyEach; j += 1)
            {
                skGridHole(baseHoles, "s1_" ~ i ~ "_" ~ j, vector(-W / 2 + s / 2 + i * s, s / 2 + j * s), d, square);
                skGridHole(baseHoles, "s2_" ~ i ~ "_" ~ j, vector(-W / 2 + s / 2 + i * s, Yc + t + s / 2 + j * s), d, square);
            }
        }
        skSolve(baseHoles);
        extrude(context, id + "baseCut", {
                    "entities" : qSketchRegion(id + "baseHoles", true),
                    "endBound" : BoundingType.THROUGH_ALL,
                    "hasSecondDirection" : true,
                    "secondDirectionBound" : BoundingType.THROUGH_ALL,
                    "operationType" : NewBodyOperationType.REMOVE,
                    "defaultScope" : false,
                    "booleanScope" : qCreatedBy(id + "base", EntityType.BODY)
                });

        // --- leg holes -----------------------------------------------------------
        var legHoles = newSketchOnPlane(context, id + "legHoles", { "sketchPlane" : legPlane });
        for (var i = 0; i < definition.nx; i += 1)
        {
            for (var k = 0; k < definition.nyVert; k += 1)
            {
                skGridHole(legHoles, "h_" ~ i ~ "_" ~ k, vector(-W / 2 + s / 2 + i * s, t + s / 2 + k * s), d, square);
            }
        }
        skSolve(legHoles);
        extrude(context, id + "legCut", {
                    "entities" : qSketchRegion(id + "legHoles", true),
                    "endBound" : BoundingType.BLIND,
                    "depth" : t * 1.01,
                    "oppositeDirection" : true,
                    "operationType" : NewBodyOperationType.REMOVE,
                    "defaultScope" : false,
                    "booleanScope" : qCreatedBy(id + "base", EntityType.BODY)
                });

        opDeleteBodies(context, id + "cleanup", {
                    "entities" : qUnion([
                                qCreatedBy(id + "baseSk", EntityType.BODY),
                                qCreatedBy(id + "legSk", EntityType.BODY),
                                qCreatedBy(id + "baseHoles", EntityType.BODY),
                                qCreatedBy(id + "legHoles", EntityType.BODY)])
                });
    });

// =====================================================================
//  Print-first hardware: heat-set inserts & bearing pockets
// =====================================================================
//  Placement UX for both: sketch points where you want the hardware,
//  then select those points (plus the face they sit on).

export enum OpenFtcInsertSize
{
    annotation { "Name" : "M3 (4.0 mm pilot, 5.7 long)" }
    M3,
    annotation { "Name" : "M4 (5.6 mm pilot, 8.1 long)" }
    M4,
    annotation { "Name" : "M5 (6.4 mm pilot, 9.5 long)" }
    M5
}

// CNC Kitchen / Ruthex standard-length inserts -- master-variables.yaml v0.2.0
function openFtcInsertSpec(size is OpenFtcInsertSize) returns map
{
    if (size == OpenFtcInsertSize.M3)
        return { "pilot" : 4.0 * millimeter, "length" : 5.7 * millimeter };
    if (size == OpenFtcInsertSize.M4)
        return { "pilot" : 5.6 * millimeter, "length" : 8.1 * millimeter };
    return { "pilot" : 6.4 * millimeter, "length" : 9.5 * millimeter };
}

export enum OpenFtcBearing
{
    annotation { "Name" : "goBILDA 1611 (14 mm OD x 5)" }
    GOBILDA_1611,
    annotation { "Name" : "REV 49-1559 (12 mm OD x 3.5)" }
    REV_1559,
    annotation { "Name" : "608 skate / goBILDA 1600 (22 mm OD x 7)" }
    B608,
    annotation { "Name" : "625 (16 mm OD x 5)" }
    B625
}

function openFtcBearingSpec(b is OpenFtcBearing) returns map
{
    if (b == OpenFtcBearing.GOBILDA_1611)
        return { "od" : 14 * millimeter, "width" : 5 * millimeter };
    if (b == OpenFtcBearing.REV_1559)
        return { "od" : 12 * millimeter, "width" : 3.5 * millimeter };
    if (b == OpenFtcBearing.B608)
        return { "od" : 22 * millimeter, "width" : 7 * millimeter };
    return { "od" : 16 * millimeter, "width" : 5 * millimeter };
}

// Project a world point into a plane's 2D sketch coordinates.
function openFtcPlanePoint(p is Plane, wp is Vector) returns Vector
{
    const rel = wp - p.origin;
    return vector(dot(rel, p.x), dot(rel, cross(p.normal, p.x)));
}

const OPENFTC_BOSS_WALL_BOUNDS = { (millimeter) : [0.8, 1.6, 10] } as LengthBoundSpec;
const OPENFTC_FIT_CLEARANCE_BOUNDS = { (millimeter) : [0, 0.15, 1] } as LengthBoundSpec;

// =====================================================================
//  7. OpenFTC Heat-Set Boss
// =====================================================================

annotation { "Feature Type Name" : "OpenFTC Heat-Set Boss",
             "Feature Type Description" : "Heat-set insert bosses (CNC Kitchen / Ruthex sizing) at selected sketch points on a face. Part of OpenFTC CAD -- github.com/FTCTeam30401/openftc-cad" }
export const openFtcHeatSetBoss = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Face to build on", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1 }
        definition.face is Query;

        annotation { "Name" : "Boss locations (sketch points)", "Filter" : EntityType.VERTEX }
        definition.points is Query;

        annotation { "Name" : "Insert size" }
        definition.insert is OpenFtcInsertSize;

        annotation { "Name" : "Boss wall" }
        isLength(definition.wall, OPENFTC_BOSS_WALL_BOUNDS);
    }
    {
        const spec = openFtcInsertSpec(definition.insert);
        const bossDia = spec.pilot + 2 * definition.wall;
        const bossHeight = spec.length + 0.8 * millimeter;   // insert seats flush with margin below

        const facePlane = evPlane(context, { "face" : definition.face });
        const pts = evaluateQuery(context, definition.points);
        if (size(pts) == 0)
            throw regenError("Select at least one sketch point for boss placement.", ["points"]);

        var bossSk = newSketchOnPlane(context, id + "bossSk", { "sketchPlane" : facePlane });
        var pilotSk = newSketchOnPlane(context, id + "pilotSk", { "sketchPlane" : facePlane });
        for (var n = 0; n < size(pts); n += 1)
        {
            const c = openFtcPlanePoint(facePlane, evVertexPoint(context, { "vertex" : pts[n] }));
            skCircle(bossSk, "b" ~ n, { "center" : c, "radius" : bossDia / 2 });
            skCircle(pilotSk, "p" ~ n, { "center" : c, "radius" : spec.pilot / 2 });
        }
        skSolve(bossSk);
        skSolve(pilotSk);

        extrude(context, id + "boss", {
                    "entities" : qSketchRegion(id + "bossSk", true),
                    "endBound" : BoundingType.BLIND,
                    "depth" : bossHeight,
                    "operationType" : NewBodyOperationType.ADD
                });
        extrude(context, id + "pilot", {
                    "entities" : qSketchRegion(id + "pilotSk", true),
                    "endBound" : BoundingType.BLIND,
                    "depth" : bossHeight,
                    "operationType" : NewBodyOperationType.REMOVE
                });

        opDeleteBodies(context, id + "cleanup", {
                    "entities" : qUnion([
                                qCreatedBy(id + "bossSk", EntityType.BODY),
                                qCreatedBy(id + "pilotSk", EntityType.BODY)])
                });
    });

// =====================================================================
//  8. OpenFTC Bearing Pocket
// =====================================================================

annotation { "Feature Type Name" : "OpenFTC Bearing Pocket",
             "Feature Type Description" : "Press-fit bearing pockets (goBILDA 1611 / REV / 608 / 625) at selected sketch points on a face. Part of OpenFTC CAD -- github.com/FTCTeam30401/openftc-cad" }
export const openFtcBearingPocket = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Face to pocket", "Filter" : GeometryType.PLANE, "MaxNumberOfPicks" : 1 }
        definition.face is Query;

        annotation { "Name" : "Pocket centers (sketch points)", "Filter" : EntityType.VERTEX }
        definition.points is Query;

        annotation { "Name" : "Bearing" }
        definition.bearing is OpenFtcBearing;

        annotation { "Name" : "Fit clearance (on diameter)" }
        isLength(definition.clearance, OPENFTC_FIT_CLEARANCE_BOUNDS);
    }
    {
        const spec = openFtcBearingSpec(definition.bearing);
        const dia = spec.od + definition.clearance;

        const facePlane = evPlane(context, { "face" : definition.face });
        const pts = evaluateQuery(context, definition.points);
        if (size(pts) == 0)
            throw regenError("Select at least one sketch point for pocket placement.", ["points"]);

        var sk = newSketchOnPlane(context, id + "sk", { "sketchPlane" : facePlane });
        for (var n = 0; n < size(pts); n += 1)
        {
            const c = openFtcPlanePoint(facePlane, evVertexPoint(context, { "vertex" : pts[n] }));
            skCircle(sk, "c" ~ n, { "center" : c, "radius" : dia / 2 });
        }
        skSolve(sk);

        // Pocket cuts INTO the material (opposite the face's outward normal),
        // one bearing-width deep. If the part is thinner, it becomes a
        // through-bore -- fine for printed plates.
        extrude(context, id + "cut", {
                    "entities" : qSketchRegion(id + "sk", true),
                    "endBound" : BoundingType.BLIND,
                    "depth" : spec.width,
                    "oppositeDirection" : true,
                    "operationType" : NewBodyOperationType.REMOVE
                });

        opDeleteBodies(context, id + "cleanup", {
                    "entities" : qCreatedBy(id + "sk", EntityType.BODY)
                });
    });
