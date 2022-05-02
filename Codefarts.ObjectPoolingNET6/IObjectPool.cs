namespace Codefarts.ObjectPooling;

public interface IObjectPool<TKey, T>
{
    T Get(TKey id);
}