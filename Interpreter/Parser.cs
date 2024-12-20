namespace Interpreter;

public class Parser(string input)
{
    private int currentIndex;
    private readonly CommandList commandList = new();

    private char GetCurrentChar()
    {
        return input[currentIndex];
    }

    public List<Command> Parse()
    {
        var result = ParseOperators();
        return commandList.GetCommands();
    }

    public List<Command> Parse(out bool success)
    {
        success = ParseOperators();
        if (IsNotEnd())
        {
            success = false;
        }

        return commandList.GetCommands();
    }

    private bool ParseFunction()
    {
        if (!ParseStringLiteral("fun")) return false;
        var name = ParseName();
        if (name == "") throw new Exception("Invalid function name");
        commandList.AddFunction(currentIndex, name);
        if (!ParseStringLiteral("(")) throw new Exception("No open parentheses");
        var param = ParseName();
        while (!string.IsNullOrEmpty(param))
        {
            commandList.AddLocalVariable(currentIndex, param);
            if (ParseStringLiteral(","))
            {
                param = ParseName();
                continue;
            }
            break;
        }
        if (!ParseStringLiteral(")")) throw new Exception("no close parentheses");
        ParseBlock();
        return true;
    }

    private bool ParseOperators()
    {
        while (IsNotEnd())
        {
            if (ParseReturn())
            {
                continue;
            }

            if (ParseFunction())
            {
                continue;
            }

            if (ParseIf())
            {
                continue;
            }

            if (ParseWhile())
            {
                continue;
            }

            if (ParseAssign())
            {
                continue;
            }

            return false;
        }

        return !IsNotEnd();
    }

    private bool ParseAssign()
    {
        var name = ParseName();

        if (name == "") return false;

        if (!ParseStringLiteral("=")) return false;

        ParseExpression();
        commandList.AddEndExpression(currentIndex);

        if (!ParseStringLiteral(";")) throw new Exception("Unexpected end of expression");

        commandList.AddVariable(currentIndex, name);
        commandList.AddAssign(currentIndex, name);

        return true;
    }

    private string ParseName()
    {
        Skip();

        var prevIndex = currentIndex;

        if (IsNotEnd() && (char.IsAsciiLetter(GetCurrentChar()) || GetCurrentChar() == '_'))
        {
            while (IsNotEnd() && (char.IsAsciiLetter(GetCurrentChar()) || GetCurrentChar() == '_')) currentIndex++;

            return currentIndex > prevIndex ? input[prevIndex..currentIndex] : "";
        }

        return "";
    }

    private bool ParseReturn()
    {
        if (!ParseStringLiteral("return")) return false;

        if (ParseStringLiteral(";"))
        {
            commandList.AddReturn(currentIndex);
            return true;
        }

        ParseExpression();

        if (!ParseStringLiteral(";")) throw new Exception($"Unexpected end of input: {GetCurrentChar()}");
        commandList.AddEndExpression(currentIndex);
        commandList.AddReturn(currentIndex);

        return true;
    }


    private bool ParseIf()
    {
        if (!ParseStringLiteral("if")) return false;

        ParseExpression();
        commandList.AddEndExpression(currentIndex);
        commandList.AddIf(currentIndex, out var command1);

        ParseBlock();
        command1.Value = commandList.GetCommandCount();

        if (ParseStringLiteral("else"))
        {
            commandList.AddJump(currentIndex, out var command2);
            command1.Value = commandList.GetCommandCount();
            if (!ParseIf())
            {
                ParseBlock();
            }

            command2.Value = commandList.GetCommandCount();
        }

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
            commandList.AddOperator(currentIndex, op);
            return true;
        }

