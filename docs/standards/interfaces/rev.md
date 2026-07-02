# Interface: REV Robotics

REV ships **two incompatible build systems.** Know which one you're mating to.

## REV DUO — metric FTC system ✅ *(drilled hole Ø 🔴)*
**Fastener:** M3 × 0.5 · **Mounting pitch:** 8 mm

| Feature | Value |
|---------|-------|
| Extrusion | 15 × 15 mm T-slot (also 15×30; 15×45 C-channel) |
| U-channel | 45 × 45 mm |
| **Mounting hole pitch** | **8 mm** (M3 mounting holes on 8 mm pitch) |
| Motion Pattern | M3 holes on a **16 mm-dia bolt circle** (shaft accessories) |
| Bearing/shaft interval | every **16 mm** along the channel |
| Bearing seat | **9 mm** dia (Delrin bearings) |
| Drilled hole Ø | 🔴 unverified — M3 clearance ≈ 3.2–3.4; measure a STEP |
| Shaft / bearing | 5 mm hex; 8×12×3.5 bearing (REV-49-1559) + 5 mm-hex→8 mm insert (REV-41-1528) |

> The general mounting pitch is **8 mm**. The 16 mm figures are the bolt-circle *diameter* and the *bearing interval* — don't confuse them.

## REV ION — imperial system ✅
**Fastener:** #10-32 · **Pitch:** 0.5 in (12.7 mm)

| Feature | Value |
|---------|-------|
| Hole pitch | 0.5 in; #10 clearance ≈ 0.196 in (4.98 mm) |
| Shafts | ½" hex; **½" rounded hex** (bore ID 13.75 mm / 0.541 in); **MAXSpline** (distinct spline) |
| Bearing OD | 1.125 in std · MAXSpline bearing 1 in · #10-clearance bearing 7/8 in |
| MAXTube | 1×1 in (also 2×2, 2×1, 0.5×0.5); wall 3.175 mm on 2" faces |

> **MAXSpline is its own profile — not the ½" rounded hex.** 13.75 mm is the rounded-hex *bore ID*, not "½″ across corners." MAXTube belongs to ION (imperial).

## When designing a REV-mating part
- **First decide DUO vs ION** — they share no pattern.
- DUO: M3, 8 mm hole pitch, 16 mm bearing interval, metric.
- ION: #10-32, 0.5 in pitch, imperial — shares the 0.5″/#10-32 family with AndyMark RoBits.

🔴 **Verify before final:** DUO drilled hole Ø from a STEP file.
