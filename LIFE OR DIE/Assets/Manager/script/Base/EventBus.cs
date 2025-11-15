using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus 
{
 public static Dictionary<Type,Delegate> handlers = new Dictionary<Type,Delegate>();

    public static void Add<T>(Action<T> callBack) where T : struct
    {
        if(handlers.TryGetValue(typeof(T),out var del))
        {
            handlers[typeof(T)] = del as Action<T> + callBack;
        }
        else
        {
            handlers[typeof(T)] = callBack;
        }
    }

    public static void Remove<T>(Action<T> callBack) where T: struct
    {
        if (!handlers.ContainsKey(typeof(T)))return;
        var del = handlers[typeof(T)] as Action<T> - callBack;
        if(del == null) handlers.Remove(typeof(T));
        else handlers[typeof(T)] = del;
    }

    public static void Publish<T>(T e) where T : struct
    {
        if (handlers.TryGetValue(typeof(T), out var del))
        {
            var temp = del as Action<T>;
            temp?.Invoke(e);
        }
    }
}
