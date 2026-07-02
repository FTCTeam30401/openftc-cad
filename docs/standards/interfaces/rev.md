# Interface: REV Robotics

REV ships **two incompatible build systems.** Know which one you're mating to.

## REV DUO — metric FTC system ✅ *(exact hole Ø 🔴)*
**Fastener:** M3 · **Motion Pattern pitch:** 16 mm

| Feature | Value |
|---------|-------|
| Extrusion | 15 × 15 mm T-slot |
| U / C channel | 45 × 45 / 45 × 15 mm |
| Motion Pattern | M3 holes on a 16 mm bolt circle, repeating every 16 mm |
| Hole (design default) | 3.4 mm (M3 normal clearance) — exact drilled Ø 🔴 unverified |
| Shaft / bearing | 5 mm hex; 8 mm flanged bearing (49-1559, 8 × 12 × 3.5) + 5 mm-hex insert |

## REV ION — imperial system ✅
**Fastener:** #10-32 · **Pitch:** 0.5 in (12.7 mm)

| Feature | Value |
|---------|-------|
| Hole pitch | 0.5 in |
| Shaft | 1/2" rounded hex (13.75 mm across corners) |
| Bearing OD | 1.125 in |

## When designing a REV-mating part
- **First decide DUO vs ION** — they do not share a pattern.
- DUO: M3, 16 mm pitch, metric everything.
- ION: #10-32, 0.5 in pitch, imperial — closer to RoBits/VEX than to DUO.

🔴 **Verify before final:** DUO exact hole Ø and Extended Motion Pattern pitch from STEP files.
