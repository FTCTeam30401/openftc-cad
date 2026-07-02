# Interface: VEX (EDR / V5 metal)

**System:** imperial · **Fastener:** #8-32 · **Grid:** 0.5 in · Confidence: 🟡 *(square-hole width 🔴)*

> Covers VEX EDR / V5 **metal C-channel structure**. **Excludes VEX IQ** (a plastic snap-pin system, not used for FTC-grade structure).

## Pattern

| Feature | Value |
|---------|-------|
| Hole pitch | 0.5 in (12.7 mm) |
| Hole shape | **square** (distinctive to VEX) |
| Hole width | 🔴 community ≈ 0.181 in (~4.6 mm) — **unverified**, KB access-blocked |
| Fastener | #8-32 |
| Channels | 1×2, 1×3, 1×5 (named by hole count) |

## When designing a VEX-mating part
- The 0.5 in pitch matches RoBits/REV ION, so **VEX↔RoBits adapters are mostly about hole shape** (square vs round) rather than spacing.
- Square holes: if a part only needs to *bolt through* VEX, a round #8 clearance hole (≈4.4 mm) on the 0.5 in grid works — you only need the square profile if you're replicating a VEX rail.

🔴 **Verify before final:** exact square-hole width from a VEX STEP file or a measured part before committing it to the library.
