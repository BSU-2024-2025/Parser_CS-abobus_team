namespace Interpreter;

public static class Operator
{
    public const string Add = "+";
    public const string Subtract = "-";
    public const string Multiply = "*";
    public const string Divide = "/";
    public const string Modulo = "%";
    public const string And = "&&";
    public const string Or = "||";
    public const string Not = "!";
    public const string MoreThan = ">";
    public const string LessThan = "<";
    public const string GreaterThanOrEqual = ">=";
    public const string LessThanOrEqual = "<=";
    public const string UnaryMinus = "_";
    public const string UnaryPlus = "++";
    public const string Equal = "==";
    public const string NotEqual = "!=";
    public const string Empty = "";
    public const string LeftParenthesis = "(";
    public const string RightParenthesis = ")";
    public const string End = "\n";

    public static bool IsBinaryOperatorOneChar(string op, out string str)
    {
        switch (op)
        {
            //or MoreThan or LessThan
            // or GreaterThanOrEqual or LessThanOrEqual
            // TODO MAKE THIS
            case Add or Subtract or Multiply or Divide or Modulo:
                str = op;
                return true;
            default:
                str = Empty;
                return false;
        }
    }
    
    public static bool IsBinaryOperatorManyChar(string op, out string str)
    {
        switch (op)
        {
            //or MoreThan or LessThan
            // or GreaterThanOrEqual or LessThanOrEqual
            // TODO MAKE THIS
            case Equal or NotEqual or Add or Or:
                str = op;
                return true;
            default:
                str = Empty;
                return false;
        }
    } 

    public static bool IsUnaryOperator(string op, out string str)
    {
        switch (op)
        {
            case "-":
                str = UnaryMinus;
                return true;
            case "+":
                str = UnaryPlus;
                return true;
            case  Not:
                str = op;
                return true;
            default:
                str = Empty;
                return false;
        }
    }
}