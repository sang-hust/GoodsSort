using System;
using System.Collections.Generic;
using System.Linq;

public class GameDataCenter : IContext
{
    private readonly Dictionary<(Type type, string tag), object> _dic =
        new Dictionary<(Type type, string tag), object>();

    public static readonly GameDataCenter Instance = new GameDataCenter();

    public IContext Get<T>(out T t) where T : class
    {
        var tagCacheLocal = CreateLocalTagCache();
        t = _dic.ContainsKey((typeof(T), tagCacheLocal)) ? _dic[(typeof(T), tagCacheLocal)] as T : default;
        return this;
    }

    public IContext GetOrCreate<T>(out T t) where T : class, new()
    {
        var tagCacheLocal = CreateLocalTagCache();
        if (_dic.ContainsKey((typeof(T), tagCacheLocal)))
            t = _dic[(typeof(T), tagCacheLocal)] as T;
        else
        {
            t = new T();
            _dic.Add((typeof(T), tagCacheLocal), t);
            if (t is IInitialize init)
                init.OnInitialize();
        }

        return this;
    }

    public IContext Remove<T>() where T : class
    {
        var tagCacheLocal = CreateLocalTagCache();
        if (_dic.ContainsKey((typeof(T), tagCacheLocal)))
            _dic.Remove((typeof(T), tagCacheLocal));
        return this;
    }

    public IContext All<T>(out IEnumerable<T> enumerable) where T : class
    {
        enumerable = from o in _dic where o.Value is T select o.Value as T;
        return this;
    }
    
    public IContext By(string tag)
    {
        _tagCache = tag;
        return this;
    }

    private string CreateLocalTagCache()
    {
        var t = _tagCache;
        _tagCache = string.Empty;
        return t;
    }

    private string _tagCache = string.Empty;
}

public interface IContext
{
    IContext Get<T>(out T t) where T : class;
    IContext GetOrCreate<T>(out T t) where T : class, new();
    IContext Remove<T>() where T : class;
    IContext All<T>(out IEnumerable<T> enumerable) where T : class;
    IContext By(string tag);
}

public interface IInitialize
{
    void OnInitialize();
}
