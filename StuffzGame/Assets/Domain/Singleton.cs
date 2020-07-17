using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Check to see if we're about to be destroyed.
    private static bool isQuitting = false;
    private static readonly object @lock = new object();
    private static T _instance;
    private static readonly bool enableDebug = false;

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    /// 
    public static T Instance
    {
        get
        {
            if (isQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }
            lock (@lock)
            {
                if (_instance == null)
                {
                    GameObject coreGameObject = new GameObject("(Singleton)"+typeof(T).Name);
                    _instance = coreGameObject.AddComponent<T>();
                    Debug.LogWarning("Creating new instance of (Singleton)" + typeof(T).Name);
                    return _instance;
                }
                else
                {
                    if (enableDebug) { Debug.LogWarning(typeof(T).Name + " already exists, returning instance"); }
                    return _instance;
                }
            }
           
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Destroying Singleton gameObject " +gameObject.name);
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }


    private void OnDestroy()
    {
        //isQuitting = true;
    }
}