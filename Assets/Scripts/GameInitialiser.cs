using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialiser : MonoBehaviour
{
    public enum GameMode { INVALID, Menus, Gameplay, TaskList};

    public GameMode gameMode;

    public GameObject gameManagerPrefab = null;

    private bool menuLoaded = false;

    void Start()
    {
        if(GameManager.instance == null)
        {
            if (gameManagerPrefab)
            {
                Instantiate(gameManagerPrefab);
            }
            else
            {
                Debug.LogError("GameManager prefab is not set!");
            }
        }

        
    }

    private void Update()
    {
        // Additive Menu Loader
        if(menuLoaded == false)
        {
            switch (gameMode)
            {
                case GameMode.Menus:
                    MenuManager.instance.SwitchToMainMenus();
                    break;
                case GameMode.Gameplay:
                    MenuManager.instance.SwitchToGameplayMenus();
                    break;
                case GameMode.TaskList:
                    MenuManager.instance.SwitchToTaskListMenus();
                    break;
            }

            menuLoaded = true;
        }
    }
}
