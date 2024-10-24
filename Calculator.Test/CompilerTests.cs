using NUnit.Framework;

namespace Calculator.Test
{
    public class CompilerTests
    {
        [TestCase("1+2", ExpectedResult = 3d)]
        [TestCase("1-2", ExpectedResult = -1d)]
        [TestCase("1*2", ExpectedResult = 2d)]
        [TestCase("1/2", ExpectedResult = 0.5d)]
        [TestCase("1+2*3", ExpectedResult = 7d)]
        [TestCase("1-2*3", ExpectedResult = -5d)]
        [TestCase("1*2+3", ExpectedResult = 5d)]
        [TestCase("-1", ExpectedResult = -1d)]
        [TestCase("-1+2", ExpectedResult = 1d)]
        [TestCase("-1/2", ExpectedResult = -0.5d)]
        [TestCase("-1-2", ExpectedResult = -3d)]
        [TestCase("-1*2", ExpectedResult = -2d)]
        [TestCase("-1*2+4", ExpectedResult = 2d)]
        [TestCase("-1*2-4", ExpectedResult = -6d)]
        [TestCase("-3*4-4", ExpectedResult = -16d)]
        [TestCase("123", ExpectedResult = 123d)]
        [TestCase("-123", ExpectedResult = -123d)]
        [TestCase("123+123", ExpectedResult = 246d)]
        [TestCase("123-123", ExpectedResult = 0d)]
        [TestCase("123*123", ExpectedResult = 15129d)]
        [TestCase("-123*123", ExpectedResult = -15129d)]
        public decimal TestNoParentheses(string expression)
        {
            return Parser.Compile(expression);
        }

        [TestCase("(1+2)", ExpectedResult = 3d)]
        [TestCase("(1-2)", ExpectedResult = -1d)]
        [TestCase("(1*2)", ExpectedResult = 2d)]
        [TestCase("(1/2)", ExpectedResult = 0.5d)]
        [TestCase("(1+2)*3", ExpectedResult = 9d)]
        [TestCase("(1-2)*3", ExpectedResult = -3d)]
        [TestCase("3+(1*2)", ExpectedResult = 5d)]
        [TestCase("3*(2+3)", ExpectedResult = 15d)]
        [TestCase("3-(1*2+4-3*4)", ExpectedResult = 9d)]
        [TestCase("-(1*2+4-3*4)", ExpectedResult = 6d)]
        [TestCase("-(1+2)", ExpectedResult = -3d)]
        [TestCase("-(-1)", ExpectedResult = 1d)]
        [TestCase("-(-(-(-(-(-(-(-(-(-(-(-1)))))))))))", ExpectedResult = 1d)]
        [TestCase("2-(-1)", ExpectedResult = 3d)]
        [TestCase("-(2+(-1))", ExpectedResult = -1d)]
        [TestCase("1+(-1)-(2*3)", ExpectedResult = -6d)]
        [TestCase("((1+2)*(2+3)+(-3+4))", ExpectedResult = 16d)]
        public decimal TestWithParentheses(string expression)
        {
            return Parser.Compile(expression);
        }
    }
}