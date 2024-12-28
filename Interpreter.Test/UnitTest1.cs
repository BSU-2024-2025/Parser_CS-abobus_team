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
    public object? TestCompiler(string input)
    {
        //var p = new Parser(input);
        //var c = new Compiler(p.Parse());
        var c = new Compiler(input);
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
             //var p = new Parser(input);
      //var c = new Compiler(p.Parse());
            var c = new Compiler(input);
            c.Compile();
            Assert.IsFalse(true);
        }
        catch (Exception e)
        {
            Assert.IsTrue(true);
        }
    }

  [TestCase("""
               fun foo()
               {
               }
               return;
              """, ExpectedResult = 0)]
  [TestCase("""
               fun foo()
               {
                   
               }
               foo();
               return 1;
              """, ExpectedResult = 1)]
  [TestCase("""
               fun name()
               {
                   return "Hello World!";
               }
               return;
              """, ExpectedResult = 0)]
  [TestCase("""
             fun name()
             {
                 return 1;
             }
             return name();
            """, ExpectedResult = 1)]
  [TestCase("""
             fun name()
             {
                 return 1 + 2;
             }
             return name() + 3;
            """, ExpectedResult = 6)]
  [TestCase("""
             fun foo()
             {
                 x = 1;
                 return x;
             }
             return foo();
            """, ExpectedResult = 1)]
  [TestCase("""
             fun foo()
             {
                 x = 1;
                 y = 2;
                 return x + y;
             }
             return foo() + 3;
            """, ExpectedResult = 6)]
  [TestCase("""
            x = 4;
             fun foo()
             {
                 x = 1;
                 y = 2;
                 return x + y;
             }
             return foo() + 3;
            """, ExpectedResult = 6)]
  [TestCase("""
            
            fun moo()
            {
            }
            
            fun foo()
            {
            }
            
            foo();
            moo();
            return moo() - 1;
            """, ExpectedResult = -1)]
  [TestCase("""
            fun foo()
            {
                return 1;
            }
            x = foo();
            return x + 2;
            """, ExpectedResult = 3)]
  [TestCase("""
            fun foo()
            {
                return 1;
            }
            x = foo() + 3;
            return x + 2;
            """, ExpectedResult = 6)]
  [TestCase("""
            fun foo()
            {
                return 1;
            }

            fun moo()
            {
                return 2;
            }

            x = moo();
            y = foo();
            return x + y;
            """, ExpectedResult = 3)]

  [TestCase("""
            fun foo()
            {
                return 1;
            }

            fun moo()
            {
                return foo() + 2;
            }

            x = moo();
            return x;
            """, ExpectedResult = 3)]
  [TestCase("""
            fun foo()
            {
                return 1;
            }

            fun moo()
            {
                return foo();
            }

            x = moo();
            return x;
            """, ExpectedResult = 1)]
  [TestCase("""
            fun foo()
            {
                x = 1 + 2 * 3;
                return x;
            }
            return foo();
            """, ExpectedResult = 7)]
  [TestCase("""
            x = 1 + 2;
            fun foo()
            {
                x = 1 + 2 * 3;
                return x;
            }
            x = x + 5;
            return foo();
            """, ExpectedResult = 7)]
  [TestCase("""
            x = 3;
            x = x + 5;
            return x;
            """, ExpectedResult = 8)]
  [TestCase("""
            fun foo()
            {
                return 1;
            }

            fun moo()
            {
                return 2;
            }

            x = (moo()) + (foo());
            return x;
            """, ExpectedResult = 3)]
  [TestCase("""
            fun foo()
            {
                return 1;
            }

            fun moo()
            {
                return 2;
            }

            x = moo() + foo();
            return x;
            """, ExpectedResult = 3)]
  [TestCase("""
            fun foo()
            {
                return 1;
            }

            fun moo()
            {
                return 2 + foo();
            }

            x = moo();
            return x;
            """, ExpectedResult = 3)]
  [TestCase("""
            fun foo()
            {
                return 1;
            }

            fun moo()
            {
                foo();
                return 2 + foo();
            }

            x = moo();
            
            return x;
            """, ExpectedResult = 3)]
  [TestCase("""
            y = 0;

            fun foo()
            {
              if (y < 3)
              {
                y = y + 1;
                foo();
              }
              return y;
            }

            x = foo();
            
            return x;
            """, ExpectedResult = 3)]
  [TestCase("""
            y = 0;

            fun foo()
            {
              y = y + 1;
              return y;
            }
            
            return foo();
            """, ExpectedResult = 1)]
  [TestCase("""
            fun foo(a)
            {
              return a;
            }
            
            return foo(3);
            """, ExpectedResult = 3)]
  [TestCase("""
            fun foo(a, b)
            {
              return a - b;
            }
            
            return foo(3, 2);
            """, ExpectedResult = 1)]
  // not working
  [TestCase("""
            z = 3;

            fun foo(a, b, c)
            {
              a = a * 10;
              return a + b + c + z;
            }
            
            return foo(100, 200, 300);
            """, ExpectedResult = 1503)]
  [TestCase("""
            z = 3;

            fun foo(a, b, c)
            {
              a = a * 10;
              z = z * 10;
              return a + b + c + z;
            }
            
            return foo(100, 200, 300);
            """, ExpectedResult = 1530)]
    public object? TestFunctions(string input)
    {
        //var p = new Parser(input);
        //var c = new Compiler(p.Parse());
        var c = new Compiler(input);
        return c.Compile();
    }

    [TestCase("""

            fun foo()
            {
              var c;
              c = 300;
              return c;
            }
            
            return foo();
            """, ExpectedResult = 300)]
  [TestCase("""

            fun foo()
            {
              var c, d, e;
              c = 300;
              d = 200;
              e = 1000;
              return c + d + e;
            }
            
            return foo();
            """, ExpectedResult = 1500)]
  [TestCase("""
            fun foo(a, b)
            {
              var c, d, e;
              c = 300;
              d = 200;
              e = 10;
              return (c - d) * e + a - b;
            }
            
            return foo(5, 2);
            """, ExpectedResult = 1003)]
      public object? TestLocalVars(string input)
    {
        return new Compiler(input).Compile();
    }
}