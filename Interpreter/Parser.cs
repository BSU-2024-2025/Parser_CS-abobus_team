using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;

namespace Interpreter;

public partial class Parser(string input)
{
    private int _currentIndex;
    private readonly CommandList _commandList = new();

    private char GetCurrentChar()
    {
        return input[_currentIndex];
    }

    public List<Command> Parse()
    {
        var result = ParseOperators();
        return _commandList.GetCommands();
    }

    public List<Command> Parse(out bool success)
    {
        success = ParseOperators();
        if (IsNotEnd())
        {
            success = false;
        }
        return _commandList.GetCommands();
    }
    
    

    private bool ParseOperators()
    {
        while (IsNotEnd())
        {
            if (ParseReturn()) return true;

            ParseAssign();
        }

        return !IsNotEnd();
    }

    private bool ParseAssign()
    {
        var name = ParseName();

        if (name == "") return false;

        if (!ParseStringLiteral("=")) return false;

        ParseExpression();
        _commandList.AddEndExpression(_currentIndex);

        if (!ParseStringLiteral(";")) throw new Exception("Unexpected end of expression");

        _commandList.AddVariable(_currentIndex, name);
        _commandList.AddAssign(_currentIndex,name);

        return true;
    }

    private string ParseName()
    {
        Skip();

        var prevIndex = _currentIndex;

        if (IsNotEnd() && (char.IsAsciiLetter(GetCurrentChar()) || GetCurrentChar() == '_'))
        {
            while (IsNotEnd() && (char.IsAsciiLetter(GetCurrentChar()) || GetCurrentChar() == '_')) _currentIndex++;

            return _currentIndex > prevIndex ? input[prevIndex.._currentIndex] : "";
        }

        return "";
    }

    private bool ParseReturn()
    {
        if (!ParseStringLiteral("return")) return false;

        if (ParseStringLiteral(";"))
        {
            _commandList.AddReturn(_currentIndex);
            return true;
        }

        ParseExpression();

        if (!ParseStringLiteral(";")) throw new Exception($"Unexpected end of input: {GetCurrentChar()}");
        _commandList.AddEndExpression(_currentIndex);
        _commandList.AddReturn(_currentIndex);

        return true;
    }

    private bool ParseExpression()
    {
        ParseUnary();
        if (!ParseOperand()) return false;

        while (ParseBinary())
            if (!ParseOperand())
                return false;
        return true;
    }

    private bool ParseBinary()
    {
        Skip();
        if (IsNotEnd() && GetBinaryOperator(out var op))
        {
            _commandList.AddOperator(_currentIndex, op);
            return true;
        }

        return false;
    }

    private bool ParseOperand()
    {
        if (ParseNum(out var num))
        {
            if (num != null) _commandList.AddConstant(_currentIndex, num);
            return true;
        }

        if (ParseString(out var str))
        {
            if (str != null) _commandList.AddConstant(_currentIndex, str);
            return true;
        }

        if (ParseBoolean(out var boolean))
        {
            if (boolean != null) _commandList.AddConstant(_currentIndex, boolean);
            return true;
        }

        if (ParseVariable())
            return true;

        if (ParseStringLiteral(Operator.LeftParenthesis))
        {
            _commandList.AddOperator(_currentIndex, Operator.LeftParenthesis);
            ParseExpression();
            if (ParseStringLiteral(Operator.RightParenthesis))
            {
                _commandList.AddOperator(_currentIndex, Operator.RightParenthesis);
                return true;
            }

            return false;
        }

        return false;
    }

    private bool ParseVariable()
    {
        var name = ParseName();
        if (name == "") return false;
        _commandList.AddConstVariable(_currentIndex, name);
        return true;
    }

    private bool ParseBoolean(out object? o)
    {
        Skip();
        var prevIndex = _currentIndex;
        o = null;

        if (IsNotEnd())
        {
            if (ParseStringLiteral("true"))
                o = true;
            else if (ParseStringLiteral("false"))
                o = false;
            else
                prevIndex = _currentIndex;
        }

        return _currentIndex > prevIndex;
    }

    private bool ParseString(out object? o)
    {
        Skip();
        var prevIndex = _currentIndex;
        if (IsNotEnd() && ParseStringLiteral("\""))
        {
            o = string.Empty;
        }
        else
        {
            o = null;
            return false;
        }

        while (IsNotEnd())
        {
            if (ParseStringLiteral("\""))
            {
                break;
            }

            o += GetCurrentChar().ToString();
            _currentIndex++;
        }

        return _currentIndex > prevIndex;
    }

    private bool ParseNum(out object? o)
    {
        Skip();
        var prevIndex = _currentIndex;
        while (IsNotEnd() && char.IsDigit(GetCurrentChar())) _currentIndex++;
        o = _currentIndex > prevIndex ? int.Parse(input[prevIndex.._currentIndex]) : null;
        return _currentIndex > prevIndex;
    }

    private void ParseUnary()
    {
        Skip();

        if (!IsNotEnd() || !Operator.IsUnaryOperator(GetCurrentChar().ToString(), out var op)) return;
        _commandList.AddOperator(_currentIndex, op);
        _currentIndex++;
    }

    private bool GetBinaryOperator(out string op)
    {
        Skip();
        
        if (IsNotEnd() && _currentIndex + 1 < input.Length &&
            Operator.IsBinaryOperator(input.Substring(_currentIndex, 2), out op))
        {
            _currentIndex += op.Length;
            return true;
        }

        op = Operator.Empty;
        return false;
    }


    private bool ParseStringLiteral(string literal)
    {
        Skip();

        if (_currentIndex + literal.Length <= input.Length &&
            input.Substring(_currentIndex, literal.Length).Equals(literal))
        {
            _currentIndex += literal.Length;
            return true;
        }

        return false;
    }

    private void Skip()
    {
        while (IsNotEnd() && IsSymbol()) _currentIndex++;

        if (_currentIndex < input.Length - 1
            && GetCurrentChar() == '/'
            && input.Substring(_currentIndex, 1).Equals("/")
           )
        {
            _currentIndex += 2;
            while (IsNotEnd() && GetCurrentChar() != '\n') _currentIndex++;

            if (IsNotEnd())
            {
                _currentIndex++;
                Skip();
            }
        }
        // while (true)
        // {
        //     while (IsNotEnd() && IsSymbol()) _currentIndex++;
        //
        //     if (_currentIndex < input.Length - 1 && input.Substring(_currentIndex, 2).Equals(@"//"))
        //     {
        //         _currentIndex += 2;
        //         while (IsNotEnd() && GetCurrentChar() != '\n') _currentIndex++;
        //         if (IsNotEnd())
        //         {
        //             _currentIndex++;
        //             continue;
        //         }
        //     }
        //
        //     break;
        // }
    }

    private bool IsSymbol()
    {
        return GetCurrentChar().ToString() switch
        {
            Symbol.Space or Symbol.Tab or Symbol.NewLine => true,
            _ => false
        };
    }

    private bool IsNotEnd()
    {
        return _currentIndex < input.Length;
    }
}