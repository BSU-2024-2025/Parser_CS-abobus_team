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
            bool result = Parser.Parse(input);
            Assert.That(result, Is.EqualTo(false));
        }

        [TestCase("(2*(2/3))")]
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
            bool result = Parser.Parse(input);
            Assert.That(result, Is.EqualTo(true));
        }
    }
}