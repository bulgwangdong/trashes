using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy2 : MonoBehaviour
{
    public static DontDestroy2 instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
