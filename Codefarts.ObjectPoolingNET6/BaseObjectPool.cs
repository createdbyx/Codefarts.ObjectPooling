namespace Codefarts.ObjectPooling;

using System.Collections.Generic;
using System.Collections.Concurrent;

public abstract class BaseObjectPool<TKey, T> : IObjectPool<TKey, T>
{
    private IDictionary<TKey, T> pool = new ConcurrentDictionary<TKey, T>();

    protected abstract T CreateObject(TKey id);

    public T Get(TKey id)
    {
        this.pool.TryAdd(id, this.CreateObject(id));
        return this.pool[id];
    }
}