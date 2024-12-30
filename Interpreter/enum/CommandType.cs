namespace Interpreter;

public enum CommandType
{
    Operator,
    EndExpression,
    Constant,
    Assign,
    //Variable,
    ConstVariable,
    Return,
    Return0,
    If,
    Function,
    Jump,
    LocalVariable,
    CallFunction,
    PopStack,
    SetLocal,
    SetGlobal,
    GetLocal,
    GetGlobal,
    SetArrayGlobal,
    SetArrayLocal,
    GetArrayGlobal
}