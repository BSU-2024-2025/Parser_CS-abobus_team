namespace Interpreter
{
  public class LocalItem
  {
    public bool isParam;
    public int offset;
    public string name;

    public LocalItem(bool isParam, int offset, string name)
    {
      this.isParam = isParam;
      this.offset = offset;
      this.name = name;
    }
  }
}
