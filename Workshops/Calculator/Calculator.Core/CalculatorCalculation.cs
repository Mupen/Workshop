using System.Text.RegularExpressions;
using System.Globalization;

namespace Calculator.Core
{
    /// <summary>
    /// Responsible for parsing and evaluating mathematical expressions.
    /// Uses a simple left-to-right evaluation model (no operator precedence).
    /// </summary>
    public class CalculatorCalculation
    {
        // Stores previous results for ref1, ref2, etc.
        private readonly List<double> _references = new();

        /// <summary>
        /// Parses and evaluates a mathematical expression string.
        /// Supports numbers and operators (+, -, *, /).
        /// Evaluation is performed strictly left-to-right.
        /// </summary>
        /// <param name="input">User-entered expression.</param>
        /// <returns>The calculated result.</returns>
        /// <exception cref="Exception">Thrown when input is invalid.</exception>
        /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
        public double CalculateExpression(string input)
        {
            ValidateInput(input);

            input = ReplaceReferences(input);
            input = RemoveSpaces(input);

            ValidateInput(input);
            ValidateExpressionFormat(input);

            var tokens = Tokenize(input);
            double result = EvaluateLeftToRight(tokens);

            _references.Add(result);
            return result;
        }

        /// <summary>
        /// Adds two numbers.
        /// </summary>
        public double Add(double a, double b)
        {
            return a + b;
        }

        /// <summary>
        /// Subtracts the second number from the first.
        /// </summary>
        public double Subtract(double a, double b)
        {
            return a - b;
        }

        /// <summary>
        /// Multiplies two numbers.
        /// </summary>
        public double Multiply(double a, double b)
        {
            return a * b;
        }

        /// <summary>
        /// Divides the first number by the second.
        /// </summary>
        /// <exception cref="DivideByZeroException">Thrown when dividing by zero.</exception>
        public double Divide(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Cannot divide by zero.");

            return a / b;
        }

        /// <summary>
        /// Clears all stored references.
        /// </summary>
        public void ClearReferences()
        {
            _references.Clear();
        }

        /// <summary>
        /// Ensures the input is not null, empty, or whitespace.
        /// </summary>
        private void ValidateInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new Exception("Input cannot be empty.");
        }

        /// <summary>
        /// Replaces ref1, ref2, etc. with their stored numeric values.
        /// </summary>
        private string ReplaceReferences(string input)
        {
            /// The regex pattern @"ref(\d+)" searches for:
            /// "ref" followed by one or more digits.
            return Regex.Replace(input, @"ref(\d+)", match =>
            {
                int refNum = int.Parse(match.Groups[1].Value);
                return GetReference(refNum).ToString(CultureInfo.InvariantCulture);
            });
        }

        /// <summary>
        /// Removes all spaces from the input.
        /// </summary>
        private string RemoveSpaces(string input)
        {
            return input.Replace(" ", "");
        }

        /// <summary>
        /// Validates expression format: number (operator number)*
        /// Supports integers and decimals (e.g., 10, 3.5)
        /// Allowed operators: +, -, *, /
        /// Rejects invalid structure like "5+", "+5", "5++2"
        /// </summary>
        private void ValidateExpressionFormat(string input)
        {
            if (!Regex.IsMatch(input, @"^\d+(\.\d+)?([+\-*/]\d+(\.\d+)?)*$"))
                throw new Exception("Invalid input. Format: number operator number ...");
        }

        /// <summary>
        /// Splits the expression into number and operator tokens.
        /// </summary>
        private List<string> Tokenize(string input)
        {
            return Regex.Matches(input, @"\d+(\.\d+)?|[+\-*/]")
                        .Select(m => m.Value)
                        .ToList();
        }

        /// <summary>
        /// Evaluates tokens strictly from left to right.
        /// </summary>
        private double EvaluateLeftToRight(List<string> tokens)
        {
            double result = ParseNumber(tokens[0]);

            for (int i = 1; i < tokens.Count; i += 2)
            {
                string op = tokens[i];
                double next = ParseNumber(tokens[i + 1]);
                result = ApplyOperator(result, op, next);
            }

            return result;
        }

        /// <summary>
        /// Retrieves a stored reference value.
        /// </summary>
        private double GetReference(int refNum)
        {
            if (refNum > 0 && refNum <= _references.Count)
                return _references[refNum - 1];

            throw new Exception($"ref{refNum} not found.");
        }

        /// <summary>
        /// Converts a token into a numeric value.
        /// </summary>
        private double ParseNumber(string token)
        {
            if (!double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                throw new Exception($"'{token}' is not a valid number.");

            return value;
        }

        /// <summary>
        /// Routes an operator symbol to its matching math method.
        /// </summary>
        private double ApplyOperator(double a, string op, double b)
        {
            return op switch
            {
                "+" => Add(a, b),
                "-" => Subtract(a, b),
                "*" => Multiply(a, b),
                "/" => Divide(a, b),
                _ => throw new Exception($"Unsupported operator '{op}'")
            };
        }
    }
}