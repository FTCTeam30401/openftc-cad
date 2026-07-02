# OpenFTC CAD — Roadmap

The library is built foundation-first. Each phase produces the building blocks the next phase depends on.

---

## Phase 1 — Foundation *(current)*

Build the standards that every future model uses. **This is not a printable part — it's the bedrock.**

- [x] **Master variable table** — grids (goBILDA/REV DUO/REV ION/RoBits/VEX), M3–M5 fasteners, heat-set inserts, bearings, material thicknesses, fillet conventions. *Data verified & sourced; 3 items flagged for STEP-file confirmation. See [master-variables.md](docs/standards/master-variables.md).*
- [x] **Standard interface definitions** — per-ecosystem specs for [goBILDA](docs/standards/interfaces/gobilda.md), [REV](docs/standards/interfaces/rev.md), [RoBits](docs/standards/interfaces/robits.md), [VEX](docs/standards/interfaces/vex.md). *(Servos, motors, electronics interfaces still to add.)*
- [~] **First reusable hole pattern** — parametric spec written ([hole-patterns.md](docs/standards/hole-patterns.md), `gobilda_8mm_grid`). **Onshape build is the next hands-on session** (Teaching Project 1: Variables + Sketches).
- [x] **GitHub repository** with the full project structure. *(scaffolded locally; push to Anthem Robotics org pending)*

**Still open in Phase 1:** verify the 3 flagged hole dimensions from vendor STEP files; add servo/motor/electronics interface definitions; build the first hole pattern in Onshape.

## Phase 2 — Core Structural Library

Configurable generators:

- [ ] Flat plates
- [ ] L brackets
- [ ] U gussets
- [ ] T gussets
- [ ] Bearing blocks
- [ ] Standoffs
- [ ] Cross members

## Phase 3 — Universal Adapters *(the differentiator)*

Instead of redesigning robots, teams print an adapter.

- [ ] REV → goBILDA
- [ ] REV → RoBits
- [ ] RoBits → VEX
- [ ] goBILDA → VEX
- [ ] Servo adapters
- [ ] Bearing adapters
- [ ] Camera adapters

## Phase 4 — Robot Components

Useful competition parts.

- [ ] Camera mounts
- [ ] OTOS mounts
- [ ] Control Hub mounts
- [ ] Battery mounts
- [ ] Wire clips
- [ ] Sensor brackets
- [ ] Belt guides / chain guides / pulley guards
- [ ] Game-specific parts

## Phase 5 — FeatureScripts *(long-term)*

Custom Onshape tools, designed so they can be added naturally once the library matures:

- [ ] OpenFTC Plate
- [ ] OpenFTC Hole Pattern
- [ ] OpenFTC Bearing Pocket
- [ ] OpenFTC Heat-Set Insert
- [ ] OpenFTC Servo Mount
- [ ] OpenFTC Adapter Plate

> FeatureScript is a real programming language. Phase 5 is code, and code lives in this repo alongside the models.

---

## Immediate Next Steps (Phase 1)

1. Master variable table.
2. Reusable standards library.
3. First reusable hole pattern.
4. GitHub repository with project structure. ✅

Once those are in place, we build the **first configurable generator** (the Gusset Generator is the likely candidate).
