using System.Collections.Generic;

public class MinHeap<T>
{
    private readonly List<(T item, int priority)> _data = new List<(T, int)>();

    public int Count => _data.Count;
    public bool Empty => _data.Count == 0;

    public void Push(T item, int priority)
    {
        _data.Add((item, priority));
        SiftUp(_data.Count - 1);
    }

    public T Pop()
    {
        var root = _data[0].item;

        int last = _data.Count - 1;
        _data[0] = _data[last];
        _data.RemoveAt(last);

        if (_data.Count > 0)
            SiftDown(0);

        return root;
    }

    private void SiftUp(int idx)
    {
        while (idx > 0)
        {
            int parent = (idx - 1) / 2;
            if (_data[idx].priority >= _data[parent].priority)
                break;

            (_data[idx], _data[parent]) = (_data[parent], _data[idx]);
            idx = parent;
        }
    }

    private void SiftDown(int idx)
    {
        int count = _data.Count;

        while (true)
        {
            int left = idx * 2 + 1;
            int right = left + 1;
            int smallest = idx;

            if (left < count && _data[left].priority < _data[smallest].priority)
                smallest = left;

            if (right < count && _data[right].priority < _data[smallest].priority)
                smallest = right;

            if (smallest == idx)
                break;

            (_data[idx], _data[smallest]) = (_data[smallest], _data[idx]);
            idx = smallest;
        }
    }
}
