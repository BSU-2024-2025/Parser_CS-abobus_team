namespace Interpreter
{
  public class LocalItem
  {
    public string name;
    public int offset;
    public bool isParam;
    public object? defaultValue;

    public LocalItem(bool isParam, int offset, string name, object? defaultValue = null)
    {
      this.isParam = isParam;
      this.offset = offset;
      this.name = name;
      this.defaultValue = defaultValue;
    }
  }
}
