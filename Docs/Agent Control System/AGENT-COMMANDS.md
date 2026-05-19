---
color: "#f40a0a"
---
# Agent Command Protocol

## Command Format
[AGENT] :: [GOAL] :: [CONSTRAINTS]

### Example
Agent B :: Implement fixed-point zoom :: Follow 0209 §4.3 exactly

---

## Allowed Commands
- IMPLEMENT
- DESIGN
- REVIEW
- VALIDATE
- DEMO
- STOP

---

## Examples

Agent A :: VALIDATE :: Coordinate invariants after unit change  
Agent B :: IMPLEMENT :: Redraw/Refresh pipeline per 0209 §2  
Agent C :: DESIGN :: HitTest algorithms for line, rect, ellipse  
Agent D :: IMPLEMENT :: Material aggregation logic  
Agent E :: DEMO :: Full draw → save → reopen → undo

---

## Completion Criteria
A command is complete only if:
- code compiles OR
- demo succeeds OR
- decision is recorded