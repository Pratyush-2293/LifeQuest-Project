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
        sceneTransition.LoadSceneWithTransition("LevelSelectMenu");
    }

    public void OnHeroesButton()
    {
        sceneTransition.LoadSceneWithTransition("HeroesMenu");
    }

    public void OnMarketButton()
    {
        sceneTransition.LoadSceneWithTransition("MarketMenu");
    }

    public void OnBackButton()
    {
        sceneTransition.LoadSceneWithTransition("MainMenuScene");
    }

    public void OnDungeonsButton()
    {
        sceneTransition.LoadSceneWithTransition("DungeonSelectMenu");
    }
}
