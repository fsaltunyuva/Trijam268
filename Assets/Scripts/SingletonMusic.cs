using UnityEngine;

public class SingletonMusic : MonoBehaviour
{
    private static SingletonMusic instance = null;

    public static SingletonMusic Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
}

