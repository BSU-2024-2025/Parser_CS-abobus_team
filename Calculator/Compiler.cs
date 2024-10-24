using System;

namespace Calculator;

public static class Compiler
{

    private static readonly Stack<Operation> operations = new();
    private static readonly Stack<decimal> data = new();


    public static void PushNumber(decimal number)
    {
        data.Push(number);
    }


    public static void PushOperation(char symbol)
    {
        operations.Push(GetOperation(symbol));
    }

    public static void PushOperation(Operation op)
    {
        operations.Push(op);
    }

    public static Operation GetOperation(char symbol)
    {
        switch (symbol)
        {
            case (char)Operation.Add:
                return Operation.Add;
            case (char)Operation.Subtract:
                return Operation.Subtract;
            case (char)Operation.Multiply:
                return Operation.Multiply;
            case (char)Operation.Divide:
                return Operation.Divide;
            default:
                return Operation.End;
        }
    }

    public static decimal PopNumber()
    {
        return data.Pop();
    }


    public static Operation PopOperation()
    {
        return operations.Pop();
    }


    public static decimal GetResult()
    {
        return PopNumber();
    }


    public static void Execute(Operation op)
    {
        switch (op)
        {
            case Operation.Add:
                PushNumber(PopNumber() + PopNumber());
                break;
            case Operation.Subtract:
                PushNumber(-PopNumber() + PopNumber());
                break;
            case Operation.Multiply:
                PushNumber(PopNumber() * PopNumber());
                break;
            case Operation.Divide:
                PushNumber(1 / PopNumber() * PopNumber());
                break;
            case Operation.UnaryMinus:
                PushNumber(-PopNumber());
                break;
            default:
                break;

        }
    }

    public static int GetPriority(char op)
    {
        switch (op)
        {
            case (char)Operation.Add:
            case (char)Operation.Subtract:
                return 1;
            case (char)Operation.Multiply:
            case (char)Operation.Divide:
                return 3;
            case (char)Operation.LeftBracket:
                return 0;
            case (char)Operation.End:
                return 0;
            default:
                return 0;
        }
    }

    public static int GetPriority(Operation op)
    {
        switch (op)
        {
            case Operation.Add:
            case Operation.Subtract:
                return 1;
            case Operation.Multiply:
            case Operation.Divide:
            case Operation.UnaryMinus:
                return 3;
            case Operation.LeftBracket:
                return 0;
            case Operation.End:
                return 0;
            default:
                return 0;
        }
    }

    public static Operation PeekOperation()
    {
        return operations.Peek();
    }
    public static void ExecuteMany(char s)
    {
        int currentPriority = GetPriority(s);
        while (operations.Count > 0 && currentPriority < GetPriority(PeekOperation()))
        {
            Execute(PopOperation());
        }
    }

    public static void ExecuteParanthesis()
    {
        var a = PopOperation();
        while (a != Operation.LeftBracket)
        {
            Execute(a);
            a = PopOperation();
        }
    }
}