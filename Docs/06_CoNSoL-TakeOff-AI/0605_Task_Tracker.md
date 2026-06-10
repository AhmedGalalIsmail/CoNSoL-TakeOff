---
color: "#5e0101"
---
# CoNSoL-TakeOff AI Execution Matrix - Task Tracker

## 📊 Overview

| ID      | Category | Task                  | Status | Depends On | Notes             |
| ------- | -------- | --------------------- | ------ | ---------- | ----------------- |
| AI-001  | AI       | OCR Text Extraction   | 🔲     | —          | Tesseract         |
| AI-002  | AI       | Scale Detection       | 🔲     | AI-001     | Pattern-based     |
| AI-003  | AI       | Geometry Detection    | 🔲     | AI-002     | OpenCV            |
| AI-004  | AI       | Classification Engine | 🔲     | AI-003     | Rule-based        |
| AI-005  | AI       | YOLO Integration      | 🔲     | AI-003     | Future upgrade    |
| UI-001  | UI       | Canvas Rendering      | 🔲     | —          | Stable            |
| UI-002  | UI       | Layer Panel           | 🔲     | AI-004     | Needs binding     |
| UI-003  | UI       | Properties Panel      | 🔲     | UI-001     | Works             |
| UI-004  | UI       | Selection UX          | 🔲     | UI-001     | Improve lines     |
| BUS-001 | Business | TakeOff Calculator    | 🔲     | —          | Working           |
| BUS-002 | Business | Material Mapping      | 🔲     | AI-004     | Expand logic      |
| EXP-001 | Export   | Excel Export          | 🔲     | BUS-001    | Done              |
| EXP-002 | Export   | PDF Export            | 🔲     | EXP-001    | Pending           |
| SYS-001 | System   | Logging               | 🔲     | —          | Stable            |
| SYS-002 | System   | Config                | 🔲     | —          | Add default scale |
| SYS-003 | System   | Caching               | 🔲     | AI-001     | Must add          |
