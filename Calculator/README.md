# Calculator

## Introduction
A console-based calculator application built with C# using .NET 10.0. The project demonstrates clean code architecture with separated concerns: a calculation engine, user interface menu, and comprehensive unit tests using xUnit.

---

## Assignment Requirements
- The program should perform basic mathematical operations on numeric values entered by the user
  - Addition
  - Subtraction
  - Division
  - Multiplication
 
- Each mathematical operation should be in its own method
- Each operation method should have at least one xUnit test
- Division should inform the user if they try to divide by zero
- Use a loop and a menu system to keep the program running until the user chooses to end it

---

## Extras
- `.gitignore` for the project
- `README.md` for the project

---

## What I Built
I built a console-based calculator with a clear separation between logic and user interaction.

The project is structured into multiple components:
- **CalculatorCalculation** – Handles all calculation logic, including parsing expressions, validating input, and performing operations
- **CalculatorMenu** – Controls the program loop and user interaction
- **CalculatorDisplay** – Responsible for displaying output and messages to the user

The calculator supports:
- Basic operations: addition, subtraction, multiplication, and division
- Left-to-right evaluation of expressions (no operator precedence)
- Decimal numbers (e.g., `3.5 + 2`)
- Expressions with spaces and without.
- Saving results as references (`ref1`, `ref2`, etc.) that can be reused in later calculations
- Clearing stored references

The project also includes a comprehensive xUnit test suite that verifies:
- Correct calculations
- Input validation and error handling
- Edge cases such as divide-by-zero and invalid expressions
- Reference storage and reuse

---

## Why I Did It This Way
I focused on separating concerns to make the code easier to understand, test, and maintain.

- **Cleaner console experience**  
  I noticed that console output can quickly become cluttered and hard to follow. To address this, I designed the program so that help and guidance messages are shown in a way that naturally clears or replaces previous output, keeping the screen readable and focused.  

  At the same time, calculation results are kept concise and remain visible until the user chooses to clear them manually. This balance prevents unnecessary clutter while still allowing the user to review their work when needed.

- **Separation of logic and UI**  
  The calculation engine is independent of the menu and display. This allows the logic to be tested without relying on user input or console output.

- **Method-based operations**  
  Each mathematical operation is implemented in its own method (`Add`, `Subtract`, `Multiply`, `Divide`) to meet the assignment requirements and improve clarity.

- **Custom expression parsing**  
  Instead of using built-in evaluation libraries, I implemented manual parsing and validation. This gave me full control over:
  - Supported formats
  - Error handling
  - Left-to-right evaluation behavior

- **Test-driven improvements**  
  The test suite was used to verify correctness and identify edge cases. Tests were added for both valid and invalid inputs to ensure the calculator behaves predictably.

- **Readable and maintainable structure**  
  I prioritized clear naming, small methods, and explicit logic over overly compact code to make the project easier to follow.

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