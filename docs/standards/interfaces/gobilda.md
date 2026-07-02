# Interface: goBILDA

**System:** metric · **Fastener:** M4 × 0.7 · **Grid:** 8 mm · Confidence: ✅ *(verified from STEP files)*

## Pattern

| Feature | Value |
|---------|-------|
| Grid spacing | 8.0 mm |
| **Native hole Ø** | **4.000 mm** — thread-forming fit for M4 (not a clearance hole) |
| Hole Ø to bolt a **printed part onto** goBILDA | **4.5 mm** (M4 normal clearance) |
| Bearing hole | 14.0 mm, every 24 mm (3 × grid); 4 surrounding holes clocked 45° |
| Plate/channel widths | 32 / 43 / 48 / 96 mm (widths — **not** a bolt-circle footprint) |
| Channels | U 48×48 · Low-Side 12×48 · Mini 12×32 mm |
| Shaft | 6 mm D / 8 mm round; 8 mm & 12 mm REX |
| Bearing | 1611 flanged (8×14×5), 1600 non-flanged (8×22×7 = 608) |

Use pattern `gobilda_8mm_grid` from [hole-patterns.md](../hole-patterns.md).

## When designing a goBILDA-mating part
- **Bolting onto goBILDA?** Use 4.5 mm clearance holes on 8 mm centers so the M4 passes through your part freely.
- **Replicating a goBILDA structural member** (screws thread into your part)? Use 4.0 mm to match native, or a heat-set insert boss (M4 → 5.6 mm).
- 8 mm shaft → 14 mm bearing pocket (1611) or 22 mm (1600/608). REX ≠ round — REX shafts need a REX-profile bore.

Sources & full detail in [master-variables.md](../master-variables.md#1-vendor-grids).
