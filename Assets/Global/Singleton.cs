using System;
using UnityEngine;
using Exception = System.Exception;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    public static event Action OnInitialized;
    
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            OnInitialized?.Invoke();
        }

        else
        {
            throw new Exception("Game Object: " + gameObject.name + " - " + GetType().Name + " Instance is already instantiated.");
        }
    }
    
    // To implement from just inherit from it, and then you can access (CLASSNAME).Instance from other scripts.
    // If you do stuff on Awake() in your child script that inherits from it, override it and call base.Awake() at the beginning.
    // Singletons should only be present in the Global Scene.
    
    // If you need to ensure the Singleton has finished initializing before accessing it or need to do something after it has, hook into the OnInitialized event.
}