using System;
using UnityEngine;

// Singleton base class, for a new sub-systems and/or managers.
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
            }
            return _instance;
        }
    }

    protected static T _instance;

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
    }
}
