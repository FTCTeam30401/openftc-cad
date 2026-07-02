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
spacing: 8.0          # mm — master-variables.yaml -> vendor_grids.gobilda.grid_spacing
hole_diameter: 4.5    # mm — DEFAULT for a printed part bolting ONTO goBILDA (see below)
fastener: M4
origin: hole-center   # measure the grid from a hole, not a corner
```

**The hole diameter depends on the part's role — this is the important bit.**

goBILDA's own aluminum has **4.000 mm** holes (verified from their STEP files). That is *not* a loose clearance hole — it's a thread-forming fit, and goBILDA sells M4 thread-forming screws and taps to cut threads directly into it. So:

| Your part's role | Hole Ø | Why |
|------------------|--------|-----|
| **Bolts *onto* goBILDA** (screw passes through your printed part) | **4.5 mm** (M4 normal clearance) | the M4 must pass through freely; 4.0 mm would bind |
| **Replicates a goBILDA member** (screw threads *into* your part) | 4.0 mm, or a heat-set boss (5.6 mm for M4) | match native / give the insert material to grip |

The default above is **4.5 mm** because the common case is a printed part bolting onto existing goBILDA structure. If two mating parts fit loose, drop to 4.3 (close); if tight, go 4.8 (loose).

### How to build it in Onshape (your first hands-on task)

1. New part studio → create a Variable Table with `#grid = 8 mm`, `#hole = 4.5 mm`.
2. Sketch **one** circle, diameter `#hole`, on the origin.
3. Linear pattern the circle: spacing `#grid`, count `nx` in X and `ny` in Y (make `nx`, `ny` variables too).
4. Change `nx`/`ny` and watch the pattern regenerate. That's the whole idea, working.

This is exactly the "Variables + Sketches" skill from **Project 1** of the teaching plan — the hole pattern *is* your first lesson.

---

## Patterns to add next

- `robits_halfinch` — RoBits / REV ION 0.5 in grid (0.201 in holes, #10-32). *(imperial 0.5″ family)*
- `vex_square` — VEX 0.5 in grid, 0.182 in **square** holes, #8-32.
- `rev_duo_8mm` — REV DUO M3 mounting grid (8 mm pitch) — drilled Ø pending STEP verification.

Each new pattern is one small YAML block + one Onshape sketch. That's the payoff of doing the foundation first.
