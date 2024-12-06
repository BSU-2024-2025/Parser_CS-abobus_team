namespace Interpreter;

public class MyStack<T>
{
    private readonly List<T> items = new();

    private bool IsEmpty => items.Count == 0;

    public int Count => items.Count;

    public void Push(T item)
    {
        items.Add(item);
    }

    public T Pop()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Стек пуст");
        var item = items[^1];
        items.RemoveAt(items.Count - 1);
        return item;
    }

    public T Peek()
    {
        return items[^1];
    }

    public T Peek(int index)
    {
        if (IsEmpty || index >= items.Count || index < 0)
        {
            throw new InvalidOperationException("Invalid index");
        }

        return items[^index];
    }
}