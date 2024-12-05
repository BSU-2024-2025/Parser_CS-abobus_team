using NUnit.Framework;

namespace Calculator.Test
{
    public class CompilerTests
    {
        [TestCase("1+2", ExpectedResult = 3)]
        [TestCase("1-2", ExpectedResult = -1)]
        [TestCase("1*2", ExpectedResult = 2)]
        [TestCase("1/2", ExpectedResult = 0)]
        [TestCase("1+2*3", ExpectedResult = 7)]
        [TestCase("1-2*3", ExpectedResult = -5)]
        [TestCase("1*2+3", ExpectedResult = 5)]
        [TestCase("-1", ExpectedResult = -1)]
        [TestCase("-1+2", ExpectedResult = 1)]
        [TestCase("-1/2", ExpectedResult = -0)]
        [TestCase("-1-2", ExpectedResult = -3)]
        [TestCase("-1*2", ExpectedResult = -2)]
        [TestCase("-1*2+4", ExpectedResult = 2)]
        [TestCase("-1*2-4", ExpectedResult = -6)]
        [TestCase("-3*4-4", ExpectedResult = -16)]
        [TestCase("123", ExpectedResult = 123)]
        [TestCase("-123", ExpectedResult = -123)]
        [TestCase("123+123", ExpectedResult = 246)]
        [TestCase("123-123", ExpectedResult = 0)]
        [TestCase("123*123", ExpectedResult = 15129)]
        [TestCase("-123*123", ExpectedResult = -15129)]
        public int? TestNoParentheses(string expression)
        {
            return (int?)Parser.CompileExpression(expression);
        }

        [TestCase("(1+2)", ExpectedResult = 3)]
        [TestCase("(1-2)", ExpectedResult = -1)]
        [TestCase("(1*2)", ExpectedResult = 2)]
        [TestCase("(1/2)", ExpectedResult = 0)]
        [TestCase("(1+2)*3", ExpectedResult = 9)]
        [TestCase("(1-2)*3", ExpectedResult = -3)]
        [TestCase("3+(1*2)", ExpectedResult = 5)]
        [TestCase("3*(2+3)", ExpectedResult = 15)]
        [TestCase("3-(1*2+4-3*4)", ExpectedResult = 9)]
        [TestCase("-(1*2+4-3*4)", ExpectedResult = 6)]
        [TestCase("-(1+2)", ExpectedResult = -3)]
        [TestCase("-(-1)", ExpectedResult = 1)]
        [TestCase("-(-(-(-(-(-(-(-(-(-(-(-1)))))))))))", ExpectedResult = 1)]
        [TestCase("2-(-1)", ExpectedResult = 3)]
        [TestCase("-(2+(-1))", ExpectedResult = -1)]
        [TestCase("1+(-1)-(2*3)", ExpectedResult = -6)]
        [TestCase("((1+2)*(2+3)+(-3+4))", ExpectedResult = 16)]
        public int? TestWithParentheses(string expression)
        {
            return (int?)Parser.CompileExpression(expression);
        }
    }
}