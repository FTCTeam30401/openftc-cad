FeatureScript 3008;
import(path : "onshape/std/common.fs", version : "3008.0");
import(path : "onshape/std/extrude.fs", version : "3008.0");

/*
 * =====================================================================
 *  OpenFTC Hole Pattern -- the first OpenFTC CAD generator
 * =====================================================================
 *  Generates a vendor-standard hole grid on any plane or planar face,
 *  and (optionally) cuts it through every solid in the Part Studio.
 *
 *  Every dimension below is traceable to the audited source of truth:
 *  standards/master-variables.yaml (v0.2.0) in
 *  https://github.com/FTCTeam30401/openftc-cad
 *
 *  Vendor data (verified -- see repo for sources & confidence tags):
 *    goBILDA          8 mm grid,  M4  -- native bore 4.0 mm (thread-forming),
 *                                        mount clearance 4.5 mm (ISO 273 normal)
 *    RoBits / REV ION 0.5 in grid, #10 -- 0.201 in clearance
 *    VEX EDR          0.5 in grid, #8  -- native holes are 0.182 in SQUARE;
 *                                        round #8 clearance 0.177 in for bolting through
 *    REV DUO          8 mm pitch,  M3  -- drilled dia unpublished by REV;
 *                                        ISO 273 normal clearance 3.4 mm used
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
        // NOTE: true VEX native holes are 0.182 in SQUARE (VEX drawing 276-2600).
        // v1 generates round holes; square-profile option is planned.
        return { "spacing" : 0.5 * inch, "mount" : 0.177 * inch, "native" : 0.182 * inch };
    // REV DUO -- REV publishes only "M3 clearance"; ISO 273 normal used for both roles.
    return { "spacing" : 8 * millimeter, "mount" : 3.4 * millimeter, "native" : 3.4 * millimeter };
}

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
