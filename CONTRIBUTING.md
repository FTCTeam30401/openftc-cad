# Contributing to OpenFTC CAD

## Release Philosophy

Every part moves through four stages. **Nothing is considered finished until it has been used on a real FTC robot.**

```
Prototype  →  Beta  →  Competition Tested  →  Verified
```

| Stage | Meaning |
|-------|---------|
| **Prototype** | Modeled and exported. Geometry believed correct; not yet printed or printed only once. |
| **Beta** | Printed and fit-checked against real hardware. Dimensions confirmed on the bench. |
| **Competition Tested** | Used on a robot during a real match or full practice under load. |
| **Verified** | Survived competition use across time/teams with no dimensional or structural issues. |

Each part's `README.md` states its current stage.

## Design Rules

1. **Parametric or it doesn't ship.** Dimensions come from variables, and standard values come from the [master variable table](docs/standards/master-variables.md) — never hard-coded, never re-typed.
2. **Print-first.** Design for the printer: think about orientation, overhangs, layer-direction strength, captive-nut pockets, and heat-set inserts — don't just copy an aluminum part.
3. **Organize by function, not vendor.** A part lives in `parts/<function>/`; the *vendor standards it supports* are metadata, not the folder.
4. **Cite the standard.** If a part mates to goBILDA, it references the goBILDA interface definition — it does not invent its own spacing.

## Adding a Part

1. Model it in Onshape, driven by the shared variables.
2. Export STEP (source-of-truth geometry), STL, and 3MF into the part's folder.
3. Add a `README.md`: what it is, parameters, supported standards, print settings, and current release stage.
4. Add at least one image.
5. Update the relevant roadmap checkbox.

## Licensing

License is not yet finalized. Leading candidates:
- **CERN-OHL-S / CERN-OHL-W** — purpose-built for open hardware.
- **CC-BY-SA 4.0** — simple, well-understood, share-alike.

Until finalized, treat contributions as intended for a permissive share-alike open-hardware license.
