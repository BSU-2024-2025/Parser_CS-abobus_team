using System.Collections.Specialized;

namespace Interpreter;


public class Compiler(string input)
{
	private string input = input;
	private List<Command> commands = new();
	private readonly MyStack<string> operations = new();
	private readonly MyStack<object?> data = new();
	//private readonly Dictionary<string, object?> variables = new();
  //private readonly Dictionary<string, LocalDictionary> functions = new();
  private int nestLevel = 0;
	private LocalDictionary? context = null;
	private Parser parser = null;


	//public Compiler(List<Command> commands)
	//{
	//	this.commands = commands;
	//}
	//
	//public Compiler(string input)
	//{
  //  var p = new Parser(input);
  //  this.commands = p.Parse();
  //}

  public object? Compile()
	{
		parser = new Parser(input);
		commands.Clear();
		commands = parser.Parse();
		int bp = 0;
		string? funcName = null;
		for (var i = 0; i < commands.Count; i++)
		{
			var command = commands[i];
			switch (command.CommandType)
			{
				case CommandType.Constant:
				{
					PushData(command.Value!);
					break;
				}
				case CommandType.ConstVariable:
				{
					if (HasVariable((string)command.Value!))
					{
						var variable = GetVariable((string)command.Value!);
						if (variable != null)
						{
							PushData(variable);
						}
					}
					else
					{
						throw new Exception($"Variable '{command.Value}' is not defined.");
					}
					break;
				}
				case CommandType.Operator:
				{
					if (command.Value != null)
					{
						switch ((string)command.Value)
						{
							case "(":
								PushOperator("(");
								break;
							case ")":
								ExecuteParenthesis();
								break;
							default:
								ExecuteOperators((string)command.Value);
								PushOperator((string)command.Value);
								break;
						}
					}

					break;
				}
				case CommandType.Variable:
					AddVariable((string)command.Value!);
					break;
				case CommandType.Assign:
					SetVariable((string)command.Value!, PopData());
					break;
				case CommandType.Return:
					if (nestLevel == 0 && GetOperatorsLength() != 0)
					{
						throw new Exception($"Not empty operators stack.");
					}

					if (nestLevel == 0)
          {
            return (GetDataLength() != 0 ? PeekData() : 0) ?? 0;
          }
					else
					{
						i = ReturnFunc(funcName!, ref bp);
					}
					break;
				case CommandType.EndExpression:
					ExecuteOperators(Operator.End);
					break;
				case CommandType.If:
					if (!(bool)PopData()!)
					{
						i = (int)command.Value! - 1; // increment in for
					}
					break;
				case CommandType.Jump:
					i = (int)command.Value! - 1; // increment in for
          break;
				case CommandType.CallFunction:
          data.Push(i);
          data.Push(funcName);
          data.Push(bp);
          i = CallFunc((string)command.Value!);
					break;
			}
		}

		return null;
	}

  private int ReturnFunc(string funcName, ref int bp)
  {
		object? result = null;
		bool isResult = false;
    var func = parser.functions[funcName];

    if (data.Count > bp)
    {
      result = data.Pop();
			isResult = true;
    }

		for (int i = 0; i < func.localCount; i++)
		{
			data.Pop();
		}
		bp = (int)data.Pop()!;
    int curIndex = (int)data.Pop()!;

    if (isResult)
		{
      data.Push(result);
    }

		return curIndex;
  }

  private int CallFunc(string funcName)
  {
    var func = parser.functions[funcName];
  
    for (int i = 0; i < func.localCount; i++)
  	{
  		data.Push(null);
    }
  
  	return func.codeIndex;
  }

