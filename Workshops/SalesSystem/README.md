# SalesSystem

## Introduction
A console-based cinema sales system built with C# using .NET 10.0. The project is structured using a layered architecture separating domain logic, application use cases, and user interaction. It includes unit tests using xUnit to verify core functionality and ensure correctness.

---

## Assignment Requirements
- The program should allow interaction through a console-based menu system
- The system should handle core cinema-related operations:
  - Managing movies
  - Managing showings
  - Selling tickets
  - Handling basic product sales

- The program should:
  - Use a loop and a menu system to keep running until the user exits
  - Separate logic into appropriate layers (Domain, Application, API)
  - Use clear methods and structured flow for each operation

---

## Extras
- `.gitignore` for the project  
- `README.md` for the project  
- Layered architecture (Domain, Application, Infrastructure, API)  
- Use of records for requests and read models  
- Structured query and use case pattern  

---

## What I Built
I built a console-based cinema sales system with a focus on clean architecture and separation of concerns.

The project is structured into multiple layers:

- **Domain** – Core business logic, entities, enums, and rules  
- **Application** – Use cases, queries, requests, and interfaces  
- **Infrastructure** – Repository implementations and services  
- **API (Console UI)** – Menu system and user interaction  

The system currently supports:

- Managing movies with:
  - Title, description, year, age rating, and duration  

- Managing showings:
  - Date and start time  
  - Seat generation and availability tracking  

- Ticket sales workflow:
  - Selecting movies and showings  
  - Viewing available seats  
  - Adding tickets with different age categories:
    - Adult
    - Child
    - Youth
    - Senior  
  - Building a cart before confirming a transaction  

- Product handling (foundation in place):
  - Products with pricing and VAT logic  
  - Integration ready for extended sales  

- Console-based command system:
  - Fast input commands (e.g., `SM:1`, `SS:2`, `AAT:14`)  
  - Help system for command reference  
  - Clean UI with minimal clutter and focused interaction  

The project also includes unit tests verifying:
- menu behavior and ui tools

---

## Why I Did It This Way

### Clean separation of concerns
I structured the system into layers to clearly separate responsibilities:

- Domain handles business rules  
- Application coordinates logic through use cases and queries  
- API handles user interaction  

This makes the system easier to test, extend, and maintain.

---

### State-driven UI design
Instead of relying on history logs, the UI is built around **current state**:

- Selected movie  
- Selected showing  
- Available seats  
- Current ticket cart  

This reduces clutter and makes the system faster to use for trained operators.

---

### Command-based interaction
The system uses short commands instead of verbose menus:

- Faster for experienced users  
- Reduces screen noise  
- Scales well as more features are added  

A help screen (`H`) provides full command documentation when needed.

---

### Read models and queries
Instead of exposing domain entities directly to the UI, I use:

- **Read models** (e.g., `ShowingListItem`)  
- **Queries** to shape data for display  

This keeps the UI simple and avoids leaking domain complexity.

---

### Use case-driven logic
All operations are handled through explicit use cases, such as:

- Creating movies  
- Creating showings  
- Selling tickets  

This ensures that business logic is centralized and reusable.

---

### Incremental development
The system was built step by step:

1. Core domain and entities  
2. Queries and read models  
3. Menu system  
4. Ticket sales flow  
5. UI refinement  

This made it easier to validate each part before moving forward.

---

## AI Disclosure
This project was written by Daniel Henriksen. ChatGPT (AI by OpenAI) was used as a collaborative tool throughout the process.

My workflow was to build functional features independently, then use ChatGPT to:
- Identify architectural improvements  
- Debug issues and trace errors  
- Refine structure and naming  
- Assist with documentation and explanations  

All core logic, structure, and design decisions are my own work.  
ChatGPT acted as a reviewer and technical assistant, not as a code generator.

---

## Status
Work in progress – currently focusing on:
- Completing ticket sales flow  
- Improving UI and seat handling  
- Expanding product and order management  