# Master Variable Table

The single source of truth for every dimension in OpenFTC CAD. The machine-readable version lives at [`standards/master-variables.yaml`](../../standards/master-variables.yaml); this page is the human-readable mirror with sources.

**Golden rule:** parts reference these values. Nobody re-types a dimension.

### Confidence legend

| Tag | Meaning |
|-----|---------|
| ✅ **verified** | Confirmed against ≥2 authoritative sources |
| 🟡 **moderate** | Single/secondary source, or a nominal-vs-actual gap |
| 🔴 **unverified** | Do **not** trust until pulled from a vendor STEP file / drawing or physically measured |
| 🔵 **convention** | An OpenFTC design default, not a vendor standard |

All lengths in **mm** unless noted. `1 inch = 25.4 mm` (exact).

---

## 1. Vendor Grids

### goBILDA ✅ *(hole Ø 🔴)*
| Parameter | Value |
|-----------|-------|
| Grid spacing | **8 mm** (the "pattern") |
| Hole (label) | "4 mm" — goBILDA's nomenclature |
| Hole (actual drilled Ø) | 🔴 **unverified** — real M4 clearance ≈ 4.3–4.5; pull STEP |
| Fastener | **M4** |
| Bearing hole | **14 mm** |
| Secondary pattern | **32 × 32 mm** square (also 16 × 32); tapped grid 16 mm |

