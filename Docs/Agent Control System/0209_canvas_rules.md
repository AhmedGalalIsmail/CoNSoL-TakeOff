---
color: "#c11313"
---
## ✅ A-01 — Coordinate Invariants

- Logical data must never be modified for rendering
- Pan must translate LogicalOrigin, not Geometry
- Zoom must modify ScaleFactor only
- Rendering must apply transformation at draw time

❌ Forbidden:
- Modifying Shape coordinates during pan/zoom

