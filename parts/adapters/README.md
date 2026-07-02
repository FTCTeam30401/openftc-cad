# Adapters — the signature contribution

Cross-vendor adapters let teams combine ecosystems instead of committing to one. **Instead of redesigning a robot, a team prints an adapter.**

## The three hardware families

Adapters make the most sense once you see that the four ecosystems collapse into **three families** (verified — see the [hardware-families table](../../docs/standards/master-variables.md#1a-cross-vendor-hardware-families)):

| Family | Members | Grid · Fastener · Hole |
|--------|---------|------------------------|
| Imperial 0.5″ / #10-32 | **RoBits + REV ION** | 0.5 in · #10-32 · round ~0.20″ |
| Imperial 0.5″ / #8-32 square | **VEX** | 0.5 in · #8-32 · 0.182″ square |
| Metric 8 mm grid | **goBILDA (M4) + REV DUO (M3)** | 8 mm · M4/M3 · round |

This reframes the adapter roadmap:
- **Within the imperial 0.5″ world**, RoBits ↔ REV ION are nearly 1:1 — the easy wins.
- **VEX** shares the 0.5″ pitch but needs its own adapter (different thread *and* square holes).
- **goBILDA ↔ REV DUO** share the 8 mm metric grid but differ in fastener (M4 vs M3).
- **Metric ↔ imperial** is the hardest jump — and the highest-value adapter to get right.

## Planned (Phase 3)
- goBILDA ↔ REV DUO (metric bridge, M4↔M3)
- Metric 8 mm ↔ Imperial 0.5″ (goBILDA/REV-DUO ↔ RoBits/VEX/REV-ION)
- REV ION ↔ RoBits (near-trivial, imperial 1:1)
- VEX ↔ RoBits (0.5″ pitch, but #8-32/square ↔ #10-32/round)
- Servo adapters · Bearing adapters · Camera adapters

Every adapter references two interface definitions (the "from" and "to" standards) from [`docs/standards/interfaces/`](../../docs/standards/interfaces/). An adapter is a bridge between two standards — so it must cite both.