Sources: [goBILDA Pattern](https://www.gobilda.com/pattern), [Game Manual 0 – goBILDA](https://gm0.org/en/latest/docs/hardware-components/kit-and-hardware-guide/gobilda.html), cross-confirmed by [AndyMark grid gusset](https://andymark.com/products/8-mm-to-1-2-in-grid-gusset).

### REV DUO *(metric FTC system)* ✅ *(hole Ø 🔴)*
| Parameter | Value |
|-----------|-------|
| Extrusion | **15 × 15 mm** T-slot |
| U-channel / C-channel | **45 × 45** / **45 × 15 mm** |
| Motion Pattern pitch | mounting **every 16 mm** |
| Motion Pattern geometry | M3 holes on a **16 mm** bolt circle |
| Fastener | **M3** |
| Hole (actual Ø) | 🔴 **unverified** — M3 clearance ≈ 3.2–3.4; pull STEP |
| MAXTube 2×1 wall | 3.175 mm (2" face) / ~1 mm (1" face) |

Sources: [GM0 – REV](https://gm0.org/en/latest/docs/hardware-components/kit-and-hardware-guide/rev-robotics.html), [REV 45mm U-Channel](https://docs.revrobotics.com/duo-build/structure/intro/45mm-x-45mm-u-channel).

### REV ION *(imperial system — distinct from DUO)* ✅
| Parameter | Value |
|-----------|-------|
| Hole pitch | **0.5 in (12.7 mm)** |
| Fastener | **#10-32** |
| Hex shaft | 1/2" rounded hex (13.75 mm across corners) |
| Bearing OD | 1.125 in |

> ⚠️ REV ships **two** build systems. DUO (metric, M3) and ION (imperial, #10-32) do **not** share a pattern — don't assume compatibility. Source: [REV ION system standards](https://docs.revrobotics.com/ion-build/home/system-standards).

### AndyMark RoBits ✅ *(cleanest-specified vendor)*
| Parameter | Value |
|-----------|-------|
| Grid spacing | **0.5 in (12.7 mm)** |
| Hole Ø | **0.201 in (5.11 mm)** — #10 clearance |
| Fastener | **#10-32** |
| Gusset resolution | 0.25 in (6.35 mm) |
| Bearing-hole pitch | every 1.5 in (38.1 mm) |
| Tube | 1 × 1 in (25.4 mm) |
| Shaft | 3/8 in hex |

Source: [AndyMark – How Do I Use RoBits](https://andymark.com/pages/how-do-i-use-robits).

### VEX (EDR / V5 metal) 🟡 *(square-hole width 🔴)*
| Parameter | Value |
|-----------|-------|
| Hole pitch | **0.5 in (12.7 mm)** |
| Hole shape | **square** |
| Hole width | 🔴 **unverified** — community ≈ 0.181 in (~4.6 mm); pull STEP |
| Fastener | **#8-32** |
| Channels | 1×2, 1×3, 1×5 (hole-count naming) |

> Excludes VEX IQ (plastic snap system, not FTC metal structure). Sources: [Purdue SIGBots – C-Channels](https://wiki.purduesigbots.com/hardware/misc.-vex-parts-1/structure/c-channels-and-angles), [VEX channel](https://www.vexrobotics.com/channel.html).

---

## 2. Fasteners — M3 / M4 / M5 ✅

*Metric, ISO/DIN. Cross-verified by two independent research passes; the M5 nut thickness (4.7) was corrected during adversarial review.*

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

**Captive-nut pocket** 🟡 (convention): pocket across-flats = nut AF **+ ~0.4**, depth = nut thickness **+ ~0.2**. Size by across-flats but *check against across-corners*. Print a fit gauge per printer.

Sources: [AmesWeb ISO 273](https://amesweb.info/screws/Metric-Clearance-Hole-Chart.aspx), [Fuller Fasteners DIN 912](https://fullerfasteners.com/tech/din-912-specifications-hex-socket-head-cap-screws/), [fasteners.eu ISO 4032](https://www.fasteners.eu/standards/iso/4032/), [Bolt Depot tap drill](https://boltdepot.com/Fastener-Information/Metric-Tap-Drill-Size).

---

## 3. Heat-Set Threaded Inserts ✅ *(CNC Kitchen / Ruthex)*

*Defaults are CNC Kitchen / Ruthex "standard" — the de-facto FTC community insert (both brands dimensionally identical). `hole Ø` is the value you model the receiving boss to.*

| Size | Hole Ø | Insert length | Insert OD | McMaster 94459A (≈) |
|------|--------|---------------|-----------|---------------------|
| M3 | **4.0** | 5.7 | 4.6 | 4.2 |
| M4 | **5.6** | 8.1 | 6.3 | 5.3 |
| M5 | **6.4** | 9.5 | 7.1 | 6.4 |

Min plastic wall around an insert: **~1.6 mm**. Match the hole to the brand you buy; if fit is tight/loose, adjust in 0.1 mm steps and test-print.

Sources: [CNC Kitchen M3](https://cnckitchen.store/products/heat-set-insert-m3-x-5-7-100-pieces), [3DJake Ruthex M4](https://www.3djake.com/ruthex/threaded-inserts-m4-50-pieces), [Ruthex HSS drill set](https://www.ruthex.de/en/products/ruthex-hss-bohrer-set-5-tlg-fur-gewindeeinsatze-m2-m2-5-m3-m4-m5-m6-din-338-bohrer-3-2-4-0-5-6-6-4-8-0-mm-fur-kunststoff-stahl-alu-kupfer-messing-uvm). 🔴 McMaster per-size values need on-site confirmation (JS pages unreadable).

---

## 4. Bearings (bore × OD × width, mm)

| Bearing | Bore | OD | Width | Notes |
|---------|------|-----|-------|-------|
| goBILDA 1611 flanged ✅ | 8 (REX/round) | 14 | 5 | 6 mm-bore variant also exists |
| goBILDA 1600 non-flanged ✅ | 8 | 22 | 7 | = **608** skate bearing |
| REV 49-1559 flanged ✅ | 8 | 12 | 3.5 | REV pairs it w/ 5 mm-hex insert; MR128-class |
| 608 | 8 | 22 | 7 | classic skate bearing |
| 625 | 5 | 16 | 5 | |
| 626 | 6 | 19 | 6 | |
| 688 | 8 | 16 | 5 | 🟡 also exists in 4 mm width |
| MR105 | 5 | 10 | 4 | 🟡 width conflicts across sources |
| MR115 | 5 | 11 | 4 | |
| 6800 | 10 | 19 | 5 | thin section |

> REV uses **5 mm hex** shafts but does not sell a discrete 5 mm-bore metal bearing — it uses the 8 mm bearing + a 5 mm-hex→8 mm-round insert. Sources: [goBILDA 1611](https://www.gobilda.com/1611-series-flanged-ball-bearing-8mm-rex-id-x-14mm-od-5mm-thickness-2-pack/), [REV 49-1559](https://www.revrobotics.com/rev-49-1559-pk10/), [RHD bearing chart](https://rhdbearings.com/bearing-size-chart/).

---

## 5. Material / Sheet Thicknesses (mm)

| Nominal | mm | Use |
|---------|-----|-----|
| 1/16" | 1.588 | light panels |
| 1/8" | 3.175 | **most common** structural/drivetrain plate |
| 3/16" | 4.763 | heavier plates (polycarb often measures ~4.5) |
| 1/4" | 6.35 | heavy drivetrain |
| metric | 3.0, 6.0 | also stocked directly |

COTS: goBILDA plate **2.5 mm** 🟡 · goBILDA 1140 baseplate **6 mm** · REV MAXTube 2×1 wall **3.175 mm**.

**FDM walls** 🟡 (0.4 mm nozzle, general FDM guidance not FTC-specific): min structural **0.8 mm** (2 perimeters); recommended **1.2–1.6 mm** (3–4 perimeters) for load-bearing parts; keep walls integer multiples of nozzle width.

Sources: [Game Manual 0 – Materials](https://gm0.org/en/latest/docs/custom-manufacturing/materials-guide.html), [Robosource FTC polycarb](https://www.robosource.net/specialty-parts-robot-cases-more/ftc-related/polycarbonate-abs-sheets-ftc-frc/clear-polycarbonate-ftc-frc/765-polycarb-0177-3-16-thick).

---

## 6. Project Conventions 🔵

| Convention | Value |
|------------|-------|
| Standard fillet set | 1, 2, 3 mm (pick smallest that carries the load) |
| FDM hole clearance | +0.2 mm over nominal for printed holes |

---

## Open verification tasks

Before treating these as final source-of-truth, pull the official STEP/drawings for:

1. **goBILDA** true drilled hole Ø (label "4 mm" vs. real M4 clearance) + any goRAIL-specific pattern.
2. **REV** exact hole Ø (M3 clearance) + the Extended Motion Pattern pitch.
3. **VEX** square-hole width (community ~0.181 in, unverified — KB was access-blocked).
