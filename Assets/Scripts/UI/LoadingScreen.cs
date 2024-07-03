using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private static LoadingScreen instance;

    public static LoadingScreen Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<LoadingScreen>();
            }
            return instance;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
