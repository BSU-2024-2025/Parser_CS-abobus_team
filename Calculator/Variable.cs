namespace Calculator;

public static class Variable
{

    private static readonly Dictionary<string, object?> variables = new();

    public static void SetVariable(string name, object? value)
    {
        if (variables.ContainsKey(name))
        {
            variables[name] = value;
        }
        else
        {
            variables.Add(name, value);
        }
    }

    public static void AddVariable(string name)
    {
        if (!variables.ContainsKey(name))
        {
            variables.Add(name, null);
        }
    }

    public static object? GetVariable(string name)
    {
        if (!variables.ContainsKey(name))
        {
            return null;
        }
        return variables[name];
    }


    public static bool HasVariable(string name)
    {
        return variables.ContainsKey(name);
    }
    public static void ClearVariables() 
    { 
        variables.Clear(); 
    }
}
