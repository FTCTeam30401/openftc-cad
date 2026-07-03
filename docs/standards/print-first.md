# Print-First Design: Holes

How OpenFTC CAD makes holes that survive FDM printing. Researched from primary
sources and adversarially verified 2026-07-03; every value below carries a
confidence tag (see [master-variables.md](master-variables.md) for the legend).

---

## The two kinds of holes

Orientation decides everything:

| Hole axis vs. print bed | Example in this library | Failure mode |
|--------------------------|------------------------|--------------|
| **Vertical** (hole points up) | plate holes, boss pilots, bearing pockets — printed flat | prints slightly **undersized**, but round |
| **Horizontal** (hole through a wall) | **gusset leg holes** when printed base-down | top arc **sags** — undersized *and* out-of-round |

## Why horizontal holes sag ✅

A printer can hold ~45° overhangs (each layer still rests on ~50% of the one
below). But a circular hole's overhang angle sweeps from 0° at the sides to
**90° — a pure unsupported bridge — at the crown**, so the top always violates
the rule, on any printer ([Hubs](https://www.hubs.com/knowledge-base/how-design-parts-fdm-3d-printing/),
[Prusa KB](https://help.prusa3d.com/article/overhangs_2100)).

Two measured mechanisms (plug-gauge validated,
[nophead "Horiholes"](https://hydraraptor.blogspot.com/2020/07/horiholes_36.html)):

1. **Stair-stepping** — the slicer's layer corners intrude into the circle.
2. **Crown droop** ≈ `√(4·layer_height·extrusion_width/π) − layer_height`
   (~0.15 mm at 0.25/0.5 mm settings).

Combined: a horizontal hole typically loses **0.1–0.3 mm** of vertical
diameter before any visible failure. ✅

## The fix: the (truncated) teardrop ✅

Replace the circle's top arc with straight **45° tangent walls** — every
surface is now self-supporting. Two variants:

```
   pointed                truncated (flat-top)
     /\                        ____
    /  \                      /    \        flat bridge = 0.828·r
   |    |                    |      |       walls at 45°
    \__/                      \____/        tangents at (±0.707r, +0.707r)
```

- **Tangent angle: 45° per side (90° included)** — the cross-library consensus
  since the RepRap era ([MCAD](https://github.com/openscad/MCAD/blob/master/teardrop.scad),
  [NopSCADlib](https://github.com/nophead/NopSCADlib/blob/master/utils/core/teardrops.scad),
  [Rahix](https://blog.rahix.de/design-for-3d-printing/)).
  [FRCDesign.org](https://frcdesign.org/design-handbook/structure/design-for-3d-printing/)
  runs slightly steeper (100° included). ✅
- **Pointed apex** sits at `r·√2` above center. ✅
- **Truncated is the modern default** (NopSCADlib defaults to it): clip the
  peak flat at the **top of the original circle** (y = +r). The flat is a
  short bridge, width `2·r·tan(22.5°) ≈ 0.828·r`, and short bridges print
  cleanly. You keep the fastener fit and lose the pointed tip's dead space. ✅
- **Bridge limit ~5 mm** before visible sag — reached around a 12 mm hole, far
  bigger than any fastener hole in this library. ✅

**Thresholds** ✅: under ~6 mm, plain circles print *acceptably* (teardrop
still measurably better); above ~10–12 mm a teardrop (or reorientation /
supports) is essential.

**Fit impact** ✅: a teardrop only *adds* space above the nominal circle —
fasteners pass exactly as before. But **never teardrop a press-fit bearing
pocket**: bearings need full-circumference contact. (Our Bearing Pocket
feature cuts vertical-axis pockets, so this doesn't arise.)

## What OpenFTC CAD does with this

**Gusset leg holes (L / U / T) are truncated teardrops by default** —
"Teardrop leg holes (print-first)" in the dialog, on the assumption the gusset
prints base-down (the natural orientation). Base/plate holes stay circular
(vertical axis — no sag). VEX native *square* holes are left square; if you
must print one horizontally, rotate the part 45° so the square becomes a
self-supporting diamond ✅ (a standard trick — diamond holes are the
drill-out-free alternative).

Geometry encoded (matching NopSCADlib's battle-tested default): circle +
45° tangent walls from (±0.707r, 0.707r), flat top at y = +r spanning
±0.414r, apex direction = up the leg.

## Vertical-axis holes: undersize & compensation

- FDM holes print undersized from extrusion squash + chord faceting; desktop
  accuracy baseline ±0.5% (±0.5 mm floor) ✅
  ([Hubs](https://www.hubs.com/knowledge-base/key-design-considerations-3d-printing/)).
- Our **+0.2 mm diametral clearance** default for printed holes is supported
  by independent sources for the 4–12 mm range on tuned printers ✅.
- Small holes (2–3 mm) vary wildly by printer (~0.1–0.5 mm undersize) — the
  neat compensation tables circulating online are 🟡 folklore; calibrate.
- Our **+0.15 mm bearing-pocket clearance** 🟡: sound *starting point* — it
  assumes typical (~0.15–0.2 mm) undersize, netting out to a light press fit.
  **Caveat (verified):** if your slicer's hole compensation is enabled, the
  same pocket comes out a slip fit. Print a calibration coupon per printer;
  the feature's clearance parameter exists precisely for this.
- Where to compensate 🔵 (our convention): printer-*agnostic* geometry
  (teardrops, fit clearances) lives in CAD; printer-*specific* correction
  (X-Y hole compensation, shrinkage) belongs in the slicer profile.

## Ecosystem notes ✅

- Onshape already has community "3D Printed Hole" / FRC "Prepare to Print"
  FeatureScripts — validation that CAD-side teardrops are standard practice.
- **gm0.org's 3D-printing guide has no hole-design/teardrop section** — this
  document covers ground the main FTC reference doesn't.

*Research gap (honest ledger): the dedicated community-practice survey agent
failed twice on infrastructure errors; its key questions were answered by the
verification passes above, but a deeper sweep of team engineering notebooks
is still open.*
