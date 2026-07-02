# OpenFTC CAD

> **Mission:** Create the best open-source parametric CAD library for FIRST Tech Challenge (FTC).

OpenFTC CAD is not a dumping ground for STL files. It is a **toolkit that generates the right part for the job.**

## Guiding Principle

> **If a team can describe the part they need in a few parameters, OpenFTC CAD should be able to generate it — instead of requiring them to search for it.**

Everything in this project is measured against that sentence.

## What Makes This Different

Rather than shipping hundreds of individual parts:

```
3x3 gusset   3x4 gusset   4x5 gusset   5x6 gusset   ...
```

we ship **configurable generators**:

```
Gusset Generator
  ├─ width
  ├─ height
  ├─ thickness
  ├─ hole pattern
  ├─ material style
  └─ reinforcement options
```

One model generates hundreds of parts.

## The Four Emphases

1. **Parametric** — everything driven by variables; no hard-coded dimensions wherever avoidable.
2. **Print-first** — designed for additive manufacturing, not copied from aluminum: internal ribs, captive-nut pockets, heat-set inserts, cable management, print-orientation-aware geometry.
3. **Vendor-neutral** — support REV, goBILDA, AndyMark RoBits, and VEX. Make it easy to *combine* ecosystems instead of forcing teams to pick one.
4. **Competition-tested** — nothing is "finished" until it has been used on a real FTC robot.

## Toolchain

| Layer | Tool |
|-------|------|
| Source CAD | [Onshape](https://www.onshape.com) (parametric, cloud, free for FTC) |
| Standards source-of-truth | Machine-readable files in [`standards/`](standards/) |
| Distribution | This GitHub repository |

GitHub carries the documentation, exported STEP / STL / 3MF files, images, release notes, and roadmap. Onshape holds the live parametric models.

## Repository Layout

```
openftc-cad/
├── docs/            Documentation & the standards library (human-readable)
│   └── standards/   Master variables + per-vendor interface definitions
├── standards/       Machine-readable source-of-truth (the numbers)
├── parts/           Organized by FUNCTION, not by vendor
│   ├── structural/  Plates, brackets, gussets, cross members
│   ├── motion/      Bearing blocks, standoffs, pulleys, guides
│   ├── electronics/ Hub / battery / driver-station mounts
│   ├── sensors/     Camera, OTOS, distance-sensor brackets
│   ├── adapters/    Cross-vendor adapters (REV↔goBILDA↔RoBits↔VEX)
│   ├── utility/     Wire clips, cable management, misc
│   └── examples/    Reference assemblies showing parts in use
├── releases/        Release notes & versioned exports
└── assets/          Shared images
```

Each part can support **one or more mounting standards** — that's why we organize by function, not vendor.

## Project Status

⚙️ **Phase 2-3 — the generators are live.** Four custom Onshape features, generated from the audited standards and verified in CAD (2026-07-02):

| Generator | What one dialog produces |
|-----------|--------------------------|
| **OpenFTC Hole Pattern** | any vendor's hole grid on any face — incl. true VEX 0.182″ square holes |
| **OpenFTC Plate** | a printable pattern plate; optional goBILDA 14 mm bearing holes; corner fillets |
| **OpenFTC Adapter Plate** | *the differentiator* — any two standards bridged on one part |
| **OpenFTC L Gusset** | an L bracket with vendor grids on both legs |

Source: [`featurescript/openftc-features.fs`](featurescript/openftc-features.fs) · Progress: [ROADMAP.md](ROADMAP.md)

## License

TBD — see [CONTRIBUTING.md](CONTRIBUTING.md#licensing). (A permissive hardware/design license such as CERN-OHL or CC-BY-SA is under consideration.)

---

*An [Anthem Robotics](https://anthemrobotics.com) open-source project.*
