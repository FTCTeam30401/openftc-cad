# Interface: goBILDA

**System:** metric · **Fastener:** M4 · **Grid:** 8 mm · Confidence: ✅ *(exact hole Ø 🔴)*

## Pattern

| Feature | Value |
|---------|-------|
| Grid spacing | 8 mm |
| Hole (design default) | 4.5 mm (M4 normal clearance) — label says "4 mm" |
| Bearing hole | 14 mm |
| Block mounting pattern | 32 × 32 mm (also 16 × 32); tapped grid 16 mm |
| Shaft | 8 mm round or **REX** (proprietary) |
| Bearing | 1611 flanged (8 × 14 × 5), 1600 non-flanged (8 × 22 × 7 = 608) |

Use pattern `gobilda_8mm_grid` from [hole-patterns.md](../hole-patterns.md).

## When designing a goBILDA-mating part
- Grid holes on 8 mm centers, `#hole = 4.5`.
- If it carries an 8 mm shaft, use the 14 mm bearing pocket (1611) or 22 mm (1600/608).
- REX vs round matters — REX shafts need a REX-profile bore; don't assume round.

🔴 **Verify before final:** true drilled hole Ø from goBILDA's STEP file; any goRAIL-specific spacing. Sources in [master-variables.md](../master-variables.md#1-vendor-grids).
