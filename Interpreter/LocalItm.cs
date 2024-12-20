using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
  internal class LocalItem
  {
    public bool isParam;
    public int offset;

    public LocalItem(bool isParam, int offset)
    {
      this.isParam = isParam;
      this.offset = offset;
    }
  }
}
