using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHubMenuManager : MonoBehaviour
{
    public void OnLevelSelectButton()
    {
        // open level select scene
    }

    public void OnHeroesButton()
    {
        SceneManager.LoadScene("HeroesMenu");
    }
}
