# Reusable Hole Patterns

A hole pattern defined **once**, reused **everywhere**. This is the first real proof of the project's thesis: describe a pattern with a few parameters, and generate it on demand instead of drawing it by hand every time.

---

## The generic parametric pattern

Every grid-based FTC pattern is the same idea with different numbers:

| Parameter | Symbol | Meaning |
|-----------|--------|---------|
| Spacing | `s` | Center-to-center distance between holes |
| Hole diameter | `d` | Drilled clearance hole |
| Columns × Rows | `nx × ny` | How many holes in each direction |
| Edge margin | `m` | Distance from pattern edge to first hole center |
| Origin | — | Which corner/center the pattern is measured from |

From those five things, any rectangular grid pattern can be generated. In Onshape this becomes: **one sketch of a single hole, constrained to a variable, then a linear pattern driven by `nx`, `ny`, and `s`.** Change the numbers → get a different part. No redrawing.

> This same structure is what the Phase 5 **OpenFTC Hole Pattern** FeatureScript will automate.

---

## First concrete pattern — goBILDA 8 mm grid

The first pattern we standardize, because it's the cleanest and most widely used.

```yaml
name: gobilda_8mm_grid
spacing: 8.0          # mm — from master-variables.yaml -> vendor_grids.gobilda.grid_spacing
hole_diameter: 4.5    # mm — M4 NORMAL clearance (see note below)
fastener: M4
origin: hole-center   # measure the grid from a hole, not a corner
```

**Why `hole_diameter: 4.5` and not "4 mm"?**
goBILDA *labels* its holes "4 mm," but that's design-intent nomenclature. A real M4 fastener needs clearance — the ISO 273 **normal** clearance for M4 is **4.5 mm** (close 4.3 / loose 4.8). We use 4.5 as the sensible printable default and flag it: the true drilled Ø on goBILDA parts is 🔴 **unverified** pending a STEP-file pull. If mating parts fit loose, drop to 4.3.

### How to build it in Onshape (your first hands-on task)

1. New part studio → create a Variable Table with `#grid = 8 mm`, `#hole = 4.5 mm`.
2. Sketch **one** circle, diameter `#hole`, on the origin.
3. Linear pattern the circle: spacing `#grid`, count `nx` in X and `ny` in Y (make `nx`, `ny` variables too).
4. Change `nx`/`ny` and watch the pattern regenerate. That's the whole idea, working.

This is exactly the "Variables + Sketches" skill from **Project 1** of the teaching plan — the hole pattern *is* your first lesson.

---

## Patterns to add next

- `rev_duo_16mm` — REV DUO Motion Pattern (M3, 16 mm pitch) — pending exact hole Ø from STEP.
- `robits_halfinch` — RoBits / VEX 0.5 in grid (0.201 in holes, #10).
- `vex_square` — VEX square-hole 0.5 in grid — pending verified square width.

Each new pattern is one small YAML block + one Onshape sketch. That's the payoff of doing the foundation first.
