using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHubMenuManager : MonoBehaviour
{
    public Button dungeonsButton;
    public Button marketButton;

    private void Start()
    {
        if(GameData.instance != null)
        {
            if(GameData.instance.maxCompletedLevel >= 9)
            {
                marketButton.interactable = true;
            }

            if(GameData.instance.maxCompletedLevel >= 11)
            {
                dungeonsButton.interactable = true;
            }
        }
    }

    public SceneTransition sceneTransition;
    public void OnLevelSelectButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        sceneTransition.LoadSceneWithTransition("LevelSelectMenu");
    }

    public void OnHeroesButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        sceneTransition.LoadSceneWithTransition("HeroesMenu");
    }

    public void OnMarketButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        sceneTransition.LoadSceneWithTransition("MarketMenu");
    }

    public void OnBackButton()
    {
        AudioManager.instance.PlayUISound("uiClick2");

        sceneTransition.LoadSceneWithTransition("MainMenuScene");
    }

    public void OnDungeonsButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        sceneTransition.LoadSceneWithTransition("DungeonSelectMenu");
    }
}
