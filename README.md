# RaceDay – Part 2 API

ASP.NET Core 8 Web API using Entity Framework Core, SQL Server LocalDB, cookie authentication, role checks, Swagger and xUnit tests. This is a learning project scaffold; review it against your lecturer's rubric and approved Part 1 ERD before submission.

## Open in Visual Studio 2022
1. Extract the ZIP to a folder you can edit.
2. Open `RaceDay.sln`.
3. Restore NuGet packages and ensure the .NET 8 SDK and SQL Server LocalDB are installed.
4. In Package Manager Console, set Default project to `RaceDay.API`, then run `Add-Migration InitialCreate` and `Update-Database`. If your database already has a migration, do not create a duplicate; inspect the existing migrations first.
5. Set `RaceDay.API` as startup project and run with Ctrl+F5. Swagger opens at `/swagger`.
6. Run tests using Test > Run All Tests, or `dotnet test RaceDay.sln`.

## Connection string
Default is LocalDB: `(localdb)\MSSQLLocalDB`, database `RaceDayDB`. Change `RaceDay.API/appsettings.json` if your SQL Server instance differs.

## Main endpoints
- `POST /api/auth/register` — register with role `Organiser` or `Participant`.
- `POST /api/auth/login` — login; returns an HTTP-only cookie.
- `POST /api/auth/logout`, `GET /api/auth/me`, `PUT /api/profile`.
- `GET /api/events` and `GET /api/events/{id}` — public event list/details.
- Organiser: `POST/PUT/DELETE /api/events`, `GET /api/events/{id}/enrolments`, manage categories under `/api/events/{eventId}/categories`, and record results at `POST /api/results/enrolments/{enrolmentId}`.
- Participant: `POST /api/enrolments/events/{eventId}`, `GET /api/enrolments/mine`, `GET /api/results/mine`.

## Authentication note
Registration hashes passwords using ASP.NET Core's `PasswordHasher<User>`. Authentication uses an HTTP-only cookie and role claims. For Swagger's cookie-auth testing, log in first using the same browser/session. The role is selected during registration for this educational assignment; for a production system, restrict Organiser account creation to administrators.

## Unit tests and CI
`RaceDay.Tests` includes basic xUnit validation tests. GitHub Actions restores, builds and runs tests on pushes and pull requests to `main`/`master`. Add a screenshot of a green Actions run here before final submission.

## Demo video
Add your unlisted YouTube demonstration link here after recording.
