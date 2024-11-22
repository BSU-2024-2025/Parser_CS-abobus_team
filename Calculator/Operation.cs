namespace Calculator;

public enum Operation
{
    Add = '+',
    Subtract = '-',
    Multiply = '*',
    Divide = '/',
    LeftBracket = '(',
    RightBracket = ')',
    UnaryMinus = '_',
    End = '\0',
    Negation = '!',
    IsEqual = '=',
    MoreThan = '>',
    MoreOrEqual,
    LessThan = '<',
    LessOrEqual = 50000,
    LogicalAnd = 50001,
    LogicalOr = 50002,

    Const = 50003,
    Assign = 50004,
    Var = 50005,
    EndExpr = 50006,
    Return = 50007
}

