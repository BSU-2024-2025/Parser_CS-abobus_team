namespace Interpreter
{
  public class LocalItem
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
