using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
  public class MyStack<T>
  {
    private List<T> items;
    private int count;
    const int n = 10;
    public MyStack()
    {
      items = new List<T>(n);
    }
    public MyStack(int length)
    {
      items = new List<T>(length);
    }

    public bool IsEmpty
    {
      get { return count == 0; }
    }
    public int Count
    {
      get { return count; }
    }

    public void Push(T item)
    {
      items.Add(item);
    }
    public T Pop()
    {
      if (IsEmpty)
        throw new InvalidOperationException("Стек пуст");
      T item = items[--count];
      items.RemoveAt(count);

      return item;
    }

    public T Peek()
    {
      return items[count - 1];
    }

    public T Peek(int index)
    {
      if (index >= items.Count || index < 0)
      {
        throw new InvalidOperationException("Invalid index");
      }
      return items[index];
    }
  }
}
