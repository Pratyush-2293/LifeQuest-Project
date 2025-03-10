using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHubMenuManager : MonoBehaviour
{
    public void OnLevelSelectButton()
    {
        SceneManager.LoadScene("LevelSelectMenu");
    }

    public void OnHeroesButton()
    {
        SceneManager.LoadScene("HeroesMenu");
    }

    public void OnMarketButton()
    {
        SceneManager.LoadScene("MarketMenu");
    }
}
