using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one instance of GameManager!");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameManager Created.");
    }
}
