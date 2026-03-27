namespace Calculator.Core
{
    /// <summary>
    /// Handles all console input/output and visual behavior.
    /// Responsible for displaying messages, history, and clearing UI.
    /// </summary>
    public class CalculatorDisplay
    {
        // Stores previous calculations shown to the user
        private readonly List<string> history = new();

        // Tracks where the welcome message ends in the console
        private int welcomeLineEnd;

        /// <summary>
        /// Displays the welcome message when the program starts.
        /// </summary>
        public void WelcomeMessage()
        {
            Console.WriteLine("Calculator - Type 'help' for commands or 'exit' to quit.\n");
            welcomeLineEnd = Console.CursorTop;
        }

        /// <summary>
        /// Adds a new calculation result to history and prints it.
        /// </summary>
        public void UpdateHistory(string expr, string result)
        {
            string line = $"{expr} = {result}";
            history.Add(line);
            Console.WriteLine(line);
        }

        /// <summary>
        /// Clears console output except for the welcome message.
        /// </summary>
        public void ClearCommand()
        {
            int currentLine = Console.CursorTop;

            // Overwrite lines after welcome message
            for (int i = welcomeLineEnd; i < currentLine; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, welcomeLineEnd);
        }

        /// <summary>
        /// Displays exit message.
        /// </summary>
        public void ExitCommand()
        {
            Console.WriteLine("Exiting Program...");
        }

        /// <summary>
        /// Displays help screen temporarily, then clears it.
        /// </summary>
        public void HelpCommand(int lineBeforeInput)
        {
            Console.WriteLine("--- Calculator Help Start ---");
            Console.WriteLine("Commands:");
            Console.WriteLine("  help   - show this help screen");
            Console.WriteLine("  clear  - clear all calculation history");
            Console.WriteLine("  exit   - close the program");

            Console.WriteLine("\nEnter a calculation (e.g., 10 + 5 * 2)");
            Console.WriteLine("Expressions are evaluated strictly left-to-right.");
            Console.WriteLine("Example: 10 + 5 * 2 = 30 (not 20)");

            Console.WriteLine("Supported operators: +, -, *, /");
            Console.WriteLine("Division by zero will show an error.");
            Console.WriteLine("--- Calculator Help End ---");

            Console.ReadKey(true);

            int endLine = Console.CursorTop;

            // Clear help text
            for (int i = lineBeforeInput; i < endLine; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, lineBeforeInput);
        }

        /// <summary>
        /// Displays error message temporarily, then clears it.
        /// </summary>
        public void ErrorCommand(string errorMessage, int lineBeforeInput)
        {
            Console.WriteLine("--- Error Log Start ---");
            Console.WriteLine($"Error: {errorMessage}");
            Console.WriteLine("--- Error Log End ---");

            Console.ReadKey(true);

            int endLine = Console.CursorTop;

            // Clear error output
            for (int i = lineBeforeInput; i < endLine; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, lineBeforeInput);
        }
    }
}