# SkillsTracker — Claude Instructions

## Project Overview
A .NET 10 Avalonia desktop + ASP.NET Core Web API application for tracking user skills within structured topics and levels. Users can track skill progress; admins can view and rank all users.

## Architecture
- **Desktop**: Avalonia UI with CommunityToolkit.Mvvm — `src/SkillsTracker.Desktop/`
- **API**: ASP.NET Core controllers — `src/SkillsTracker.Api/Controllers/`
- **Services**: Business logic — `src/SkillsTracker.Services/` — always depend on interfaces from `Core.Abstractions`
- **Repositories**: `src/SkillsTracker.Data/Repository/` — generic `IRepository<T>` and `IPagedRepository<T>` (User only)
- **DbContext**: PostgreSQL via Npgsql — `src/SkillsTracker.Data/ApplicationDbContext.cs`
- **Middleware**: `src/SkillsTracker.Api/Middleware/ExceptionHandlingMiddleware.cs` handles all exception → ProblemDetails mapping; services and controllers do NOT catch exceptions

## Key Conventions
- **Exception handling**: Services propagate exceptions; middleware maps them to HTTP responses. Do not add try/catch in services or controllers unless there is specific business logic (e.g. `DbUpdateConcurrencyException` → `ExistsAsync` check in `UpdateAsync`)
- **Exception → HTTP mapping**: `ArgumentException` → 400, `KeyNotFoundException` → 404, `DbUpdateConcurrencyException` / `DbUpdateException` → 409, everything else → 500
- **Primary constructor syntax**: Preferred for services and controllers (e.g. `public UserService(IPagedRepository<User> repository)`)
- **Async throughout**: All repo and service methods are async
- **No tracking on reads**: Repositories use `.AsNoTracking()` for all read operations
- **Paging**: Only `UserRepository` implements `IPagedRepository<T>`; other repos use plain `IRepository<T>`

## Domain Models
| Model | Notes |
|---|---|
| `User` | Has many `UserSkill` |
| `Skill` | Has many `UserSkill`, `TopicSkillLevel` |
| `Topic` | Has many `TopicSkillLevel`, `Level` |
| `Level` | Belongs to `Topic` |
| `UserSkill` | Join: User ↔ Skill, with `SkillStatus` enum |
| `TopicSkillLevel` | Join: Topic ↔ Skill ↔ Level |

## Projects
| Project | Purpose |
|---|---|
| `src/SkillsTracker.Core/` | Models, enums, DTOs, service & repository interfaces |
| `src/SkillsTracker.Data/` | DbContext, EF config, repository impls, migrations, seeder |
| `src/SkillsTracker.Services/` | Concrete I*Service implementations |
| `src/SkillsTracker.Api/` | REST controllers, exception middleware, web host |
| `src/SkillsTracker.Desktop/` | Avalonia UI (CommunityToolkit.Mvvm) |
| `SkillsTracker.tests/` | xUnit unit tests |

## Testing
- Framework: xUnit + Moq
- Service tests mock `IRepository<T>` / `IPagedRepository<T>` via `ServiceMockRepository.cs` fixture
- Middleware tests use `Microsoft.AspNetCore.TestHost` with a minimal `HostBuilder`
- Run tests: `dotnet test`
- **Do not** test exception wrapping in service tests — that belongs in `ExceptionHandlingMiddlewareTests`

## Database
- PostgreSQL via Npgsql
- Connection string key: `PostgresConnection`
- Seeded with Bogus (50 users) via `DatabaseSeeder.cs`
- Migrations in `src/SkillsTracker.Data/Migrations/`
