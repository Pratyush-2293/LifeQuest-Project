using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("SettingsMenu", LoadSceneMode.Additive);
        SceneManager.LoadScene("TaskListMenu", LoadSceneMode.Additive);
    }

    public void SwitchToTaskListMenus()
    {

    }
}
