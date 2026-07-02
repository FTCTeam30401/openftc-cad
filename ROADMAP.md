# OpenFTC CAD — Roadmap

The library is built foundation-first. Each phase produces the building blocks the next phase depends on.

> **Re-sequenced 2026-07-02:** FeatureScript was originally Phase 5 ("long-term").
> It is now the **primary build medium** — custom features give teams a parameter
> dialog instead of a file library, and the code lives in this repo as reviewable,
> versioned text. The generator phases below are built *as* FeatureScript.

---

## Phase 1 — Foundation ✅ *(complete except noted items)*

Build the standards that every future model uses.

- [x] **Master variable table** — grids (goBILDA/REV DUO/REV ION/RoBits/VEX), M3–M5 fasteners, heat-set inserts, bearings, material thicknesses, fillet conventions. *Audited & sourced ([master-variables.md](docs/standards/master-variables.md)).*
- [x] **Standard interface definitions** — [goBILDA](docs/standards/interfaces/gobilda.md) · [REV](docs/standards/interfaces/rev.md) · [RoBits](docs/standards/interfaces/robits.md) · [VEX](docs/standards/interfaces/vex.md). *(Servos, motors, electronics still to add.)*
- [x] **First reusable hole pattern** — `gobilda_8mm_grid` spec + built in Onshape (Variable Studio + parametric sketch, regeneration verified).
- [x] **GitHub repository** — [github.com/FTCTeam30401/openftc-cad](https://github.com/FTCTeam30401/openftc-cad) (public; org transfer planned).

**Still open:** verify 2 flagged dimensions from vendor STEP files (REV DUO drilled hole Ø, Extended Motion Pattern row pitch); servo/motor/electronics interface definitions.

## Phase 2 — FeatureScript Generators *(current)*

The heart of the project: custom Onshape features, sourced from [`featurescript/`](featurescript/).

- [x] **OpenFTC Hole Pattern** — vendor-standard grids with mount/native hole sizing, center option, cut-through. *Compiled & cut-tested in Onshape 2026-07-02.*
- [x] **OpenFTC Plate** — plate sized by its hole pattern + optional goBILDA bearing holes (14 mm @ 24 mm sub-grid) + corner fillets. *Generated & verified in Onshape 2026-07-02.*
- [ ] **OpenFTC Gusset** (L / U / T) — print-first ribs, role-aware holes
- [ ] **OpenFTC Heat-Set Boss** — CNC Kitchen/Ruthex-sized insert bosses
- [ ] **OpenFTC Bearing Pocket** — 1611 / 608 / REV pocket geometry
- [ ] VEX square-hole option for the Hole Pattern feature
- [ ] Standoffs / cross members

## Phase 3 — Universal Adapters *(the differentiator)*

Instead of redesigning robots, teams print an adapter. Built as the **OpenFTC Adapter Plate** feature: pick two standards, get one part. The three hardware families ([master-variables.md §1a](docs/standards/master-variables.md#1a-cross-vendor-hardware-families)) define the map:

- [ ] goBILDA ↔ REV DUO (metric bridge, M4↔M3)
- [ ] Metric 8 mm ↔ Imperial 0.5″ (the high-value jump)
- [ ] REV ION ↔ RoBits (near-1:1)
- [ ] VEX ↔ RoBits (square/8-32 ↔ round/10-32)
- [ ] Servo adapters · Bearing adapters · Camera adapters

## Phase 4 — Robot Components

Useful competition parts, generated or curated:

- [ ] Camera mounts · OTOS mounts · Control Hub mounts · Battery mounts
- [ ] Wire clips · Sensor brackets
- [ ] Belt / chain guides · Pulley guards
- [ ] Game-specific parts

## Phase 5 — Distribution & Verification

- [ ] Publish the Feature Studio as a public Onshape document teams can link
- [ ] Exported STEP/STL/3MF examples for teams that don't use Onshape
- [ ] Release ladder in practice: Prototype → Beta → Competition Tested → Verified
- [ ] Print-and-measure validation of generated parts on a real printer

---

## Where things run

| Layer | Location |
|-------|----------|
| Source of truth (data + code) | this repo — [`standards/`](standards/), [`featurescript/`](featurescript/) |
| Live CAD | [Onshape "OpenFTC CAD"](https://cad.onshape.com/documents/c1a27ea89277347b6bb7d589) — Standards, Feature Studio, test studios |
| Distribution | GitHub + (later) public Onshape share link |
