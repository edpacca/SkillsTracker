# Decisions: E1 · Dev Tooling / E8 · Auth — Identity & Roles

## Identity flow (E1 → E8)

**Desktop → API: `X-User-Id` header per request.**
- Dev: Desktop reads `StubUserId` + `StubIsAdmin` from `appsettings.Development.json`, sends as headers
- Prod (E8): replaced by MSAL token; same middleware interface, different source

**API middleware: `CurrentUserMiddleware`**
- Reads headers, populates scoped `ICurrentUserContext` in DI
- Controllers inject `ICurrentUserContext` — never touch headers directly
- Swap dev→prod: implement a second middleware behind the same interface; controllers unchanged

---

## Roles model (E8, informed by E1)

**Separate `UserRole` join table with enum.**

```
UserRole
  UserId  (FK → User)
  Role    (enum: User | Admin)
```

- No `IsAdmin` field on `User`
- Dev stub: `StubIsAdmin` in config populates `ICurrentUserContext.IsAdmin` directly (no DB lookup)
- Prod: resolve from `UserRole` table after identity confirmed via token
