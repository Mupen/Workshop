# Technology Stack

Version: v0.1.0

## Overview

ShoppingCart is a backend-first ASP.NET Core Web API built for the shopping cart workshop assignment.

The project focuses on:

- clean layered architecture
- explicit domain rules
- use case-driven application logic
- SQLite persistence
- Swagger-based API testing
- Docker support
- unit test coverage for core behavior

## Runtime And Language

| Technology | Use |
| --- | --- |
| C# | Main programming language |
| .NET 10 | Application runtime and SDK |
| ASP.NET Core Web API | HTTP API framework |
| Nullable Reference Types | Safer handling of null values |
| Implicit Usings | Reduced boilerplate in project files |

## API And Documentation

| Technology | Use |
| --- | --- |
| Controllers | HTTP endpoint implementation |
| Swagger / OpenAPI | Manual API testing and endpoint documentation |
| Swashbuckle.AspNetCore | Swagger UI generation |
| Microsoft.AspNetCore.OpenApi | OpenAPI support |
| ProblemDetails | Consistent HTTP error response format |
| JSON enum string conversion | Human-readable enum values in API responses |

## Persistence

| Technology | Use |
| --- | --- |
| Entity Framework Core | Object-relational mapping |
| SQLite | Local file-based database |
| EF Core Migrations | Database schema versioning |
| Repository Pattern | Persistence abstraction behind application interfaces |
| Development Seed Data | Predictable sample users, products, brands, categories, and cart data |

SQLite was chosen because this is a workshop project. It keeps the project easy to run without requiring SQL Server, PostgreSQL, or another database server.

## Architecture

The project uses layered architecture:

```text
ShoppingCart.Api
ShoppingCart.Application
ShoppingCart.Domain
ShoppingCart.Infrastructure
ShoppingCart.UnitTests
```

Dependency direction:

```text
Api -> Application -> Domain
Infrastructure -> Application -> Domain
```

The domain layer is kept independent from ASP.NET Core, EF Core, Swagger, and SQLite.

## Project Layers

| Layer | Responsibility |
| --- | --- |
| Domain | Entities, enums, validation rules, business behavior, Result contracts |
| Application | Use cases, queries, requests, read models, repository interfaces |
| Infrastructure | EF Core DbContext, repositories, migrations, seed data |
| Api | Controllers, DTOs, Swagger, HTTP result mapping |
| UnitTests | Tests for domain behavior, application logic, queries, and persistence |

## Design Patterns And Style

| Pattern / Style | Use |
| --- | --- |
| Use Case Pattern | Keeps workflows explicit and reusable |
| Query Pattern | Separates read operations from commands/use cases |
| Repository Pattern | Keeps application logic independent from EF Core |
| Result Pattern | Represents expected business failures without exceptions |
| DTOs / Records | Keeps HTTP request/response models separate from domain entities |
| Read Models | Shapes API-friendly data such as product brand/category names |
| Factory Methods | Keeps entity creation and validation in the domain |

## Coding Standards

General code style:

- keep business rules in the Domain layer
- keep controllers thin
- call application use cases from controllers
- keep request/response DTOs separate from domain entities
- use clear names for use cases and requests
- use `async` methods for persistence and API workflows
- return `Result` or `Result<T>` for expected business failures
- use exceptions only for unexpected programming/runtime failures
- serialize enums as strings for readable API responses
- prefer small focused tests around domain rules and use cases

The code is intentionally direct and readable because this is a workshop project. The goal is to show clean structure without adding unnecessary enterprise complexity.

## Testing

| Tool / Approach | Use |
| --- | --- |
| xUnit | Unit testing framework |
| EF Core SQLite/In-memory style tests | Persistence behavior checks |
| Swagger manual testing | HTTP endpoint and response verification |

The automated tests verify core behavior, but Swagger testing is still used to confirm routing, HTTP status codes, JSON shape, and real API behavior.

## Docker

| Tool | Use |
| --- | --- |
| Dockerfile | Builds the ShoppingCart API image |
| Docker Compose | Runs the API with a persistent SQLite volume |
| Visual Studio Docker Profile | Allows running the API container from Visual Studio |

The Docker build context is the `Workshops/ShoppingCart` folder because the API image needs access to the API, Application, Domain, and Infrastructure projects.

## Current Implemented Features

- Product catalog API
- Product brand API
- Product category API
- Shopping cart API
- SQLite persistence
- EF Core migrations
- Development seed data
- Swagger UI
- Docker support
- Unit tests
- Documentation

## Experimental Or Planned Features

The following are not finished product features in this version:

- real login
- password hashing service
- authentication middleware
- role-based authorization
- frontend pages
- HTMX interactions
- checkout
- payment
- order history

The `User` and `UserRole` code exists as a placeholder/foundation for later work and to give shopping carts a realistic `UserId`.
