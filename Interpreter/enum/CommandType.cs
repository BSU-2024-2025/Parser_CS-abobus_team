namespace Interpreter;

public enum CommandType
{
    Operator,
    EndExpression,
    Constant,
    Assign,
    Variable,
    ConstVariable,
    Return,
    If,
    Else,
}