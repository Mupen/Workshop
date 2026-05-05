# Documentation Index

Version: v0.1.0

This folder contains the final project documentation for the ShoppingCart workshop backend API.

## Project Summary

ShoppingCart is a backend-first ASP.NET Core Web API for a small online store shopping cart.

The project implements:

- product catalog foundations
- product brands and categories
- shopping carts with product items and quantities
- cart ownership through `UserId`
- SQLite persistence
- Swagger manual testing
- Docker support
- unit tests

The current version does not include a frontend, login, checkout, payment, or order history.

## Current Scope

Completed for this version:

- backend API
- domain entities and business rules
- application use cases and queries
- EF Core persistence
- development seed data
- Docker/Visual Studio Docker support
- project documentation

Experimental placeholder only:

- `User`
- `UserRole`
- user use cases/repository
- seeded demo users

The user foundation is included to support future authentication and realistic cart ownership. It is not a finished login or authorization system.

## Recommended Reading Order

1. [Project/Assignment.md](Project/Assignment.md)
2. [Project/Technology_Stack.md](Project/Technology_Stack.md)
3. [Architecture/Architecture_Documentation.md](Architecture/Architecture_Documentation.md)
4. [Release/Development_Runbook.md](Release/Development_Runbook.md)
5. [Release/Manual_Test_Checklist.md](Release/Manual_Test_Checklist.md)

## Documentation Files

| Path | Audience | Purpose |
| --- | --- | --- |
| [Project/Assignment.md](Project/Assignment.md) | Teachers / developers | Explains the original workshop requirement and how this project maps to it. |
| [Project/Technology_Stack.md](Project/Technology_Stack.md) | Teachers / developers | Describes the runtime, frameworks, tools, patterns, persistence, tests, Docker setup, and current feature scope. |
| [Architecture/Architecture_Documentation.md](Architecture/Architecture_Documentation.md) | Developers / architects | Explains the architecture decisions and dependency direction. |
| [Release/Development_Runbook.md](Release/Development_Runbook.md) | Developers / operators | Explains how to run, build, test, and reset the local app. |
| [Release/Manual_Test_Checklist.md](Release/Manual_Test_Checklist.md) | Developers / reviewers | Lists manual Swagger checks for the important API behavior. |
| [Release/Release_Notes_v0.1.0.md](Release/Release_Notes_v0.1.0.md) | Everyone | Summarizes the current project state. |

## Technical Identity

The project style is:

- layered
- backend-first
- API-driven
- use case-oriented
- domain-rule focused
- test-supported
- simple to run locally

SQLite and Swagger were chosen to keep the workshop easy to run and verify without extra infrastructure.

## Hand-In Notes

The strongest parts of the project are the shopping cart domain behavior, product/catalog support, layered architecture, API endpoints, persistence, Docker setup, and tests.

The account/user area should be treated as future-facing foundation only. It is intentionally documented as incomplete so the project scope is clear.

## Removed During Cleanup

The documentation folder was simplified before hand-in.

Removed as redundant or out of current scope:

- separate frontend/HTMX plan
- separate authentication plan
- separate user guide
- separate architecture decision log
- long internal completion checklist
- duplicate technical documentation

The useful content from those files is now represented in the assignment, technology stack, architecture, runbook, release notes, and manual test checklist.
