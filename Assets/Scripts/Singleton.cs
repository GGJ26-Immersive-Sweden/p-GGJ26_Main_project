using UnityEngine;

/// <summary>
/// This base class keeps the Singleton behaviour in one place, so
/// any Manager Class can inherit.
/// The Awake and OnDestroy Need to call `base.` to allow the 
/// implementation of the base class to be called.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance {get; private set;}
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    
    /* // This is not Relevant for General Singletons
    protected virtual void OnDestroy()
    {
        if (Instance == this as T)
            Instance = null;
    }
    */
}