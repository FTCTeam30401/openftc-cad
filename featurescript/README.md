# OpenFTC FeatureScript Generators

This is where OpenFTC CAD's generators live as **code**. FeatureScript is Onshape's
built-in language for custom features — each `.fs` file here becomes a real feature
in Onshape's toolbar with a parameter dialog.

**Why code instead of hand-modeled parts?** A custom feature *is* the guiding
principle made real: a team picks a vendor, types a few numbers, and the geometry
generates. No STL library to search. And because it's text, it's versioned,
reviewable, and testable here in git.

## Pipeline

```
this repo (source of truth)
   └── featurescript/*.fs
         │  pasted/synced into
         ▼
Onshape document "OpenFTC CAD" → Feature Studio
         │  appears automatically in
         ▼
any Part Studio → Custom features menu → parameter dialog → geometry
```

The live Onshape document: https://cad.onshape.com/documents/c1a27ea89277347b6bb7d589

## Features

All features live in one file — [`openftc-features.fs`](openftc-features.fs) — which maps 1:1 to the document's Feature Studio.

| Feature | Status |
|---------|--------|
| **OpenFTC Hole Pattern** | ✅ Working — compiled & cut-tested 2026-07-02 |
| **OpenFTC Plate** | ✅ Working — generated a 9×7 goBILDA plate w/ six 14 mm bearing holes + fillets, 2026-07-02 |
| **OpenFTC Adapter Plate** | ✅ Working — generated a goBILDA(4×5) ↔ RoBits(3×3) adapter, 2026-07-02 |
| **OpenFTC L Gusset** | ✅ Working — generated a 3-wide L bracket, holes on both legs, 2026-07-02 |
| **OpenFTC U Gusset** | ✅ Working — channel bracket, hole grids on all three legs, 2026-07-02 |
| **OpenFTC T Gusset** | ✅ Working — tee bracket, grids on base (both sides) + leg, 2026-07-02 |
| **OpenFTC Heat-Set Boss** | ✅ Working — M3/M4/M5 insert bosses at sketch points, 2026-07-02 |
| **OpenFTC Bearing Pocket** | ✅ Working — 1611/REV/608/625 pockets at sketch points, 2026-07-02 |

### OpenFTC Plate

A printable plate sized by its hole pattern: width = columns × spacing, height =
rows × spacing, holes centered with a half-pitch edge margin (vendor pattern-plate
convention). **Parameters:** standard · columns · rows · thickness · hole size ·
goBILDA bearing holes (14 mm replacing grid holes on a 24 mm sub-grid, outer ring
excluded so bearings never break the edge) · corner fillet radius.

One dialog → one finished part. This is the first feature a team can print a
robot part from.

### OpenFTC Hole Pattern

Generates a vendor-standard hole grid on any plane or planar face and (optionally)
cuts it through every solid in the Part Studio.

**Parameters:** standard (goBILDA / RoBits–REV ION / VEX / REV DUO) · columns ·
rows · hole size (mount-clearance vs native) · center-on-origin · cut-through.

All dimensions trace to [`standards/master-variables.yaml`](../standards/master-variables.yaml)
(v0.2.0, audited). The mount/native distinction matters: goBILDA's real holes are
4.0 mm thread-forming bores; a printed part bolting *onto* goBILDA needs 4.5 mm
clearance. The dialog makes that choice explicit.

**Known limitations (v1):**
- One pattern per feature invocation; bearing-hole interleaving lives in the
  Plate feature (not yet in the bare pattern).

**VEX square holes:** selecting VEX + "replicate vendor member" generates true
0.182 in square holes (verified 2026-07-02); all other combinations are round.

### OpenFTC Adapter Plate

Two hole-pattern zones — any two standards — side by side on one plate. Zone
widths/heights derive from each standard's own spacing; plate height is the
larger of the two. Holes are always mount-clearance (adapters bolt onto both
structures). This is the Phase 3 signature: instead of redesigning a robot,
print the bridge.

### Teardrop leg holes (gusset family)

Gusset leg holes print with horizontal axes (base-down orientation), so their
top arcs sag. All three gussets default to **truncated-teardrop leg holes**:
45-degree self-supporting walls, flat bridge at the top of the circle
(0.828 x radius wide) — the NopSCADlib-standard geometry, researched and
adversarially verified in [print-first.md](../docs/standards/print-first.md).
Toggle off in the dialog for machined parts. Square VEX holes are never
teardropped. Base/plate holes stay circular (vertical axis — no sag).

### OpenFTC L Gusset

Two perpendicular legs sharing a corner, each carrying the standard's grid.
First hole row in each leg sits one half-pitch beyond the other leg's inner
face so holes never collide with the corner. U/T variants and print-first
ribs are planned.

### OpenFTC U Gusset

A channel bracket: base spanning two parallel vertical legs, vendor grid on
all three. Leg hole cuts are blind (one leg's holes never tunnel across the
channel into the other).

### OpenFTC Heat-Set Boss & Bearing Pocket

Print-first hardware placement: sketch points where you want the hardware,
select the face + those points. Bosses use CNC Kitchen/Ruthex pilot sizing
(M3 4.0 / M4 5.6 / M5 6.4 mm) with auto height (insert length + 0.8 mm) and
a wall parameter (default 1.6 mm, the Ruthex minimum). Pockets take a
diametral fit clearance (default 0.15 mm) and cut one bearing-width deep.

## Planned generators

- Print-first ribs for the gusset family
- Servo mount generator (needs servo interface definitions first)

## Editing workflow

1. Edit the `.fs` file here; commit.
2. Copy it to the clipboard (`LANG=en_US.UTF-8 pbcopy < file.fs` — the LANG matters,
   Onshape's editor mangles non-ASCII pasted as MacRoman; keep sources ASCII-only).
3. In the Onshape Feature Studio: select-all, paste, **Commit**.
4. Check the FeatureScript notices panel for errors; fix here, not just in Onshape.

FeatureScript version header must match the Feature Studio's std import
(currently `FeatureScript 3008`). Note: the std `extrude` feature needs its own
`import(path : "onshape/std/extrude.fs", ...)` — `common.fs` doesn't export it.
