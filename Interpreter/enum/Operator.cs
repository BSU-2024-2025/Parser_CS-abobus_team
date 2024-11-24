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

    public static bool IsBinaryOperator(string op, out string str)
    {
        switch (op[0].ToString())
        {
            case Add or Subtract or Multiply or Divide or Modulo:
                str = op[0].ToString();
                return true;
            case MoreThan or LessThan when op[1].ToString().Equals("="):
                str = op;
                return true;
            case MoreThan or LessThan when !op[1].ToString().Equals("="):
                str = op[0].ToString();
                return true;
        }
        switch (op)
        {
            case Equal or NotEqual or And or Or:
                str = op;
                return true;
        }
        str = Empty;
        return false;
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