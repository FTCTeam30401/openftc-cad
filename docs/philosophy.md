# Design Philosophy

## The one sentence

> **If a team can describe the part they need in a few parameters, OpenFTC CAD should be able to generate it — instead of requiring them to search for it.**

When a design decision is unclear, return here. The question is always: *does this move us toward a generator, or toward another pile of files?*

## Generators, not files

A repository of STLs grows linearly — every new size is a new file someone has to make, name, store, and find. A generator grows the *space* of parts it can produce with every parameter added. One well-built generator replaces a category.

This is why Phase 1 is not a part. The master variables and interface definitions are what let a generator "know" what a goBILDA hole pattern *is*, so it can place one on demand.

## Print-first, not aluminum-mimicry

3D printing is not a worse way to make an aluminum part — it's a different manufacturing process with its own strengths. We design *for* it:

- **Internal ribs** instead of solid mass.
- **Captive-nut pockets** and **heat-set inserts** instead of tapped threads in plastic.
- **Print-orientation-aware geometry** — strength follows the layer direction.
- **Cable management** built into the structure.
- **Lightweight** by default; add material only where load requires it.

## Vendor-neutral, combination-friendly

The FTC ecosystem is fragmented across REV, goBILDA, RoBits, and VEX. Most teams own a mix. The library's job is not to pick a winner — it's to make the seams between systems disappear. That's why **adapters** (Phase 3) are treated as the project's signature contribution.

## Competition-tested truth

A part is a hypothesis until a robot proves it. The release stages (Prototype → Beta → Competition Tested → Verified) exist so a team can tell, at a glance, how much they can trust a part before a match.
