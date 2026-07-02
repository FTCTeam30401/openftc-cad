# Creating a Part with OpenFTC CAD

This is the whole point of the project: **you don't model parts — you describe
them.** Every part comes out of a generator dialog in Onshape.

*Time for your first part: about 2 minutes.*

---

## 1. Open the document

Open the **OpenFTC CAD** Onshape document. Everything you need is already
inside it:

- **OpenFTC Features** tab — the generators (you never need to open this)
- **Standards** tab — the master variables (you never need to open this either)
- Part Studio tabs — where parts get made

## 2. Make a home for your part

Click the **`+`** at the bottom-left corner of the window → **Create Part
Studio**. Right-click its tab → **Rename** to whatever you're building
(`battery_mount_v1`, `drive_gusset`, …).

> You can also work in an existing Part Studio, but one part per studio keeps
> exports clean.

## 3. Open the generator menu

In the top toolbar, at the **far right end**, there's a small icon with a
dropdown arrow — hover text says **"Custom features in this workspace."**
Click it. You'll see the OpenFTC toolkit:

| Pick this… | …to get |
|------------|---------|
| **OpenFTC Plate** | a flat pattern plate (most common starting point) |
| **OpenFTC Adapter Plate** | two vendors' patterns bridged on one plate |
| **OpenFTC L / U / T Gusset** | corner, channel, or tee brackets |
| **OpenFTC Hole Pattern** | just holes, cut into anything you already have |
| **OpenFTC Heat-Set Boss** | threaded-insert bosses on an existing part |
| **OpenFTC Bearing Pocket** | press-fit bearing seats on an existing part |

## 4. Fill in the dialog

Every generator asks roughly the same things:

1. **Plane or planar face** — click the **Top** plane in the feature list on
   the left (or any face of an existing part).
2. **Standard** — which ecosystem the part mates to:
   goBILDA · RoBits/REV ION · VEX · REV DUO. All the spacing and hole sizes
   come from this one choice.
3. **Columns / Rows** — the size of the part, counted in holes. A goBILDA
   9×7 plate is 72 × 56 mm; sizes are always `count × grid spacing`.
4. **Hole size** — the important one:
   - **"Bolt onto vendor structure (clearance)"** — your part bolts *onto*
     aluminum. Screws must pass through freely. *This is the default and the
     right answer 90% of the time.*
   - **"Replicate vendor member (native size)"** — your part *replaces* a
     piece of aluminum, so screws thread into it (goBILDA's real holes are
     4.0 mm thread-forming, and VEX gets true square holes).
5. Feature-specific extras — thickness, corner fillet, goBILDA bearing
   holes, etc. Defaults are sensible.

The part **previews live** as you type. When it looks right, click the green
**✓**. Done — that's a part.

## 5. Change your mind anytime

Double-click the feature in the list on the left → the same dialog reopens →
change any number → ✓. The part regenerates. A 6×4 plate becomes a 12×4 plate
in one edit. **Never redraw anything.**

## Adding hardware to a part (bosses & bearing seats)

The Heat-Set Boss and Bearing Pocket features place hardware **at points you
choose**:

1. **Sketch the locations:** toolbar → **Sketch** → click the part's face →
   use the **Point** tool (small dot icon) → click wherever hardware goes →
   green ✓ to close the sketch.
2. Run **OpenFTC Heat-Set Boss** (or Bearing Pocket) from the custom features
   menu: pick the **face**, then click your **sketch points**, choose the
   insert size / bearing, ✓.

> If you can't click the points, the sketch is hidden — hover over it in the
> feature list and click the eye icon.

Boss sizing is pre-loaded with CNC Kitchen / Ruthex insert dimensions —
pick M3/M4/M5 and the pilot hole is automatically correct.

## Exporting for print

1. Right-click the part under **Parts** (bottom-left panel) → **Export…**
2. Format: **3MF** (best) or STL. Resolution: Fine.
3. Slice with print-first settings from the standards library:
   - **4 perimeters** (≈1.6 mm walls) minimum for anything load-bearing
   - Holes print slightly tight — the generators already use proper
     clearances, so don't scale the model; adjust flow/horizontal expansion
     if your printer runs tight
4. Print a small test coupon first (a 2×2 plate takes minutes) and check a
   real screw + bearing fit before committing to the big part.

## The release ladder

A part you just generated is a **Prototype**. It becomes **Beta** when it's
printed and fit-checked, **Competition Tested** when it survives a match, and
**Verified** when it's proven across time. Nothing is "finished" until a
robot says so.

---

## FAQ

**Which "Standard" do I pick if my robot is goBILDA?** goBILDA. If you're
bolting a printed part onto goBILDA channel, keep hole size on
*clearance*.

**How do I bolt goBILDA to RoBits/REV ION/VEX?** That's the **Adapter
Plate** — zone A one standard, zone B the other, print the bridge.

**Can I combine generators?** Yes — they stack in one Part Studio. Common
combo: Plate → sketch points → Heat-Set Boss on the same plate.

**Something regenerated red?** Double-click the feature and read the error —
usually an unselected field (the dialogs tell you exactly what's missing).
