using UnityEngine;
using UnityEngine.Assertions;


public static class ComponentUtility
{
    public static T GetComponentSafe<T>(this Component c) where T : Component
    {
        T got = c.GetComponent<T>();
        
        Assert.IsFalse(got == null, $"'{c.GetType().Name}' on game object '{c.name}' could not get component of type '{typeof(T).Name}'");
        
        
        Debug.Log($"GOT: {got == null} -- {got}");
        return got;
    }
}