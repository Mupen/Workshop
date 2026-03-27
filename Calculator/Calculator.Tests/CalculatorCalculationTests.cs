using Calculator.Core;

namespace Calculator.Tests
{
    public class CalculatorCalculationTests
    {
        /// <summary>
        /// Tests basic arithmetic operations (+, -, *, /)
        /// Ensures correct results for simple expressions
        /// </summary>
        [Theory]
        [InlineData("2+3", 5)]
        [InlineData("10-4", 6)]
        [InlineData("6*7", 42)]
        [InlineData("20/5", 4)]
        public void CalculateExpression_BasicOperations_ReturnsExpectedResult(string input, double expected)
        {
            var calc = new CalculatorCalculation();

            double result = calc.CalculateExpression(input);

            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Verifies expressions are evaluated left-to-right
        /// Confirms no operator precedence is applied
        /// </summary>
        [Fact]
        public void CalculateExpression_EvaluatesLeftToRight()
        {
            var calc = new CalculatorCalculation();

            double result = calc.CalculateExpression("10+5*2");

            Assert.Equal(30, result);
        }

        /// <summary>
        /// Ensures division by zero throws an exception
        /// </summary>
        [Fact]
        public void CalculateExpression_DivideByZero_ThrowsDivideByZeroException()
        {
            var calc = new CalculatorCalculation();

            Assert.Throws<DivideByZeroException>(() => calc.CalculateExpression("10/0"));
        }

        /// <summary>
        /// Ensures empty or whitespace input throws an error
        /// </summary>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void CalculateExpression_EmptyInput_ThrowsException(string input)
        {
            var calc = new CalculatorCalculation();

            var ex = Assert.Throws<Exception>(() => calc.CalculateExpression(input));

            Assert.Equal("Input cannot be empty.", ex.Message);
        }

        /// <summary>
        /// Tests invalid input like non-numeric characters
        /// Ensures expression is rejected
        /// </summary>
        [Theory]
        [InlineData("abc")]
        [InlineData("2+a")]
        [InlineData("hello+5")]
        public void CalculateExpression_InvalidCharacters_ThrowsException(string input)
        {
            var calc = new CalculatorCalculation();

            var ex = Assert.Throws<Exception>(() => calc.CalculateExpression(input));

            Assert.Equal("Invalid input. Format: number operator number ...", ex.Message);
        }

        /// <summary>
        /// Tests malformed expressions like "5+", "+5", "5++2"
        /// Ensures incorrect structure is rejected
        /// </summary>
        [Theory]
        [InlineData("5+")]
        [InlineData("+5")]
        [InlineData("5++2")]
        [InlineData("10*")]
        [InlineData("8//2")]
        public void CalculateExpression_InvalidStructure_ThrowsException(string input)
        {
            var calc = new CalculatorCalculation();

            var ex = Assert.Throws<Exception>(() => calc.CalculateExpression(input));

            Assert.Equal("Invalid input. Format: number operator number ...", ex.Message);
        }

        /// <summary>
        /// Tests unsupported input like negatives and parentheses
        /// Ensures they are rejected
        /// </summary>
        [Theory]
        [InlineData("(5+2)")]
        [InlineData("-5+2")]
        [InlineData("4*(-2)")]
        public void CalculateExpression_UnsupportedFeatures_ThrowsException(string input)
        {
            var calc = new CalculatorCalculation();

            var ex = Assert.Throws<Exception>(() => calc.CalculateExpression(input));

            Assert.Equal("Invalid input. Format: number operator number ...", ex.Message);
        }

        /// <summary>
        /// Ensures expressions with spaces are handled correctly
        /// </summary>
        [Fact]
        public void CalculateExpression_ValidExpressionWithSpaces_ReturnsExpectedResult()
        {
            var calc = new CalculatorCalculation();

            double result = calc.CalculateExpression(" 8 + 2 * 3 ");

            Assert.Equal(30, result);
        }

        /// <summary>
        /// Tests that stored references (ref1, ref2) work correctly
        /// </summary>
        [Fact]
        public void CalculateExpression_ReferenceCanBeUsedInLaterExpression()
        {
            var calc = new CalculatorCalculation();

            calc.CalculateExpression("10+5"); // ref1 = 15
            double result = calc.CalculateExpression("ref1*2");

            Assert.Equal(30, result);
        }

        /// <summary>
        /// Ensures using a non-existing reference throws an error
        /// </summary>
        [Fact]
        public void CalculateExpression_MissingReference_ThrowsException()
        {
            var calc = new CalculatorCalculation();

            var ex = Assert.Throws<Exception>(() => calc.CalculateExpression("ref1+2"));

            Assert.Equal("ref1 not found.", ex.Message);
        }

        /// <summary>
        /// Ensures clearing references removes stored values
        /// </summary>
        [Fact]
        public void ClearReferences_RemovesStoredReferences()
        {
            var calc = new CalculatorCalculation();

            calc.CalculateExpression("4+1"); // ref1 = 5
            calc.ClearReferences();

            var ex = Assert.Throws<Exception>(() => calc.CalculateExpression("ref1+2"));

            Assert.Equal("ref1 not found.", ex.Message);
        }

        /// <summary>
        /// Ensures multiple results are stored and used correctly
        /// </summary>
        [Fact]
        public void CalculateExpression_MultipleCalculations_CreateMultipleReferences()
        {
            var calc = new CalculatorCalculation();

            calc.CalculateExpression("2+3"); // ref1 = 5
            calc.CalculateExpression("4+6"); // ref2 = 10
            double result = calc.CalculateExpression("ref1+ref2");

            Assert.Equal(15, result);
        }

        /// <summary>
        /// Tests decimal input and calculations
        /// Ensures correct floating-point results
        /// </summary>
        [Theory]
        [InlineData("3.5+2", 5.5)]
        [InlineData("10.5-0.5", 10.0)]
        [InlineData("2.5*4", 10.0)]
        [InlineData("9.0/2", 4.5)]
        public void CalculateExpression_Decimals_ReturnExpectedResult(string input, double expected)
        {
            var calc = new CalculatorCalculation();

            double result = calc.CalculateExpression(input);

            Assert.Equal(expected, result, 10);
        }

        /// <summary>
        /// Validates expression format: number (operator number)*
        /// Supports integers and decimals (e.g., 10, 3.5)
        /// Allowed operators: +, -, *, /
        /// Rejects invalid structure like "5+", "+5", "5++2"
        /// </summary>
        [Theory]
        [InlineData("10")]
        [InlineData("3.5")]
        [InlineData("10+2")]
        [InlineData("7.5*2")]
        [InlineData("100/4-10")]
        public void CalculateExpression_ValidFormats_ReturnResult(string input)
        {
            var calc = new CalculatorCalculation();

            double result = calc.CalculateExpression(input);

            Assert.True(double.IsFinite(result));
        }

        [Fact]
        public void Add_TwoNumbers_ReturnsSum()
        {
            var calc = new CalculatorCalculation();

            double result = calc.Add(2, 3);

            Assert.Equal(5, result);
        }

        [Fact]
        public void Subtract_TwoNumbers_ReturnsDifference()
        {
            var calc = new CalculatorCalculation();

            double result = calc.Subtract(10, 4);

            Assert.Equal(6, result);
        }

        [Fact]
        public void Multiply_TwoNumbers_ReturnsProduct()
        {
            var calc = new CalculatorCalculation();

            double result = calc.Multiply(6, 7);

            Assert.Equal(42, result);
        }

        [Fact]
        public void Divide_TwoNumbers_ReturnsQuotient()
        {
            var calc = new CalculatorCalculation();

            double result = calc.Divide(20, 5);

            Assert.Equal(4, result);
        }

        [Fact]
        public void Divide_ByZero_ThrowsDivideByZeroException()
        {
            var calc = new CalculatorCalculation();

            Assert.Throws<DivideByZeroException>(() => calc.Divide(10, 0));
        }
    }
}