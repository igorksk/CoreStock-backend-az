# CoreStock API — Backend

ASP.NET Core 8 Web API for inventory and financial accounting of PC hardware components. Clean Architecture, EF Core InMemory, Swagger.

## Project structure

```
CoreStock.Domain/          # Entities, enums — no external dependencies
CoreStock.Application/     # Business logic, DTOs, service interfaces
CoreStock.Infrastructure/  # EF Core InMemory, data seeder
CoreStock.API/             # Controllers, DI composition root, Swagger
```

## Run locally

```bash
dotnet run --project CoreStock.API
```

API: **http://localhost:5116**  
Swagger: **http://localhost:5116/swagger**

Demo data is seeded automatically on startup — 21 products, 3 suppliers, 12 purchase orders, ~70 sales orders.

## API endpoints

| Resource | Endpoints |
|----------|-----------|
| Products | `GET/POST /api/products` · `GET/PUT/DELETE /api/products/{id}` · `GET /api/products/low-stock` |
| Catalog | `GET /api/catalog/categories` · `/brands` · `/suppliers` |
| Purchases | `GET/POST /api/purchases` · `POST /api/purchases/{id}/receive` · `/cancel` |
| Orders | `GET/POST /api/orders` · `PATCH /api/orders/{id}/status` · `POST /api/orders/{id}/cancel` |
| Reports | `GET /api/reports/financial-summary` · `/daily` · `/monthly` · `/top-products` · `/inventory` · `/dashboard` |

## Tech stack

| | |
|-|-|
| Runtime | .NET 8 |
| ORM | EF Core 8 (InMemory — no DB setup needed) |
| Mapping | AutoMapper 13 |
| Validation | FluentValidation 11 |
| API docs | Swashbuckle / Swagger |

## Deploy to Azure App Service

```bash
dotnet publish CoreStock.API -c Release -o ./publish

az webapp deploy \
  --name <APP_SERVICE_NAME> \
  --resource-group <RESOURCE_GROUP> \
  --src-path ./publish \
  --type zip
```

Set the `ALLOWED_ORIGINS` app setting to the frontend URL (e.g. `https://your-app.azurestaticapps.net`).

GitHub Actions workflow: [`.github/workflows/backend.yml`](../.github/workflows/backend.yml)

Required secrets: `AZURE_WEBAPP_NAME`, `AZURE_WEBAPP_PUBLISH_PROFILE`
