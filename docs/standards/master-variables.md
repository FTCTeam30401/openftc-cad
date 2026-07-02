# Master Variable Table

The single source of truth for every dimension in OpenFTC CAD. The machine-readable version lives at [`standards/master-variables.yaml`](../../standards/master-variables.yaml); this page is the human-readable mirror with sources.

**v0.2.0** — audited against vendor STEP files & engineering drawings via an independent re-verification + adversarial-challenge pass. **Golden rule:** parts reference these values. Nobody re-types a dimension.

### Confidence legend

| Tag | Meaning |
|-----|---------|
| ✅ **verified** | Confirmed from a primary source (STEP/drawing) or ≥2 authoritative sources, adversarially cross-checked |
| 🟡 **moderate** | Single/secondary source, or a derived value |
| 🔴 **unverified** | Do **not** trust until pulled from a vendor STEP/drawing or physically measured |
| 🔵 **convention** | An OpenFTC design default, not a vendor standard |

All lengths in **mm** unless noted. `1 inch = 25.4 mm` (exact).

---

## 1. Vendor Grids

### goBILDA ✅ *(metric, M4)*
| Parameter | Value |
|-----------|-------|
| Grid spacing | **8.0 mm** (verified from 1120 U-channel STEP) |
| **Native hole Ø** | **4.000 mm** — a *thread-forming* fit for M4, **not** a clearance hole (verified from STEP geometry; goBILDA sells M4 thread-forming screws + taps to match) |
| Hole Ø for a **printed part bolting onto** goBILDA | 🔵 **4.5 mm** (M4 normal clearance; 4.3 close / 4.8 loose) |
| Fastener | **M4 × 0.7** |
| Bearing hole | **14.0 mm**, every **24 mm** (3 × grid); the 4 pattern holes around it are clocked **45°** |
| Plate/channel *widths* | 32 / 43 / 48 / 96 mm — **widths, not a bolt pattern** (there is no discrete "32×32" footprint) |
| Channels | U-channel 48×48 · Low-Side 12×48 · Mini Low-Side 12×32 mm |
| Shafts | 6 mm D & 8 mm round; 8 mm REX (8 mm round + 7 mm hex) & 12 mm REX |
| goRAIL | same 8 mm grid via slot/"hurricane" nuts — no distinct spacing |

> **The 4.0 vs 4.5 distinction is the whole game.** goBILDA's aluminum is 4.0 mm (screws thread-form into it). A *printed* part that bolts *through* onto goBILDA needs a 4.5 mm clearance hole so the M4 passes freely. Use `gobilda_8mm_grid` from [hole-patterns.md](hole-patterns.md).

