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
              x = false;
              return x;
              """, ExpectedResult = false)]
    [TestCase("""
              x = false;
              y = x;
              return y;
              """, ExpectedResult = false)]
    // [TestCase("""
    //           x = 3;
    //           y = 2;
    //           return x > y;
    //           """, ExpectedResult = true)]
    // [TestCase("""
    //           x = 3;
    //           y = 2;
    //           return x < y;
    //           """, ExpectedResult = false)]
    public object? Test1(string input)
    {
        var p = new Parser(input);
        var c = new Compiler(p.Parse());
        return c.Compile();
    }
}