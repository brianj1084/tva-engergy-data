# Copilot Instructions

## Project Overview

ASP.NET Core MVC web application (.NET 8) for TVA energy data. Root namespace is `tva_engery_data` (note: project name has a typo — "engery" not "energy"; preserve this in namespaces and file references).

## Build & Run

```bash
dotnet build
dotnet run
dotnet watch run   # hot reload during development
```

## Architecture

Standard ASP.NET Core MVC layout:

- **Controllers/** — Action methods returning `IActionResult`; use constructor-injected services
- **Services/EiaApiService** — Typed `HttpClient` wrapper for the EIA v2 API; registered via `AddHttpClient<EiaApiService>()`. All EIA calls go through this service.
- **Models/EiaModels.cs** — EIA API response DTOs; fields use `[JsonPropertyName]` because EIA returns hyphenated JSON keys (e.g. `"type-name"`, `"nameplate-capacity-mw"`)
- **Models/TvaGenerationViewModel.cs** — View models built in `GenerationController`
- **Views/** — Razor (`.cshtml`) views; shared layout in `Views/Shared/_Layout.cshtml`
- **wwwroot/** — Static assets; Bootstrap and jQuery are bundled under `wwwroot/lib/` (no npm/Node build step)

Default route points to `Generation/Index` (the main dashboard): `{controller=Generation}/{action=Index}/{id?}`.

## EIA API

- Base: `https://api.eia.gov/v2/`
- Key configured at `EiaApi:ApiKey` in `appsettings.json` (use `dotnet user-secrets` in production)
- **Hourly generation by fuel type** (most real-time, ~1 hr lag): `electricity/rto/fuel-type-data/data/` — filter `facets[respondent][]=TVA`; values are MWh/hour ≡ average MW
- **Generator unit inventory** (monthly, EIA-860M): `electricity/operating-generator-capacity/data/` — filter `facets[balancing_authority_code][]=TVA`; returns nameplate & net-summer capacity per unit

## Key Conventions

- Nullable reference types are enabled (`<Nullable>enable</Nullable>`); always handle nullability explicitly
- Implicit usings are enabled; avoid redundant `using` statements for common namespaces
- Use `asp-controller` / `asp-action` tag helpers for internal links instead of hardcoded URLs
- CSS scoping per-view is supported via `Views/Shared/_Layout.cshtml` referencing `tva_engery_data.styles.css` (Razor CSS isolation)
- `appsettings.Development.json` overrides `appsettings.json` in local dev; secrets should use `dotnet user-secrets`, not config files
