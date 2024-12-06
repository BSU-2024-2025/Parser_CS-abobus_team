using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Interpreter.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
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
    [TestCase("""
              return "str";
              """, ExpectedResult = "str")]
    [TestCase("""
              x = "str";
              return x;
              """, ExpectedResult = "str")]
    [TestCase("""
              x = "str1";
              y = "str2";
              return x + y;
              """, ExpectedResult = "str1str2")]
    [TestCase("""
              x = "str1";
              y = "str2";
              return y + x;
              """, ExpectedResult = "str2str1")]
    [TestCase("""
              x = "str1";
              y = "str2";
              z = x + y;
              return y + x + z;
              """, ExpectedResult = "str2str1str1str2")]
    [TestCase("""
              x = true;
              return x;
              """, ExpectedResult = true)]
    [TestCase("""
              x = true;
              return !x;
              """, ExpectedResult = false)]
    [TestCase("""
              x = false;
              return !x;
              """, ExpectedResult = true)]
    [TestCase("""
              x = -(-(-1));
              return x;
              """, ExpectedResult = -1)]
    [TestCase("""
              x = true;
              """, ExpectedResult = null)]
    [TestCase("""
              x = false;
              return x;
              """, ExpectedResult = false)]
    [TestCase("""
              x = false;
              y = x;
              return y;
              """, ExpectedResult = false)]
    [TestCase("""
              x = 3;
              y = 2;
              return x > y;
              """, ExpectedResult = true)]
    [TestCase("""
              x = 3;
              y = 2;
              return x < y;
              """, ExpectedResult = false)]
    [TestCase("""
              x = 3;
              y = 2;
              return x >= y;
              """, ExpectedResult = true)]
    [TestCase("""
              x = 2;
              y = 2;
              return x >= y;
              """, ExpectedResult = true)]
    [TestCase("""
              x = 2;
              y = 2;
              return x <= y;
              """, ExpectedResult = true)]
    [TestCase("""
              return 3 >= 3;
              """, ExpectedResult = true)]
    [TestCase("""
              return 4 >= 3;
              """, ExpectedResult = true)]
    [TestCase("""
              return 4 <= 3;
              """, ExpectedResult = false)]
    [TestCase("""
              return 4 <= 3 && 9 > 1;
              """, ExpectedResult = false)]
    [TestCase("""
              return 4 >= 3 && 9 > 1;
              """, ExpectedResult = true)]
    [TestCase("""
              return (4 >= 3) && (9 > 1);
              """, ExpectedResult = true)]
    [TestCase("""
              return 10 > 3 || 9 < 1;
              """, ExpectedResult = true)]
    [TestCase("""
              return (10 > 3) || (9 < 1);
              """, ExpectedResult = true)]
    [TestCase("""
              return 1 > 2 || 4 < 3;
              """, ExpectedResult = false)]
    [TestCase("""
              return (1 > 2) || (4 < 3);
              """, ExpectedResult = false)]
    [TestCase("""
              return (!(1 > 2)) || (!(4 < 3));
              """, ExpectedResult = true)]
    [TestCase("""
              return (1+2);
              """, ExpectedResult = 3)]
    [TestCase("""
              x = 1;
              if (1 > 2)
              {
                x = 3;
              }
              return x;
              """, ExpectedResult = 1)]
    [TestCase("""
              y = 1;
              x = 0;
              if (y == 2){
               x = 3;
               }
              else{
              x = 5;
              }
              return x;
              """, ExpectedResult = 5)]
    [TestCase("""
              y = 0;
              while (y <= 5) {
                y = y + 1;
              }
              return y;
              """, ExpectedResult = 6)]
    [TestCase("""
              if (2 == 2){
               return 3;
              }else if (4 == 4){
               return 6;
              }else{
               return 7;
              }
              return 8;
              """, ExpectedResult = 3)]
    [TestCase("""
              if (2 != 2){
               return 3;
              }else if (4 == 4){
               return 6;
              }else{
               return 7;
              }
              return 8;
              """, ExpectedResult = 6)]
    [TestCase("""
              if (2 != 2){
               return 3;
              }else if (4 < 4){
               return 6;
              }else{
               return 7;
              }
              return 8;
              """, ExpectedResult = 7)]
    [TestCase("""
              if (2 != 2){
               return 3;
              }else if (4 < 4){
               return 6;
              }

              return 8;
              """, ExpectedResult = 8)]
    [TestCase("""
              if (1 >= 2){
               return 3;
              }else if (3 <= 4){
               if (true) {
                 return 5;
               } else {
                 return 6;
               }
              }
              return 8;
              """, ExpectedResult = 5)]
    [TestCase("""
              if (1 >= 2){
               return 3;
              }else if (3 <= 4){
               if (!true) {
                 return 5;
               } else {
                 return 6;
               }
              }
              return 8;
              """, ExpectedResult = 6)]
    [TestCase("""
              if (1 <= 2){
               return 3;
              }else if (3 <= 4){
               if (true) {
                 return 5;
               } else {
                 return 6;
               }
              }
              return 8;
              """, ExpectedResult = 3)]
    [TestCase("""
              if 3 == 3 {
               return 1;
              }
              return 6;
              """, ExpectedResult = 1)]
    [TestCase("""
              if 3 != 3 {
               return 1;
              }
              return 6;
              """, ExpectedResult = 6)]
    [TestCase("""
               fun name()
               {
                   return "Hello World!";
               }
               return;
              """,ExpectedResult = 0)]
    public object? TestCompiler(string input)
    {
        var p = new Parser(input);
        var c = new Compiler(p.Parse());
        return c.Compile();
    }

    [TestCase("""
              y = 1;
              x = 0;
              if (y == 2){
               x = 3;
               
              else{
              x = 5;
              }
              return x;
              """)]
    public void BadTestCompiler(string input)
    {
        try
        {
            var p = new Parser(input);
            var c = new Compiler(p.Parse());
            Assert.IsFalse(true);
        }
        catch (Exception e)
        {
            Assert.IsTrue(true);
        }
    }
}