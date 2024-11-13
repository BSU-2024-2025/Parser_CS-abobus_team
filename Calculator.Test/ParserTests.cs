using NUnit.Framework;

namespace Calculator.Test
{
  public class ParserTests
  {
    [TestCase("2/*1+-")]
    [TestCase("*8")]
    [TestCase("5/")]
    [TestCase("((1+2)-3")]
    [TestCase("((((1+(2-3*(2+3))-(2*(4-5)*3))")]
    [TestCase("1 2")]
    [TestCase("12 131 41242 ")]
    [TestCase("1+-2")]
    [TestCase("1 + - 2")]
    [TestCase("1 + tyhgjg - 2")]
    [TestCase("1 + 2 / / /")]
    [TestCase(
          """ 
        1 + ( - 1 ) 
        1+(-1)
      """)]
    [TestCase(
          """ 
        1 + ( - 1 ) 
        /
        /
        /

        /jhgjg
        1+(-1)
      """)]
    [TestCase(
          """ 
        1 + ( - 1 ) 
        /
        /
        1+(-1)
      """)]
    public static void TestParseFalse(string input)
    {
      bool result = Parser.ParseExpression(input);
      Assert.That(result, Is.EqualTo(false));
    }

    [TestCase("(1+2)-(2*3)")]
    [TestCase("1")]
    [TestCase(" 1 ")]
    [TestCase(" 1 + 2 ")]
    [TestCase("1213141242")]
    [TestCase(" -1 ")]
    [TestCase("-(2*(2/3))")]
    [TestCase("1+(-1)")]
    [TestCase(" 1 + ( - 1 ) ")]
    [TestCase(
      """ 
        1 +( - 1 ) * 
        1+(-1)
      """)]
    [TestCase(
      """ 
        23 * ( 3 -
        1+(-1) )
      """)]
    [TestCase(
      """ 
        23 *    ( 3 -
        1+(-1) )
      """)]
    [TestCase("1+\t(-1)")]
    [TestCase(
      """ 
        23 *    ( 3 -
        1+(-1) ) // tyui hjkl
        // skjfnsd 
        +1
      """)]
    [TestCase(
      """ 
        23 *    ( 3 - //
        1+(-1) ) // sjknfsdk jksadd
      """)]
    [TestCase(
      """ 
       23 *    ( 3 - // FHJASFHAI
      // FHJASFHAI
           // FHJASFHAI

              
      // FHJASFHAI // FHJASFHAI
        1+(-1) ) // hekjsdd sdakajfnsd
      """)]
    public void TestParseTrue(string input)
    {
      bool result = Parser.ParseExpression(input);
      Assert.That(result, Is.EqualTo(true));
    }

    [TestCase(" x = 1;")]
    // [TestCase("y+1")]
    // [TestCase("_x + 1")]
    [TestCase("""
      x = 1;
      y = x;
    """)]
    [TestCase("""
        x = 1;
        x = x + 1;
    """)]
    public void VariableTest(string input)
    {
      var result = Parser.Parse(input);
      Assert.That(result, Is.EqualTo(true));
    }

    // [TestCase("y+1")]
    // [TestCase("_x + 1")]
    [TestCase("""
      y = x;
      x = 1;
    """)]
    [TestCase("""
      x = x + 1;
    """)]
    [TestCase("return = 2;")]
    [TestCase("x = return + 1")]
    [TestCase("return x = x + 1")]
    public void VariableTestFalse(string input)
    {
      var result = Parser.Parse(input);
      Assert.That(result, Is.EqualTo(false));
    }

    [TestCase("""
          x = 1+2;
          return x;
    """, ExpectedResult = 3)]
    [TestCase("""
          return 3;
          """, ExpectedResult = 3)]
    [TestCase("""
          return 3 + 1;
          """, ExpectedResult = 4)]
    [TestCase("""
          x = 1+2;
          return x * 3;
          """, ExpectedResult = 9)]
    [TestCase("""
          x = 1+2;
          y = x * 3;
          return y + x;
          """, ExpectedResult = 12)]
    [TestCase("""
          x = 1+2;
          y = x * 3;
          return y + x;
          return x;
          """, ExpectedResult = 12)]
    [TestCase("""
          return;
          """, ExpectedResult = 0)]
    public static int? TestCompile(string input)
    {
      var result = Parser.Compile(input);
      return (int?)result;
    }
  }
}