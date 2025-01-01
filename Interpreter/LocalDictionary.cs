namespace Interpreter
{
  public class LocalDictionary
  {
    public int paramCount = 0;
    public int localCount = 0;
    public int codeIndex = 0;

    internal Dictionary<string, LocalItem> Locals { get; set; } = [];

    public LocalDictionary(int codeIndex)
    {
      this.codeIndex = codeIndex;
      this.paramCount = 0;
      this.localCount = 0;
    }

    public void AddParam(string varName)
    {
      Locals.Add(varName, new LocalItem(isParam: true, paramCount++));
    }
    public void AddLocal(string varName)
    {
      var offset = -localCount;
      localCount++;
      Locals.Add(varName, new LocalItem(isParam: false, offset));
    }

    public void CalcParamOffset()
    {
      foreach (var param in Locals)
      {
        if (param.Value.isParam)
        {
          param.Value.offset = paramCount - param.Value.offset + 3; // 3 => address to return, funcName, bp
        }
      }
    }
  }
}
