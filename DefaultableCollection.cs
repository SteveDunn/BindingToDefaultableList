using System.Collections;

public class DefaultableCollection<T> : ICollection<T>
{
    private readonly List<T> _items;
    private bool _overridden;

    public DefaultableCollection(params T[] defaults) => _items = defaults.ToList();

    public void Add(T value)
    {
        if (!_overridden)
        {
            _overridden = true;
            _items.Clear();
        }
        
        _items.Add(value);
    }

    public void Clear() => _items.Clear();

    public bool Contains(T item) => _items.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

    public bool Remove(T item) => _items.Remove(item);

    public int Count => _items.Count;
    
    public bool IsReadOnly => false;

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}