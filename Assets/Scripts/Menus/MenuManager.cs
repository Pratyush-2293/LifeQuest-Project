using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance = null;

    internal Menu activeMenu = null;

    void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one MenuManager!");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SwitchToGameplayMenus()
    {

    }

    public void SwitchToMainMenus()
    {

    }
}
