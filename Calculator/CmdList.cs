using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
  public class CmdList
  {
    public List<Cmd> cmds = new();

    public void AddOperation(int txtInd, Operation operation)
    {
      Cmd cmd = new Cmd();
      cmd.cmdType = operation;
      cmd.txtIndex = txtInd;
      cmds.Add(cmd);
    }

    public void AddConst (int txtInd, object num)
    {
      Cmd cmd = new Cmd();
      cmd.cmdType = Operation.Const;
      cmd.cmdData = num;
      cmd.txtIndex = txtInd;
      cmds.Add(cmd);
    }

    public void AddAssign(int txtInd, object varName)
    {
      Cmd cmd = new Cmd();
      cmd.cmdType = Operation.Assign;
      cmd.cmdData = varName;
      cmd.txtIndex = txtInd;
      cmds.Add(cmd);
    }

    public void AddEndExpr(int txtInd)
    {
      Cmd cmd = new Cmd();
      cmd.cmdType = Operation.EndExpr;
      cmd.txtIndex = txtInd;
      cmds.Add(cmd);
    }

    public void AddReturn(int txtInd)
    {
      Cmd cmd = new Cmd();
      cmd.cmdType = Operation.Return;
      cmd.txtIndex = txtInd;
      cmds.Add(cmd);
    }

    public void AddVar(int txtInd, object varName)
    {
      Cmd cmd = new Cmd();
      cmd.cmdType = Operation.Var;
      cmd.cmdData = varName;
      cmd.txtIndex = txtInd;
      cmds.Add(cmd);
    }

    public void Clear()
    {
      cmds.Clear();
    }
  }
}
