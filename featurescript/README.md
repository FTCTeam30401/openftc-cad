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

| Feature | File | Status |
|---------|------|--------|
| **OpenFTC Hole Pattern** | [`openftc-hole-pattern.fs`](openftc-hole-pattern.fs) | ✅ Working — compiled & cut-tested 2026-07-02 |

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
- VEX native holes are actually 0.182 in *square*; v1 generates round holes
  (correct for bolt-through use, which is the common case for printed parts).
- One pattern per feature invocation; no bearing-hole interleaving yet
  (goBILDA 14 mm bearing holes every 24 mm are planned as an option).

## Planned generators

- **OpenFTC Plate** — outline + auto hole pattern + optional bearing holes + corner fillets
- **OpenFTC Gusset** (L / U / T) — print-first ribs, role-aware holes
- **OpenFTC Adapter Plate** — two standards, one part (the Phase 3 signature)
- **OpenFTC Heat-Set Boss** — CNC Kitchen/Ruthex-sized insert bosses
- **OpenFTC Bearing Pocket** — 1611/608/REV pocket geometry

## Editing workflow

1. Edit the `.fs` file here; commit.
2. Copy it to the clipboard (`LANG=en_US.UTF-8 pbcopy < file.fs` — the LANG matters,
   Onshape's editor mangles non-ASCII pasted as MacRoman; keep sources ASCII-only).
3. In the Onshape Feature Studio: select-all, paste, **Commit**.
4. Check the FeatureScript notices panel for errors; fix here, not just in Onshape.

FeatureScript version header must match the Feature Studio's std import
(currently `FeatureScript 3008`). Note: the std `extrude` feature needs its own
`import(path : "onshape/std/extrude.fs", ...)` — `common.fs` doesn't export it.
