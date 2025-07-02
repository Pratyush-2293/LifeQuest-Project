using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
    public static MainMenu instance = null;

    public SceneTransition sceneTransition;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one MainMenu!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        if (!AudioManager.instance.isPlayingBGMTrack("hometownVillage"))
        {
            AudioManager.instance.PlayMusic("hometownVillage");
        }
    }

    public void OnStartGameButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        sceneTransition.gameObject.SetActive(true);

        if(GameData.instance != null)
        {
            if(GameData.instance.firstTimeEquipmentLoaded == false)
            {
                GameManager.instance.AldenItemsInitialise();
            }
        }

        sceneTransition.LoadSceneWithTransition("GameHubMenu");
    }

    public void OnTaskListButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        TurnOff(false);
        TaskListMenu.instance.TurnOn(this);
        TaskListMenu.instance.LoadPlayerTasks();
    }

    public void OnSettingsButton()
    {
        AudioManager.instance.PlayUISound("uiClick2");

        TurnOff(false);
        SettingsMenu.instance.TurnOn(this);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
