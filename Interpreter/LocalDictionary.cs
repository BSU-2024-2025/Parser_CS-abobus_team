using System.Collections;
using System.Linq;

namespace Interpreter
{
  public class LocalDictionary
  {
    public int paramCount = 0;
    public int localCount = 0;
    public int codeIndex = 0;

    //internal Dictionary<string, LocalItem> Locals { get; set; } = [];
    internal IList<LocalItem> Locals { get; set; } = [];

    public LocalDictionary(int codeIndex)
    {
      this.codeIndex = codeIndex;
      this.paramCount = 0;
      this.localCount = 0;
    }

    public bool TryGetValue(string key, out LocalItem? item) 
    {
      item = Locals.FirstOrDefault((x) => (x.name == key));
      return (item == null);
    }

    public void AddParam(string varName)
    {
      Locals.Add(new LocalItem(isParam: true, paramCount++, varName));
      //Locals.Add(varName, new LocalItem(isParam: true, paramCount++, varName));
    }
    public void AddLocal(string varName)
    {
      var offset = -localCount;
      localCount++;
      Locals.Add(new LocalItem(isParam: false, offset, varName));
      //Locals.Add(varName, new LocalItem(isParam: false, offset));
    }

    public void CalcParamOffset()
    {
      foreach (LocalItem param in Locals)
      {
        if (param.isParam)
        {
          param.offset = paramCount - param.offset + 4; // 4 => argc, address to return, funcName, bp
        }
      }
        //foreach (var (param as LocalItem) in Locals)
        //{
        //  if (param.isParam)
        //  {
        //    param.Value.offset = paramCount - param.Value.offset + 3; // 3 => address to return, funcName, bp
        //  }
        //}
      }
    }
}
