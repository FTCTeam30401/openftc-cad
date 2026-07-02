# Adapters — the signature contribution

Cross-vendor adapters let teams combine ecosystems instead of committing to one. **Instead of redesigning a robot, a team prints an adapter.**

**Planned (Phase 3):**
- REV → goBILDA
- REV → RoBits
- RoBits → VEX
- goBILDA → VEX
- Servo adapters
- Bearing adapters
- Camera adapters

Every adapter references two interface definitions (the "from" standard and the "to" standard) from [`docs/standards/interfaces/`](../../docs/standards/interfaces/). An adapter is, by definition, a bridge between two standards — so it must cite both.
