# Design Decision Records

---

## DDR-003 — Desktop uses services in-process; API is a separate deployable

**Date:** 2026-04-28  
**Status:** Decided  
**Epics:** E1, all

### Decision
Desktop references `Services` + `Data` directly (no HTTP). The API project hosts the same services behind HTTP controllers as a separate deployable for future web clients.

### Rationale
- Single runtime for desktop — no HTTP overhead, no second process in dev
- Web-ready: a future web client hits the API project; desktop never changes
- `Core.Abstractions` interfaces allow swapping Desktop service registrations for `HttpClient`-backed impls if desktop ever needs to go remote

### Constraint
API controllers must be kept in sync with service changes. Whenever a service is modified or added, the corresponding controller and its tests must be updated. The API is never treated as a secondary concern.

---

## DDR-002 — Skill progress history deferred to stretch goal

**Date:** 2026-04-28  
**Status:** Decided  
**Epics:** E2, E9

### Decision
No history tracking in E2. `UserSkillProgress` is a single mutable current-state row per (UserId, SkillId). History is deferred to E9 (stretch goal).

### Options considered
- **Current-state only (E2)** ✓ — one row per user+skill, updated in place
- Append-only log — every change is a new row; current = latest by `AchievedAt`
- History table (E9) — separate `UserSkillProgressHistory` table; `UserSkillProgress` stays as current state

### Rationale
No UI requirement for history display in current scope. Adding history now adds schema and query complexity with no user-facing benefit. When E9 is implemented, preferred approach is Option A (history table) — keeps current-state queries fast and simple.

---

## DDR-001 — Level scope: global (not per-topic or per-skill)

**Date:** 2026-04-28  
**Status:** Decided  
**Epics:** E2, E3, E6

### Decision
`Level` is a global entity shared across all topics and skills. All skills use the same level taxonomy.

### Options considered
| Option | Description |
|--------|-------------|
| Raw value | `int` on `UserSkillProgress` — no entity |
| **Global entity** ✓ | Shared `Level` rows; all skills use same scale |
| Per-topic entity | `Level` gets `TopicId` FK; each topic owns its own levels |

### Rationale
- Per-skill nuance is handled by `SkillLevelRequirement(SkillId, LevelId, Description)` — the label is shared, the definition is per-skill
- Topic progress (avg `Level.SortOrder`) is only meaningful if all skills share the same scale
- Per-topic levels add admin burden and referential integrity complexity with no clear benefit given current requirements

### Known constraint
All skills must share the same level scale. A skill requiring a fundamentally different taxonomy (e.g. binary pass/fail) cannot be expressed cleanly. Acceptable for current scope — revisit if requirements change.
