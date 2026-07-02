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
                skCircle(sketch, "hole_" ~ i ~ "_" ~ j, {
                            "center" : vector(x0 + i * s, y0 + j * s),
                            "radius" : d / 2
                        });
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
                if (definition.bearingHoles &&
                    definition.standard == OpenFtcStandard.GOBILDA &&
                    i > 0 && i < definition.nx - 1 &&
                    j > 0 && j < definition.ny - 1 &&
                    (i - 1) % 3 == 0 && (j - 1) % 3 == 0)
                {
                    dia = GOBILDA_BEARING_DIA;
                }
                skCircle(holes, "h_" ~ i ~ "_" ~ j, {
                            "center" : vector(x, y),
                            "radius" : dia / 2
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

        // --- tidy: remove construction sketches ---------------------------
        opDeleteBodies(context, id + "cleanup", {
                    "entities" : qUnion([
                                qCreatedBy(id + "outline", EntityType.BODY),
                                qCreatedBy(id + "holes", EntityType.BODY)])
                });
    });
