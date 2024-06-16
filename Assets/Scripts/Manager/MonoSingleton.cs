using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    GameObject gameObject = new GameObject(typeof(T).Name);

                    instance = gameObject.AddComponent<T>();
                }

                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
}