Sources: [goBILDA Pattern](https://www.gobilda.com/pattern), goBILDA STEP files (1106 beam, 1120 U-channel), [Game Manual 0](https://gm0.org/en/latest/docs/hardware-components/kit-and-hardware-guide/gobilda.html).

### REV DUO *(metric FTC system, M3)* ✅ *(drilled hole Ø 🔴)*
| Parameter | Value |
|-----------|-------|
| Extrusion | **15 × 15 mm** T-slot (also 15×30; 15×45 C-channel) |
| U-channel | **45 × 45 mm** |
| **Mounting hole pitch** | **8 mm** (M3 mounting holes on an 8 mm pitch) |
| Motion Pattern | M3 holes on a **16 mm-diameter bolt circle** (for shaft accessories) |
| Bearing/shaft interval | every **16 mm** along the channel |
| Bearing seat | **9 mm** dia (Delrin bearings, 9 mm OD) |
| Fastener | **M3 × 0.5** |
| Drilled hole Ø | 🔴 **unverified** — REV never publishes it; M3 clearance ≈ 3.2–3.4 |
| Shaft / bearing | 5 mm hex; 8×12×3.5 ball bearing (REV-49-1559) + 5 mm-hex→8 mm insert (REV-41-1528) |

> ⚠️ Earlier drafts called the pitch "16 mm" — **it's 8 mm.** The 16 mm figures are the bolt-circle diameter and the bearing interval. Sources: REV 15mm Building System Guide (2017), [REV docs](https://docs.revrobotics.com/), [GM0](https://gm0.org/en/latest/docs/hardware-components/kit-and-hardware-guide/rev-robotics.html).

### REV ION *(imperial system — distinct from DUO)* ✅
| Parameter | Value |
|-----------|-------|
| Hole pitch | **0.5 in (12.7 mm)** |
| Fastener | **#10-32** (#10 clearance ≈ 0.196 in / 4.98 mm) |
| Shafts | ½" hex; **½" rounded hex** (bore ID **13.75 mm / 0.541 in**); **MAXSpline** — a *distinct* spline profile |
| Bearing OD | 1.125 in (28.58 mm) standard · MAXSpline bearing 1 in (25.4 mm) · #10-clearance bearing 7/8 in (22.22 mm) |
| MAXTube | 1×1 in (also 2×2, 2×1, 0.5×0.5); #10 holes 0.5 in apart; 3.175 mm wall on 2" faces |

> ⚠️ **MAXSpline ≠ ½" rounded hex** — they're different profiles (an earlier draft conflated them). And 13.75 mm is the rounded-hex **bore ID**, not "½" hex across corners." **MAXTube is an ION part** (imperial), not DUO. Source: [REV ION system standards](https://docs.revrobotics.com/ion-build/home/system-standards).

### AndyMark RoBits ✅ *(cleanest-specified vendor)*
| Parameter | Value |
|-----------|-------|
| Grid spacing | **0.5 in (12.7 mm)** |
| Hole Ø | **0.201 in (5.11 mm)** — #10 clearance |
| Fastener | **#10-32** — AndyMark uses #10-32 **broadly across product lines** |
| Gusset resolution | 0.25 in (6.35 mm) |
| Bearing-hole pitch | 1.5 in (38.1 mm) on 1×1 tube |
| Tubes | 0.5×0.5, 1×0.5, 1×1 in; wall 0.063 in (1.6 mm); 6061-T6 |
| Shaft | 3/8 in hex (9.525 mm), #10-32 both ends |

Source: [AndyMark – How Do I Use RoBits](https://andymark.com/pages/how-do-i-use-robits).

### VEX (EDR / V5 metal) ✅
| Parameter | Value |
|-----------|-------|
| Hole pitch | **0.5 in (12.7 mm)** |
| Hole shape | **square** |
| **Hole width** | **0.182 in (4.62 mm)** — ✅ resolved from VEX drawing **PN 276-2600** |
| Fastener | **#8-32** |
| Channel naming | AxBxCxL (flange × web × flange in half-inch hole counts, × length in holes) |

> Excludes VEX IQ (plastic snap system). The square-hole width was previously 🔴 unknown — now verified from a primary VEX engineering drawing. Sources: VEX drawing 276-2600, [Purdue SIGBots](https://wiki.purduesigbots.com/hardware/misc.-vex-parts-1/structure/c-channels-and-angles).

---

## 1a. Cross-Vendor Hardware Families *(drives adapter design — Phase 3)*

There are **three** families. Adapters bridge *between* them.

| Family | Members | Grid | Fastener | Hole |
|--------|---------|------|----------|------|
| **Imperial 0.5″ / #10-32** | RoBits + **REV ION** | 0.5 in | #10-32 | round ~0.20 in |
| **Imperial 0.5″ / #8-32 square** | VEX | 0.5 in | #8-32 | 0.182 in **square** |
| **Metric 8 mm grid** | goBILDA (M4) + **REV DUO** (M3) | 8 mm | M4 / M3 | round |

**Consequences:** RoBits ↔ REV ION are ~1:1 on the imperial grid (easiest adapters). VEX shares the 0.5″ pitch but needs a distinct adapter (different thread *and* square holes). goBILDA ↔ REV DUO share the metric 8 mm world but differ in fastener (M4 vs M3). The metric↔imperial jump is the hardest — and the most valuable adapter to nail.

---

## 2. Fasteners — M3 / M4 / M5 ✅

*Metric, ISO/DIN. Cross-verified by independent + adversarial passes; the M5 nut thickness (4.7) was corrected during review.*

| | M3 | M4 | M5 |
|---|---|---|---|
| **Clearance hole** — close / normal / loose (ISO 273) | 3.2 / 3.4 / 3.6 | 4.3 / 4.5 / 4.8 | 5.3 / 5.5 / 5.8 |
| **Tap drill** (coarse) | 2.5 | 3.3 | 4.2 |
| **SHCS head Ø** (ISO 4762 / DIN 912 max) | 5.5 | 7.0 | 8.5 |
| **SHCS head height** | 3.0 | 4.0 | 5.0 |
| **Hex key** | 2.5 | 3.0 | 4.0 |
| **Hex nut** across-flats (ISO 4032 / DIN 934) | 5.5 | 7.0 | 8.0 |
| **Hex nut** thickness | 2.4 | 3.2 | **4.7** |
| **Hex nut** across-corners (WAF × 1.155) | 6.35 | 8.08 | 9.24 |

**Captive-nut pocket** 🔵: pocket across-flats = nut AF **+ ~0.4**, depth = nut thickness **+ ~0.2**. Size by across-flats but *verify it clears across-corners*. Print a fit gauge per printer.

Sources: [AmesWeb ISO 273](https://amesweb.info/screws/Metric-Clearance-Hole-Chart.aspx), [Fuller Fasteners DIN 912](https://fullerfasteners.com/tech/din-912-specifications-hex-socket-head-cap-screws/), [fasteners.eu ISO 4032](https://www.fasteners.eu/standards/iso/4032/).

---

## 3. Heat-Set Threaded Inserts ✅ *(CNC Kitchen / Ruthex)*

*The de-facto FTC community insert (both brands dimensionally identical, adversarially confirmed). `hole Ø` = the value you model the receiving boss to.*

| Size | Hole Ø | Insert length | Insert OD | McMaster 94459A (≈) |
|------|--------|---------------|-----------|---------------------|
| M3 | **4.0** | 5.7 | 4.6 | 4.2 🔴 |
| M4 | **5.6** | 8.1 | 6.3 | 5.3 🔴 |
| M5 | **6.4** | 9.5 | 7.1 | 6.4 🔴 |

Min plastic wall around an insert: **~1.6 mm**. If fit is tight/loose, adjust in 0.1 mm steps and test-print. McMaster values remain 🔴 (JS pages unreadable).

Sources: [CNC Kitchen M3](https://cnckitchen.store/products/heat-set-insert-m3-x-5-7-100-pieces), [3DJake Ruthex](https://www.3djake.com/ruthex/threaded-inserts-m4-50-pieces), [Ruthex HSS drill set](https://www.ruthex.de).

---

## 4. Bearings (bore × OD × width, mm) ✅

| Bearing | Bore | OD | Width | Notes |
|---------|------|-----|-------|-------|
| goBILDA 1611 flanged | 8 (REX/round) | 14 | 5 | flange Ø ~15; 6 mm & 1/4″ bore variants exist |
| goBILDA 1600 non-flanged | 8 | 22 | 7 | = **608** skate bearing |
| REV 49-1559 flanged | 8 | 12 | 3.5 | flange Ø 13.6 / thk 0.8; pairs w/ 5 mm-hex insert |
| 608 | 8 | 22 | 7 | classic skate bearing |
| 625 | 5 | 16 | 5 | |
| 626 | 6 | 19 | 6 | |
| 688 | 8 | 16 | 5 | also exists in 4 mm width — specify |
| MR105 | 5 | 10 | 4 | |
| MR115 | 5 | 11 | 4 | |
| 6800 | 10 | 19 | 5 | thin section |

> REV uses 5 mm hex shafts but has no discrete 5 mm-bore metal bearing — it uses the 8 mm bearing + a 5 mm-hex→8 mm-round insert.

---

## 5. Material / Sheet Thicknesses (mm) ✅

| Nominal | mm | Use |
|---------|-----|-----|
| 1/16" | 1.588 | light panels |
| 1/8" | 3.175 | **most common** structural/drivetrain plate |
| 3/16" | 4.763 | heavier plates (polycarb often measures ~4.5) |
| 1/4" | 6.35 | heavy drivetrain |
| metric | 3.0, 6.0 | also stocked directly |

COTS: goBILDA pattern plate **2.5 mm** ✅ · goBILDA 1140 baseplate **6 mm** · REV ION MAXTube 2×1 wall **3.175 mm** *(ION, imperial)*.

**FDM walls** 🔵 (0.4 mm nozzle): absolute min **0.8 mm** (2 perimeters, non-structural); recommended **1.6 mm** (4 perimeters) for load-bearing FTC parts; **2.0 mm+** for heavily-loaded regions. Keep walls integer multiples of nozzle width.

Sources: [Game Manual 0 – Materials](https://gm0.org/en/latest/docs/custom-manufacturing/materials-guide.html), [Robosource FTC polycarb](https://www.robosource.net).

---

## 6. Project Conventions 🔵

| Convention | Value |
|------------|-------|
| Standard fillet set | 1, 2, 3 mm (pick smallest that carries the load) — *OpenFTC choice, no external source* |
| FDM hole clearance | +0.2 mm on hole **diameter** (= +0.1 mm/side) over nominal |

---

## Remaining verification tasks

Most 🔴 items are now resolved. Still open:

1. **REV DUO** exact drilled hole Ø (M3 clearance) — REV never publishes it; measure a STEP file.
2. **REV DUO** Extended Motion Pattern row pitch — REV states an 8 mm base pitch + equilateral-triangle grid; the ~6.93 mm row spacing is a *derivation* (8·sin60°), not published.
3. **McMaster 94459A** heat-set per-size values — confirm on mcmaster.com (their pages are JS-gated).
