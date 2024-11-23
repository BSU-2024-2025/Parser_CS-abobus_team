namespace Interpreter;

public class Program
{
    public static void Main(string[] args)
    {
        var a = InitParser("""
                           x = 1 + 2;
                           return x + 1;
                           """);
        // var a = InitParser("x = 1 + 2;" +
        //                    "y = 1+x;" +
        //                    "return y +x;");
        // var a = InitParser("""
        //                         x = false || true;
        //                         return x;
        //                    """);
        var command = a.Parse();
        foreach (var c in command)
        {
            Console.WriteLine("type: " + c.CommandType);
            Console.WriteLine("value: " + c.Value);
        }
        var b = new Compiler(command);
        Console.WriteLine("result: " + b.Compile());
    }

    public static Parser InitParser(string input)
    {
        return new Parser(input);
    }
}