        return false;
    }

    private bool ParseOperand()
    {
        if (ParseNum(out var num))
        {
            if (num != null) commandList.AddConstant(currentIndex, num);
            return true;
        }

        if (ParseString(out var str))
        {
            if (str != null) commandList.AddConstant(currentIndex, str);
            return true;
        }

        if (ParseBoolean(out var boolean))
        {
            if (boolean != null) commandList.AddConstant(currentIndex, boolean);
            return true;
        }

        if (ParseVariableOrFunctionCall())
            return true;

        if (ParseStringLiteral(Operator.LeftParenthesis))
        {
            commandList.AddOperator(currentIndex, Operator.LeftParenthesis);
            ParseExpression();
            if (ParseStringLiteral(Operator.RightParenthesis))
            {
                commandList.AddOperator(currentIndex, Operator.RightParenthesis);
                return true;
            }

            return false;
        }

        return false;
    }

  private bool ParseFunctionCall(string name)
  {
    commandList.AddCallFunction(currentIndex, name!);
    while (!ParseStringLiteral(")"))
    {
      ParseExpression();
      if (ParseStringLiteral(")")) break;
      if (ParseStringLiteral(","))
      {
        continue;
      }
      else
      {
        throw new Exception("invalid function call");
      }
    }
    return true;
  }

  private bool ParseVariableOrFunctionCall()
    {
        var name = ParseName();
        if (name == "") return false;
        if (ParseStringLiteral("("))
        {
          ParseFunctionCall(name);
          commandList.AddCallFunction(currentIndex, name!);
        }
        else
        {
          commandList.AddConstVariable(currentIndex, name);
        }
        return true;
    }

    private bool ParseBoolean(out object? o)
    {
        Skip();
        var prevIndex = currentIndex;
        o = null;

        if (IsNotEnd())
        {
            if (ParseStringLiteral("true"))
                o = true;
            else if (ParseStringLiteral("false"))
                o = false;
            else
                prevIndex = currentIndex;
        }

        return currentIndex > prevIndex;
    }

    private bool ParseString(out object? o)
    {
        Skip();
        var prevIndex = currentIndex;
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
            currentIndex++;
        }

        return currentIndex > prevIndex;
    }

    private bool ParseNum(out object? o)
    {
        Skip();
        var prevIndex = currentIndex;
        while (IsNotEnd() && char.IsDigit(GetCurrentChar())) currentIndex++;
        o = currentIndex > prevIndex ? int.Parse(input[prevIndex..currentIndex]) : null;
        return currentIndex > prevIndex;
    }

    private void ParseUnary()
    {
        Skip();

        if (!IsNotEnd() || !Operator.IsUnaryOperator(GetCurrentChar().ToString(), out var op)) return;
        commandList.AddOperator(currentIndex, op);
        currentIndex++;
    }

    private bool GetBinaryOperator(out string op)
    {
        Skip();

        if (IsNotEnd() && currentIndex + 1 < input.Length &&
            Operator.IsBinaryOperator(input.Substring(currentIndex, 2), out op))
        {
            currentIndex += op.Length;
            return true;
        }

        op = Operator.Empty;
        return false;
    }


    private bool ParseStringLiteral(string literal)
    {
        Skip();

        if (currentIndex + literal.Length <= input.Length &&
            input.Substring(currentIndex, literal.Length).Equals(literal))
        {
            currentIndex += literal.Length;
            return true;
        }

        return false;
    }

    private bool ParseWhile()
    {
        if (!ParseStringLiteral("while")) return false;
        var i = commandList.GetCommandCount();
        ParseExpression();
        commandList.AddEndExpression(currentIndex);
        commandList.AddIf(currentIndex, out var command1);

        ParseBlock();

        commandList.AddJump(currentIndex, out var command2);
        command1.Value = commandList.GetCommandCount();
        command2.Value = i;
        return true;
    }

    private void ParseBlock()
    {
        if (!ParseStringLiteral("{")) throw new Exception($"Expected operator's block");
        ParseOperators();
        if (!ParseStringLiteral("}")) throw new Exception($"Unexpected end of block");
    }

    private void Skip()
    {
        while (IsNotEnd() && IsSymbol()) currentIndex++;

        if (currentIndex < input.Length - 1
            && GetCurrentChar() == '/'
            && input.Substring(currentIndex, 1).Equals("/")
           )
        {
            currentIndex += 2;
            while (IsNotEnd() && GetCurrentChar() != '\n'
                              && GetCurrentChar() != '\r'
                              && GetCurrentChar() != '\t') currentIndex++;

            if (IsNotEnd())
            {
                currentIndex++;
                Skip();
            }
        }
    }

    private bool IsSymbol()
    {
        return GetCurrentChar().ToString() switch
        {
            Symbol.Space or Symbol.Tab or Symbol.NewLine or Symbol.LineFeed => true,
            _ => false
        };
    }

    private bool IsNotEnd()
    {
        return currentIndex < input.Length;
    }
}