Ths repo shows _one_ solution to a problem that you might have when binding to configuration (`appsettings.json` or similar) in your .NET app.

## The problem:

> I want to bind a collection from config, but if the config doesn't exist, I want to use some default values. I want either what's in config, **or** the default values, **but not both**

For instance, given this type:

```csharp
public class MyImageProcessingOptions
{
    public IList<int> ResizeWidths { get; } = new List<int> { 100, 200, 400, 800 }; 
}
```

If the config **doesn't** contain an entry for `ResizeWidths`, then the values should be the defaults.  But if config **does** contain that entry, e.g in the `appsettings.json` file:

```json
{
  "MyImageProcessingOptions": {
    "ResizeWidths":  [
      "42",
      "69",
      "666"
    ]
  } 
}
```

... then those values should be used **and the default values should disappear**.

The current behaviour when binding collections is to **append** any values in config to the collection you created in your code.

At the time of writing, there is [an open API Proposal](https://github.com/dotnet/runtime/issues/62112) for a new flag in `BinderOptions` to overwrite the collection. But there's an easier way...

## The solution

_(or one of them)_

Without requiring any Runtime API changes, one solution is to hand-role your own defaultable collection:

```csharp
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
```

... and then just declare your collection to be a `DefaultableCollection` instead of `IList`:

```csharp
public class MyImageProcessingOptions
{
    public DefaultableCollection<int> ResizeWidths { get; } = new(100, 200, 400, 800);
}
```