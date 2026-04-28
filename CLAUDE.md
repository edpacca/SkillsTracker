# SkillsTracker — Claude Instructions

## What this is
.NET 10 Avalonia desktop app + ASP.NET Core Web API for tracking user skill proficiency across topics. Users rate themselves per skill; admins view and analyse all user data.

Desktop runs services in-process (no HTTP). The API project is a separate deployable for future web clients — it shares the same service and data layers. See `docs/DDR.md` for architectural decisions.

---

## Project layout

| Project | Purpose |
|---|---|
| `src/SkillsTracker.Core/` | Models, enums, DTOs, `I*Service` and `IRepository` interfaces |
| `src/SkillsTracker.Data/` | DbContext, EF config, repository impls, migrations, seeder |
| `src/SkillsTracker.Services/` | Concrete `I*Service` implementations |
| `src/SkillsTracker.Api/` | REST controllers, `ExceptionHandlingMiddleware`, web host |
| `src/SkillsTracker.Desktop/` | Avalonia UI — CommunityToolkit.Mvvm |
| `SkillsTracker.tests/` | xUnit + Moq unit tests |

---

## Domain model

| Model | Key fields | Notes |
|---|---|---|
| `User` | `Id`, `Username`, `Email`, `CreatedAt` | Has many `UserSkillProgress` |
| `Skill` | `Id`, `Name`, `Description` | Has many `TopicSkill`, `UserSkillProgress` |
| `Topic` | `Id`, `Name`, `Description` | Has many `TopicSkill` |
| `TopicSkill` | `TopicId`, `SkillId` | Join: Topic ↔ Skill |
| `Level` | `Id`, `Name`, `SortOrder` | Global — shared across all topics and skills (see DDR-001) |
| `UserSkillProgress` | `Id`, `UserId`, `SkillId`, `LevelId`, `IsActive`, `CreatedAt`, `UpdatedAt` | One row per user+skill (unique index); `LevelId` required — no unrated state |

**Removed:** `UserSkill`, `SkillStatus`, `TopicSkillLevel` — all defunct, do not reintroduce.

**Planned:** `SkillLevelRequirement(SkillId, LevelId, Description)` — per-skill meaning for each level (E6). `UserRole(UserId, Role)` — roles join table (E8).

Topic progress = average `Level.SortOrder` across a user's rated skills in that topic.

---

## Architecture conventions

**Exception handling**
Services propagate exceptions; `ExceptionHandlingMiddleware` maps them to `ProblemDetails`. Never add `try/catch` in services or controllers unless handling specific business logic (e.g. `DbUpdateConcurrencyException` → `ExistsAsync` check).

| Exception | HTTP status |
|---|---|
| `ArgumentException` | 400 |
| `KeyNotFoundException` | 404 |
| `DbUpdateConcurrencyException` / `DbUpdateException` | 409 |
| anything else | 500 |

**Repositories**
- `IRepository<T>` — base interface (CRUD + exists)
- `IPagedRepository<T> : IRepository<T>` — adds paging; only `UserRepository` implements this
- All reads use `.AsNoTracking()`
- All methods async throughout

**Code style**
- Primary constructor syntax preferred: `public UserService(IRepository<Skill> repository)`
- Services depend on interfaces from `Core.Abstractions` only — never concrete types

**API ↔ Desktop sync**
Desktop uses services in-process; the API exposes the same services over HTTP. Whenever a service changes, update the corresponding controller and its tests. The API must never silently lag behind the service layer.

**Identity (E1 / E8)**
- Dev: `X-User-Id` and `X-Is-Admin` headers read from `appsettings.Development.json` (`StubUserId`, `StubIsAdmin`)
- `CurrentUserMiddleware` populates scoped `ICurrentUserContext` — controllers inject this, never read headers directly
- E8 (MSAL): swaps in a new middleware behind the same interface; controllers unchanged

---

## Testing

- xUnit + Moq
- Service tests mock `IRepository<T>` / `IPagedRepository<T>` via `ServiceMockRepository.cs`
- Middleware tests use `Microsoft.AspNetCore.TestHost` with a minimal `HostBuilder`
- Do not test exception wrapping in service tests — that belongs in `ExceptionHandlingMiddlewareTests`

---

## Database

- PostgreSQL via Npgsql EF Core
- Connection string key: `PostgresConnection`
- Seeded with Bogus via `DatabaseSeeder.cs` — 50 users, ~40% skill coverage per user; stub user has ~80–100% coverage
- Migrations: `src/SkillsTracker.Data/Migrations/`
