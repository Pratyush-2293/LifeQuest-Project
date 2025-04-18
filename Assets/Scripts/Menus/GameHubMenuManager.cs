using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHubMenuManager : MonoBehaviour
{
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
