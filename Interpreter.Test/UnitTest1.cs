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
              """, ExpectedResult = 0)]
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
              """, ExpectedResult = 0)]
  [TestCase("""
               fun foo()
               {
               }
               return;
              """, ExpectedResult = 0)]
  [TestCase("""
            fun f()
            {
              return;
            }
            return f();
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
             x = 2;
             fun foo()
             {
                 x = 1;
                 return x;
             }
             return foo();
            """, ExpectedResult = 1)]
  [TestCase("""
             x = 111;
             y = 2222;
            
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
            x = 2;
            
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
  [TestCase("""
            fun f1(a)
            {
              return a + 2;
            }
            
            fun f2(n)
            {
              return f1(n + 1);
            }
            
            return f2(5);
            """, ExpectedResult = 8)]
  [TestCase("""
            fun sub(a,b)
            {
              return a - b;
            }
            
            fun div(a,b)
            {
              return a / b ;
            }

            return sub(5 * div(6,3) + sub(70,2), div( 4 + sub(9,1)*2, 10/2 ) ); 

            """, ExpectedResult = 74)]
  // sub(     10     +     68,     div( 4     + 16,      5) ) = 78 - 20/5 = 74
  public object? TestFunctions(string input)
    {
      return new Compiler(input).Compile();
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
  [TestCase("""
            fun factorial(n)
            {
              if (n <= 1) {
                return 1;
              }
              var x;
              x = n;
              return x * factorial(n - 1);
            }
            
            return factorial(5);
            """, ExpectedResult = 120)]
  [TestCase("""
            x = 100;
            fun factorial(n)
            {
              if (n <= 1) {
                return 1;
              }
              var x;
              x = n;
              return x * factorial(n - 1);
            }
            
            return x * factorial(5);
            """, ExpectedResult = 12000)]
  [TestCase("""
            x = 100;
            fun f()
            {
              var x = 200;
              return x;
            }
            
            return f();
            """, ExpectedResult = 200)]
  [TestCase("""
            x = 100;
            fun f()
            {
              var x = 200, y =300;
              return x + y;
            }
            
            return f();
            """, ExpectedResult = 500)]
  [TestCase("""
            x = 100;
            fun f()
            {
              var x = x + 10, y = x + 1;
              return y;
            }
            
            return f();
            """, ExpectedResult = 111)]
  [TestCase("""
            x = 100;
            fun f()
            {
              var x = x + 10, y = x + 1;
              var z = y * 100;
              z = z + 11;
              return z;
            }
            
            return f();
            """, ExpectedResult = 11111)]
  public object? TestLocalVars(string input)
  {
    return new Compiler(input).Compile();
  }

  [TestCase("""
            x = 5;
            y = 2.0;
            
            return x/y;
            """, ExpectedResult = 2.5)]
  [TestCase("""
            x = 7;
            y = 3.5;
            
            return x/y;
            """, ExpectedResult = 2.0)]
  [TestCase("""
            return "a b";
            """, ExpectedResult = "a b")]
  [TestCase("""
            x = 7e10;
            y = 3.5e10;
            
            return x/y;
            """, ExpectedResult = 2.0)]
  [TestCase("""
            x = 7e-10;
            y = 3.5e-10;
            
            return x/y;
            """, ExpectedResult = 2.0)]
  [TestCase("""
            x = 7e-10;
            y = 3.5e1;
            
            return x/y;
            """, ExpectedResult = 2.0e-11)]
  [TestCase("""
            x = 2.5e-10;
            y = 2.5e+10;
            
            return x * y;
            """, ExpectedResult = 6.25)]
  [TestCase("""
            x = 2.567e1;
            y = 0.066e+1;
            
            return x - y;
            """, ExpectedResult = 2.501e1)]
  [TestCase("""
            x = -2.567e1;
            y =  0.500e1;
            
            return -x + (-y);
            """, ExpectedResult = 2.067e1)]
  [TestCase("""
            x = -2.5;
            y =  0.5;
            
            if (x < y) {
              return 1;
            }
            """, ExpectedResult = 1)]
  [TestCase("""
            if (-3.3 > (-5)) {
              return 1;
            }
            """, ExpectedResult = 1)]
  [DefaultFloatingPointTolerance(1e-12)]
  public object? TestFloat(string input)
  {
    return new Compiler(input).Compile();
  }

  [TestCase("""
            if ("qwe" + 5555 > "qwe") {
              return 1;
            }
            """, ExpectedResult = 1)]
  [TestCase("""
            if ("qwe" + "5555" > "qwe") {
              return 1;
            }
            """, ExpectedResult = 1)]
  [TestCase("""
            if ("qwe" == "qwe") {
              return 1;
            }
            """, ExpectedResult = 1)]
  [TestCase("""
            if ("qwe2" > "qwe") {
              return 1;
            }
            """, ExpectedResult = 1)]
  [TestCase("""
            if ("qwe" < "qwe2") {
              return 1;
            }
            """, ExpectedResult = 1)]
  [TestCase("""
            if ("qwe" <= "qwe2") {
              return 1;
            }
            """, ExpectedResult = 1)]
  [TestCase("""
            if ("qwe3" >= "qwe2") {
              return 1;
            }
            """, ExpectedResult = 1)]
  [TestCase("""
            if (!("qwe" >= "qwe2")) {
              return 1;
            }
            """, ExpectedResult = 1)]
  [TestCase("""
            x = "qw";
            x = x + "e";
            if ("qwe" == x) {
              return 1;
            }
            """, ExpectedResult = 1)]
  [TestCase("""
            x = "qwe";
            y = x + "1";
            if (x < y) {
              return 1;
            }
            """, ExpectedResult = 1)]
  public object? TestStringCompare(string input)
  {
    return new Compiler(input).Compile();
  }

  [TestCase("""
            fun f()
            {
              x = 1;
              return x;
            }
            
            return f();
            """)]
  public void TestCreateGlobalVarInFuncException(string input)
  {
    Assert.That(() => new Compiler(input).Compile(), Throws.Exception);
  }

  [TestCase("""
            if (-3.3 && (-5)) {
              return 1;
            }
            """)]
  [TestCase("""
              return "qwe" * 5;
            """)]
  [TestCase("""
              return "qwe" - 5;
            """)]
  [DefaultFloatingPointTolerance(1e-12)]
  public void TestRuntimeBinderException(string input) 
  {
    Assert.That(() => new Compiler(input).Compile(), Throws.Exception); 
  }

  [TestCase("""
                x = [1, 2, 3];
                return x[0];
                //delete(x,1);
                //push(x,1);
            """, ExpectedResult = 1)]
  [TestCase("""
                x = [1, 2, 3];
                return x[0] / 1 + x[1] * 1 + x[2];
            """, ExpectedResult = 6)]
  [TestCase("""
                x = [1, 2, 3];
                return x[0*1 + 0*2] / 1 + x[(1 * 1)*1] * 1 + x[2/2 + 1/1];
            """, ExpectedResult = 6)]
  [TestCase("""
                x = [1, 2, 3];
                x[2] = 111;
                return x[2];
            """, ExpectedResult = 111)]
  [TestCase("""
                x = [1, 2, 3];
                x[2] = x[1];
                return x[2];
            """, ExpectedResult = 2)]
  [TestCase("""
                x = [1, 2, 3];
                x[2] = x[2]-x[0];
                return x[2];
            """, ExpectedResult = 2)]
  [TestCase("""
                x = [1, 2, 3];
                x[1+1] = x[0*1 + 0*2] / 1 + x[(1 * 1)*1] * 1 + x[2/2 + 1/1];
                return x[2];
            """, ExpectedResult = 6)]
  [TestCase("""
            x = 1;
            fun f()
            {
              var x = 200, y =300;
              return x + y;
            }
            x = [1, 2, f()];
            return x[2];
            """, ExpectedResult = 500)]
  [TestCase("""
            x = 1;
            fun f()
            {
              var y =300;
              return x + y;
            }
            x = [1, 2, f()];
            return x[2];
            """, ExpectedResult = 301)]
  public object? TestArray(string input)
  {
    return new Compiler(input).Compile();
  }
}