  private void Execute(string operation)
	{
		switch (operation)
		{
			case Operator.Add:
			{
				var operand2 = PopData()!;
				var operand1 = PopData()!;
				if (operand1 is int && operand2 is int)
				{
					PushData((int?)operand1 + (int?)operand2);
					break;
				}
				if (operand1 is float || 
				    operand2 is float || 
				    operand1 is double || 
				    operand2 is double || 
				    operand1 is decimal || 
				    operand2 is decimal || 
				    operand1 is int || 
				    operand2 is int
				    )	
				{
					PushData((double?)operand1 + (double?)operand2);
					break;
				}
				
				PushData((string?)operand1 + (string?)operand2);
				break;
			}
			case Operator.Subtract:
			{
				PushData(-(int?)PopData()! + (int?)PopData()!);
				break;
			}
			case Operator.Multiply:
			{
				PushData((int?)PopData()! * (int?)PopData()!);
				break;
			}
			case Operator.Divide:
				PushData(1/(int?)PopData()! * (int?)PopData()!);
				break;
			case Operator.UnaryMinus:
				PushData(-(int?)PopData()!);
				break;
			case Operator.Not:
			{
				var operand = PopData();
				if (operand is bool)
				{
					var res = !(bool)operand;
					PushData(res);
					break;
				}
				throw new InvalidOperationException("Not supported");
			}
			case Operator.MoreThan:
			{
				var operand = PopData();
				var operand2 = PopData();
				PushData((int?)operand < (int?)operand2);
				break;
			}
			case Operator.LessThan:
			{
				var operand1 = PopData();
				var operand2 = PopData();
				PushData((int?)operand1 > (int?)operand2);
				break;
			}
			case Operator.GreaterThanOrEqual:
			{
				var operand1 = PopData();
				var operand2 = PopData();
				PushData((int?)operand1 <= (int?)operand2);
				break;
			}
			case Operator.LessThanOrEqual:
			{
				var operand1 = PopData();
				var operand2 = PopData();
				PushData((int?)operand1 >= (int?)operand2);
				break;
			}
			case Operator.Equal:
			{
				var operand1 = PopData()!;
				var operand2 = PopData();
				var res = operand1.Equals(operand2);
				PushData(res);
				break;
			}
			case Operator.And:
			{
				var operand1 = PopData();
				var operand2 = PopData();
				if (operand1 is bool && operand2 is bool)
				{
					PushData((bool)operand2 && (bool)operand1);
				}
				break;
			}
			case Operator.NotEqual:
			{
				var operand1 = PopData()!;
				var operand2 = PopData();
				var res = !(operand1.Equals(operand2));
				PushData(res);
				break;
			}
			case Operator.Or:
			{
				var operand1 = PopData();
				var operand2 = PopData();
				if (operand1 is bool && operand2 is bool)
				{
					PushData((bool)operand2 || (bool)operand1);
				}
				break;
			}
		}
	}

	private void ExecuteParenthesis()
	{
		var cur = PopOperator();
		while (!cur.Equals(Operator.LeftParenthesis))
		{
			Execute(cur);
			cur = PopOperator();
		}
		
	}

	private void ExecuteOperators(string operation)
	{
		var curPriority = GetPriority(operation);
		while (GetOperatorsLength() > 0 && curPriority < GetPriority(PeekOperator()))
		{
			Execute(PopOperator());
		}
	}

	private static int GetPriority(string operation)
	{
		return operation switch
		{
			Operator.Not or Operator.UnaryMinus or Operator.UnaryPlus => 400,
			Operator.Multiply or Operator.Divide => 300,
			Operator.Add or Operator.Subtract => 100,
			Operator.LessThan or Operator.LessThanOrEqual or Operator.Equal or Operator.NotEqual
				or Operator.GreaterThanOrEqual or Operator.MoreThan => 50,
			Operator.And => 40,
			Operator.Or => 20,
			Operator.LeftParenthesis => 0,
			Operator.End => 0,
			_ => 0
		};
	}

	private object? PeekData()
	{
		return data.Peek();
	}
	private object? PopData()
	{
		return data.Pop();
	}
	private void PushData(object data)
	{
		this.data.Push(data);
	}

	private int GetDataLength()
	{
		return data.Count;
	}

	private int GetOperatorsLength()
	{
		return operations.Count;
	}
	
	private void PushOperator(string data)
	{
		operations.Push(data);
	}

	private string PopOperator()
	{
		return operations.Pop();
	}

	private string PeekOperator()
	{
		return operations.Peek();
	}

	private bool HasVariable(string variable)
	{
		return parser.variables.ContainsKey(variable);
	}

	private object? GetVariable(string name)
	{
		if (!HasVariable(name))
		{
			return null;
		}
		
		return parser.variables[name];
	}
	
	private void AddVariable(string name)
	{
		if (!HasVariable(name))
		{
      parser.variables.Add(name, null);
		}
	}
	
	private void SetVariable(string name, object? value)
	{
		if (HasVariable(name))
		{
      parser.variables[name] = value;
		}
		else
		{
      parser.variables.Add(name, value);
		}
	}
}