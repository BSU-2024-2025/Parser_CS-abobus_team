using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public LocalDictionary()
    {
      this.codeIndex = 0;
      this.paramCount = 0;
      this.localCount = 0;
    }

    public void AddLocal(string varName, bool isParam)
    {
      if (!isParam)
      {
        localCount++;
      }
      Locals.Add(varName, new LocalItem(isParam, paramCount++));
    }

    //public LocalItem GetOffset(string varName)
    //{
    //  var list = locals[varName];
    //  return list;
    //}
  }
}
