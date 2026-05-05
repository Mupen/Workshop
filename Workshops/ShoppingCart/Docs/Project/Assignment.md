# Assignment Explanation

Version: v0.1.0

## Original Workshop Requirement

The workshop describes a small online store that sells products and needs a shopping cart backend.

Required parts:

- create a product class with properties and behavior needed by the store
- create a shopping cart class
- the cart tracks different product items
- each cart item has a quantity
- the cart has a user id
- add endpoints for reading, creating, editing, and deleting a shopping cart

The workshop also says that a pragmatic backend can let the frontend handle quantity increase/decrease behavior. The backend only needs endpoints that can support reading, creating, editing, and deleting carts.

## How This Project Meets The Requirement

### Products

The `Product` domain entity includes:

- SKU
- name
- description
- brand id
- category id
- unit price
- inventory tracking flag
- stock quantity
- status
- availability logic

Product behavior includes validation, price changes, status changes, stock increase/decrease, and checking whether a requested cart quantity can be supplied.

### Shopping Cart

The `ShoppingCart` domain entity includes:

- cart id
- user id
- cart items
- total quantity
- total price

Each `ShoppingCartItem` stores:

- product id
- product name
- unit price
- quantity
- line total

The cart supports setting a product quantity. If the quantity is zero, the product is removed from the cart. This matches the workshop's pragmatic approach where the frontend can decide whether the user clicked plus, minus, or remove.

### Endpoints

The cart API supports:

- `GET /api/shopping-carts`
- `GET /api/shopping-carts/{id}`
- `GET /api/shopping-carts/user/{userId}`
- `POST /api/shopping-carts`
- `PUT /api/shopping-carts/{id}/items`
- `DELETE /api/shopping-carts/{id}`

These cover reading, creating, editing, and deleting shopping carts.

## Extra Scope

The project also includes product brands, product categories, Swagger, SQLite persistence, Docker, seed data, and unit tests. These are supporting features for a more realistic API, not extra requirements from the workshop PDF.

## User Scope

The workshop says a `User` class is optional.

This project includes a `User` entity and `UserRole` enum as an experimental foundation for later work, but user management is not finished:

- no real login
- no frontend
- no password hashing service
- no role-based authorization enforcement
- seeded users are development/demo data only

For this hand-in, the important cart requirement is that the cart stores a `UserId`.
