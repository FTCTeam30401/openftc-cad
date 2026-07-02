# Standards Library

The reusable building blocks every OpenFTC CAD model depends on. **Phase 1 lives here.**

## Contents

| File | What it is |
|------|-----------|
| [master-variables.md](master-variables.md) | The single source of truth for every dimension (human-readable). |
| [`../../standards/master-variables.yaml`](../../standards/master-variables.yaml) | Same data, machine-readable — feeds Onshape variable tables & future FeatureScripts. |
| [hole-patterns.md](hole-patterns.md) | The parametric hole-pattern definition + the first concrete pattern. |
| [interfaces/](interfaces/) | Per-ecosystem interface definitions (goBILDA, REV, RoBits, VEX). |

## How to use this in Onshape

1. In a new part studio, create a **Variable Table** (or import the shared "OpenFTC Standards" tab) and populate it from `master-variables.yaml`.
2. Reference variables by name in sketches — never type a raw dimension that exists here.
3. When mating to a vendor, open that vendor's [interface definition](interfaces/) and use its named pattern.

## Why a standards library exists

A generator can only place "a goBILDA hole pattern" if something *defines* what that pattern is. These files are that definition. Get them right once, and every downstream plate, bracket, and adapter inherits correct geometry for free — which is the whole point of the [guiding principle](../philosophy.md).
