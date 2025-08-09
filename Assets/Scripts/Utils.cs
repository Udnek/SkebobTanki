using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utils
{
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        var component = obj.GetComponent<T>();
        if(component == null) component = obj.AddComponent<T>();
        return component;
    }
    public static T GetOrAddComponent<T>(this Component obj) where T : Component
    {
        return GetOrAddComponent<T>(obj.gameObject);
    }
    public static void RemoveLast<T>(this List<T> list) => list.RemoveAt(list.Count - 1);
    public static T Pop<T>(this List<T> list)
    {
        var last = list.Last();
        list.RemoveLast();
        return last;
    }

    public static V GetAndRemove<K, V>(this Dictionary<K, V> dictionary, K key)
    {
        var value = dictionary[key];
        dictionary.Remove(key);
        return value;
    }

    public static V GetOrAdd<K, V>(this Dictionary<K, V> dictionary, K key, V addValue)
    {
        var value = dictionary.GetValueOrDefault(key, addValue);
        if (addValue.Equals(value)) dictionary.Add(key, value);
        return value;
    }
    
    
    
    
    
    
    
    
    
    
}