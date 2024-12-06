namespace Interpreter;

public class Program
{
    public static void Main(string[] args)
    {
    // var a = InitParser("""
    //                    return (10 > 3) || (9 < 1);
    //                 """);
    // var a = InitParser("""
    //                       if (1 > 2){
    //                         if (1 == 2){
    //                             x = 3;
    //                         }else{
    //                             x = 4;
    //                         }
    //                       }else{
    //                         if (1 != 2){
    //                             x = 6;
    //                         }
    //                         else{
    //                             x = 7;
    //                          }
    //                       }
    //                    """);
    //var a = InitParser("""
    //                       if (3 != 3){
    //                        return 1;
    //                       }else {
    //                        return 2;
    //                       }
    //                       return 6;
    //                       """);
    //var a = InitParser("""
    //                       if 3 == 3 {
    //                        return 1;
    //                       }
    //                       return 6;
    //                       """);
    //var a = InitParser("""
    //                       y = 1;
    //                       if (y == 1){
    //                        x = 3;
    //                       }else {
    //                        x = 4;
    //                       }
    //                       return x;
    //                       """);
    //var a = InitParser("""
    //                   y = 1;
    //                   x = 0;
    //                   if (y == 2){
    //                    x = 3;
    //                   }else if (y == 3){
    //                    x = 4;
    //                   }else{
    //                   x = 5;
    //                   }
    //                   z = 1;
    //                   
    //                   return x;
    //                   """);
    // var a = InitParser("""
    //                        if (2 == 2){
    //                         return 3;
    //                        }else if (4 == 4){
    //                         return 6;
    //                        }else{
    //                         return 7;
    //                        }
    //                        return 8;
    //                        """);
    var a = InitParser("""
                        fun name()
                        {
                            return "Hello World!";
                        }
                       """);
    //var a = InitParser("""
    //                       if (1 <= 2){
    //                        return 3;
    //                       }else if (3 == 4){
    //                        return 6;
    //                       }
    //                       return 8;
    //                       """);
    //var a = InitParser("""
    //                       if (1 >= 2){
    //                        return 3;
    //                       }else if (3 <= 4){
    //                        if (!true) {
    //                          return 5;
    //                        } else {
    //                          return 6;
    //                        }
    //                       }
    //                       return 8;
    //                       """);
    // var a = InitParser("""
    //                    y = 4;
    //                    x = 0;
    //                    if (y == 2){
    //                     x = 3;
    //                    }else if(y == 1){
    //                    x = 5;
    //                    }else {
    //                    x = 4;
    //                    }
    //                    return x;
    //                    """);
    // var a = InitParser("x = 1 + 2;" +
    //                    "y = 1+x;" +
    //                    "return y +x;");
    // var a = InitParser("""
    //                         x = false || true;
    //                         return x;
    //                    """);
    var command = a.Parse();
        for (var i = 0; i < command.Count; i++)
        {
            var c = command[i];
            Console.WriteLine(i + ":");
            Console.WriteLine("type: " + c.CommandType);
            Console.WriteLine("value: " + c.Value);
            Console.WriteLine("------------------");
        }

        //var b = new Compiler(command);
        //Console.WriteLine("result: " + b.Compile());
    }

    public static Parser InitParser(string input)
    {
        return new Parser(input);
    }
}