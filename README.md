# E-Commerce API (ASP.NET Core)

A modular ASP.NET Core Web API for a small e-commerce platform. The solution contains separate projects for the API, data access (Entity Framework Core), business logic, and shared/common types. It includes authentication using ASP.NET Identity and JWT, OpenAPI support in development, file uploads/static file serving, and data seeding for initial roles and an admin user.

## Projects in the solution
- `E-Commerce` — ASP.NET Core Web API (hosts controllers, app configuration and `Program.cs`).
- `E-Commerce.Data` — EF Core `DbContext`, entity models, repository implementations and migrations.
- `E-Commerce.Logic` — Business logic layer, DTOs, validators, managers, and service registrations.
- `E-Commerce.Common` — Shared types (results, pagination, filters, etc.).

## Key features
- ASP.NET Core Web API targeting .NET 10
- EF Core database migrations and repositories
- ASP.NET Core Identity with roles and seeded administrator
- JWT authentication
- OpenAPI (Swagger) exposed in Development environment
- Static file hosting for uploaded files under the `/Files` request path
- Basic seeding for categories, products, roles and an admin user

## Prerequisites
- .NET 10 SDK (install from https://dotnet.microsoft.com)
- SQL Server instance (LocalDB or SQL Server / SQL Express). The development configuration targets a local `SQLEXPRESS` instance by default.
- (Optional) An IDE such as Visual Studio 2022/2023, Visual Studio Code, or Rider.

## Configuration
1. Open `E-Commerce/appsettings.Development.json` (or `appsettings.json`) and update the `ConnectionStrings:RESTfulConnectionString` value to point to your SQL Server instance. Example for LocalDB:

   "ConnectionStrings": {
     "RESTfulConnectionString": "Server=(localdb)\\mssqllocaldb;Database=ECommerce;Trusted_Connection=True;TrustServerCertificate=True"
   }

2. JWT settings are in `appsettings.Development.json` under `JwtSettings`. You may replace the `SecretKey` with your own 256-bit base64-encoded key for production.

3. By default the repository seeds an admin role and an admin user during startup. The seeded admin credentials are:
   - Email: `admin@gmail.com`
   - Password: `P@ssw0rd`

   (These values can be changed by editing `E-Commerce/Helper/AspUsersDataSeeder.cs`.)

## Build and run
From the repository root run:

1. Restore and build

   dotnet restore
   dotnet build

2. Run the API (choose the `E-Commerce` project directory or open the solution in an IDE)

   dotnet run --project E-Commerce

On first run the application will apply EF Core migrations automatically (see `Program.cs`) and run configured seeders.

## API usage
- OpenAPI/Swagger is available when the environment is `Development` (see `Program.cs` where `MapOpenApi()` is called). Use it to explore endpoints and test requests.
- Authentication: Use the `AuthController` endpoints to register/login and obtain a JWT token. Add the token to the `Authorization: Bearer <token>` header for authenticated requests.
- Static files: Files are served from the `Files` directory in the application content root and are available under the `/Files` request path.

Main API areas (controllers available in the `E-Commerce` project):
- `AuthController` — account registration and token issuance
- `ProductController` — product CRUD and queries
- `CategoryController` — category CRUD
- `CartController` — shopping cart operations
- `OrderController` — order placement and history
- `ImageController` — uploading and serving images

Refer to the controllers' action methods for specific routes and request/response DTOs.

## Dependencies
This solution uses (not exhaustive):
- Microsoft.AspNetCore.App (ASP.NET Core)
- Microsoft.EntityFrameworkCore + EF Core provider for SQL Server
- Microsoft.AspNetCore.Identity
- Microsoft.AspNetCore.Authentication.JwtBearer
- Scalar.AspNetCore (API reference helper mapped in development)
- FluentValidation (used for DTO validators)

Check the individual `.csproj` files for exact package versions.

## Development / Contribution
Contributions are welcome. Suggested workflow:
- Fork the repository and create a feature branch
- Create small, focused commits and open a pull request with a clear description
- Ensure your changes build and that migrations or seeds are included where required

Coding notes:
- The project is structured using separation of concerns: controllers -> logic/managers -> repositories -> EF models
- Migrations live in the `E-Commerce.Data/Migrations` folder

## Running database migrations manually
If you prefer to run migrations from the CLI:

1. From the solution root (or Data project folder):

   dotnet ef migrations add <Name> --project E-Commerce.Data --startup-project E-Commerce
   dotnet ef database update --project E-Commerce.Data --startup-project E-Commerce

Note: `Program.cs` attempts to call `db.Database.Migrate()` at startup.

## License
No license file is included in this repository. If you intend to make this project public, add a `LICENSE` file (for example, MIT, Apache-2.0, or another license) to clarify terms.

## Contact / Maintainers
This repository was developed as an educational/group project. For contributions or issues, open issues and pull requests on the upstream repository.


---
Generated: Brief README describing the solution, setup and usage. Update this file as needed to reflect project-specific policies or licensing.

