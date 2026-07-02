# Interface: VEX (EDR / V5 metal)

**System:** imperial · **Fastener:** #8-32 · **Grid:** 0.5 in · Confidence: ✅

> Covers VEX EDR / V5 **metal C-channel structure**. **Excludes VEX IQ** (a plastic snap-pin system, not FTC-grade structure).

## Pattern

| Feature | Value |
|---------|-------|
| Hole pitch | 0.5 in (12.7 mm) |
| Hole shape | **square** |
| **Hole width** | **0.182 in (4.62 mm)** — ✅ verified from VEX drawing PN 276-2600 |
| Fastener | #8-32 |
| Channel naming | AxBxCxL (flange × web × flange in half-inch hole counts, × length) |

## When designing a VEX-mating part
- The 0.5 in pitch matches RoBits/REV ION, so **VEX adapters are mostly about hole shape + thread**: VEX is #8-32 with 0.182″ square holes, while RoBits/ION are #10-32 with round ~0.20″ holes.
- If a printed part only needs to *bolt through* VEX, a round #8 clearance hole (≈ 4.4 mm) on the 0.5 in grid works — you only need the square profile if replicating a VEX rail.

The square-hole width was previously unverified; it's now confirmed from a primary VEX engineering drawing.
