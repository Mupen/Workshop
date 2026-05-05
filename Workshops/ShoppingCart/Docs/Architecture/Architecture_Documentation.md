# Architecture Documentation

Version: v0.1.0

## Purpose

This document explains the structure and design choices of the ShoppingCart backend API.

## Architecture Style

This project uses a layered architecture:

```text
Api -> Application -> Domain
Infrastructure -> Application -> Domain
```

The dependency direction points inward toward the domain.

## Layer Responsibilities

| Layer | Responsibility |
| --- | --- |
| Api | HTTP controllers, request/response DTOs, Swagger, HTTP error mapping |
| Application | Use cases, queries, request models, read models, repository interfaces |
| Domain | Entities, enums, business rules, validation, Result contracts |
| Infrastructure | EF Core, SQLite, migrations, seed data, repository implementations |
| UnitTests | Tests for domain, application, query, and persistence behavior |

## Important Rule

Business rules belong in the Domain layer.

Controllers should not decide whether a product is valid, whether stock can be changed, or whether a user role is valid. They should call application use cases and map the result to HTTP.

## Read Models

Read models are used when a query needs display-friendly data.

Example:

```text
ProductDetails
```

The product domain entity stores `BrandId` and `CategoryId`.

The product read model adds:

```text
BrandName
CategoryName
```

That makes API responses easier to understand without changing the domain entity.

## Users And Roles Placeholder

The project uses:

```text
User + UserRole
```

instead of subclasses like:

```text
Admin : User
Customer : User
Clerk : User
```

This matches the common account/authorization model used in web applications.

In the current hand-in, this is only a foundation. It is not a completed authentication or authorization system.

Current limitations:

- no login endpoint
- no frontend account flow
- no password hashing service
- no cookie/JWT authentication
- no role-based authorization checks on controllers

The workshop only requires the shopping cart to have a user id. The user model was added to experiment with a realistic future direction, but the finished assignment scope should be judged mainly on products and shopping carts.

## Key Design Decisions

### Use User + Role Instead Of User Subclasses

The project uses one `User` entity with a `UserRole` enum instead of subclasses such as `Admin`, `Customer`, and `Clerk`.

This keeps account data in one place and matches how ASP.NET Core authorization commonly works. In this version it remains placeholder/foundation only.

### Use Result For Expected Business Errors

Expected failures return `Result` or `Result<T>`.

Examples:

- duplicate SKU
- missing product
- invalid quantity
- insufficient stock

This avoids using exceptions for normal business validation.

### Use Read Models For Display-Friendly Queries

Product queries return data shaped for API consumers.

For example, a product stores `BrandId` and `CategoryId`, but the API response also includes `BrandName` and `CategoryName`.
