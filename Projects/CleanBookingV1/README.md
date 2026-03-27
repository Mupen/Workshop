# CleanBookingV1

## Introduction
CleanBookingV1 is a console-based booking system built in C# as part of the Lexicon .NET developer program.

The goal of this project is to learn:
- C# and object-oriented programming
- Clean Architecture principles
- Separation of concerns
- Testable and maintainable system design

This is the first version (V1) of the system and focuses on core business logic. Future versions will expand the solution with a Web API, database integration, and a React frontend.

---

## Assignment Requirements
The system is built for a Danish Bed and Breakfast in Skagen.

The application must support:

- Managing 4 different room types with fixed pricing
- Creating bookings
- Checking room availability
- Preventing overlapping bookings
- Calculating total cost for a stay
- Handling check-in and check-out rules
- Handling limited parking availability (2 spaces)

V1 is implemented as a console application used to test and validate business logic.

---

## Extras
- `.gitignore` for the project
- `README.md` for the project

---

## What I Built
A console-based booking system structured using Clean Architecture:

- **Domain**: Core business entities and rules (Room, Booking, DateRange)
- **Application**: Use cases and interfaces for booking logic
- **Infrastructure**: In-memory implementations of repositories and services
- **Api (Console)**: User interaction layer for testing the system
- **UnitTests**: Tests for validating business logic

The system supports creating bookings, checking availability, and calculating pricing while enforcing business rules.

---

## Why I Did It This Way
The project is structured using Clean Architecture to:

- Keep business logic independent of UI and infrastructure
- Make the system easier to test and extend
- Prepare for future expansion (Web API and frontend in V2)
- Practice professional project structure used in real-world applications

The console application is intentionally kept thin and only handles user interaction, while all logic is placed in the core layers.

---

## AI Disclosure
This project was written by Daniel Henriksen. Claude (AI by Anthropic) was used as a
collaborative tool throughout the process.

My workflow was to write a functional end result independently, then use Claude to:
- Identify improvements I had not thought of myself
- Organize and write XML `<summary>` comments across all files
- Discuss and reason through design decisions

All core logic, structure, and creative decisions are my own work.
Claude acted as a reviewer and documentation assistant, not as a code generator.