# SkillsTracker — Product Requirements Document

**Last updated:** 2026-04-27  
**Status:** Draft — epics defined, user stories TBD

---

## Overview

Desktop application (Avalonia, .NET 10) for tracking and comparing skill levels across structured topics. Users record their current proficiency; admins view and analyse all user data. The goal is not mastery of everything — it's visibility into strengths and areas of active development, enabling meaningful comparison across users.

---

## Non-Goals

- No concept of "overall progress" or completion percentage
- No gamification, badges, or mastery targets
- Web frontend (desktop-only client for now)
- Microsoft auth is a stretch goal — not required for MVP

---

## Domain Notes

- `TopicSkillLevel` model is **redundant** — to be removed
- `SkillLevelRequirement` (new) replaces it: describes what a given level means for a specific skill
- Level on `UserSkillProgress.LevelId` = **current achieved level** (not target)
- Topic "progress" indicator = **average `Level.SortOrder`** across skills in that topic for a user

---

## Epics

### E1 · Dev Tooling
Unblocks all other epics. Enables development without real auth.

- Stub auth: inject fake user identity (id, role) via config/env
- Admin / non-admin toggle accessible in dev (UI or config flag)
- Seed data extended to include realistic `UserSkillProgress` records
  - Each seeded user has progress on ~40% of skills (random subset, random levels)
  - Stub user (`StubUserId`) has broader coverage (~80–100%) for easy dev/testing

---

### E2 · User Skill Progress
Core data capture. Users record their current achieved level per skill.

- API endpoints (all scoped to authenticated user; non-admin can only access own data):
  - `GET    /api/users/{userId}/skills` — all progress for a user
  - `GET    /api/users/{userId}/skills/{skillId}` — single skill progress
  - `POST   /api/users/{userId}/skills/{skillId}` — create progress record (level required)
  - `PUT    /api/users/{userId}/skills/{skillId}` — update level
  - `DELETE /api/users/{userId}/skills/{skillId}` — remove progress record
- `LevelId` required on POST — no unrated state; a skill only appears in progress once a level is set
- Desktop: user can view and set their achieved level for each skill
- Skills always shown within a topic context
- Level selection via segmented control; changes are local state until user explicitly saves
- UI form invalid (save disabled) until a level is selected
- Explicit save required before persisting — prevents accidental level changes polluting history (E9)
- Navigating away with unsaved changes shows a discard warning dialog

---

### E3 · Topic Dashboard
Primary user-facing view.

- Dashboard lists all topics
- Topic card shows average `Level.SortOrder` across skills the user has rated in that topic
- Drill into topic → list of skills with current level visualised (exact format TBD)
- User can edit their level inline

---

### E4 · Active Skill Plans
User flags skills they are actively working towards.

- `bool IsActive` on `UserSkillProgress` (default `false`) — no separate table
- Dedicated **My Plan** view: lists all active skills across all topics

---

### E5 · Anonymised Peer Data
Social context for self-assessment.

- Opt-in toggle on topic/skill view: show anonymised level distribution of all users
- API aggregation endpoint — no individual identity exposed
- Shown alongside user's own level for comparison

---

### E6 · Skill Level Requirements
Admin-authored descriptions of what each level means for a given skill.

- New table: `SkillLevelRequirement` (`SkillId`, `LevelId`, `Description`)
- Admin UI: create / edit requirement text per skill + level combination
- User UI: view requirements when inspecting a skill level

---

### E7 · Admin Dashboard
Read-only oversight view, admin-only.

- List all users; filter by topic / skill / current level
- Graphical summary of level distribution across users *(chart types TBD)*
- No editing of user data from this view

---

### E8 · Microsoft Authentication *(stretch goal)*

- Login via Microsoft MSAL
- User identity resolved from token email → `User` record
- `IsAdmin` flag on `User` model; managed by admin or config
- All views gated behind authentication
- Tenant scope: TBD

---

### E9 · Skill Progress History *(stretch goal)*

- Append-only log of level changes per user per skill
- New table: `UserSkillProgressHistory` (`UserId`, `SkillId`, `LevelId`, `AchievedAt`)
- `UserSkillProgress` remains the current-state record (unique per user+skill)
- UI: timeline or history view per skill showing progression over time

---

## Open Questions

| # | Epic | Question |
|---|------|----------|
| 1 | E3 | Exact skill-level visualisation format (bar, dots, label)? |
| 2 | E7 | Which chart types for admin graphical data? |
| 3 | E8 | Single-tenant (company AAD) or multi-tenant? |
