using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HeroesMenuManager : MonoBehaviour
{
    public int pageNumber = 0;

    public TMP_Text expPointsDisplay = null;
    public TMP_Text charNameDisplay = null;
    public TMP_Text charLevelDisplay = null;
    public Slider charExpSliderDisplay = null;
    public TMP_Text charExpDisplay = null;

    public TMP_Text charStrDisplay = null;
    public TMP_Text charDefDisplay = null;
    public TMP_Text charVitDisplay = null;
    public TMP_Text charMaxHPDisplay = null;
    public TMP_Text charMaxMPDisplay = null;
    public TMP_Text charCritRateDisplay = null;

    private void Start()
    {
        // Load EXP points to display
        expPointsDisplay.text = GameData.instance.expPoints.ToString();

        // Load alden's stats to start with
        LoadAldenStats();
    }

    private void LoadAldenStats()
    {
        // Setting the character name
        charNameDisplay.text = "Alden";

        // Setting the character level
        if(GameData.instance.aldenLevel < 10)
        {
            charLevelDisplay.text = "0" + GameData.instance.aldenLevel.ToString();
        }
        else
        {
            charLevelDisplay.text = GameData.instance.aldenLevel.ToString();
        }

        // Setting the Slider
        charExpSliderDisplay.maxValue = GameData.instance.aldenEXPForNextLevel;
        charExpSliderDisplay.value = GameData.instance.aldenEXP;

        // Setting the Exp text below slider
        charExpDisplay.text = GameData.instance.aldenEXP.ToString() + " / " + GameData.instance.aldenEXPForNextLevel;

        // Setting the character stats
        charStrDisplay.text = GameData.instance.aldenSTR.ToString();
        charDefDisplay.text = GameData.instance.aldenDEF.ToString();
        charVitDisplay.text = GameData.instance.aldenVIT.ToString();
        charMaxHPDisplay.text = GameData.instance.aldenHP.ToString();
        charMaxMPDisplay.text = GameData.instance.aldenMP.ToString();
        charCritRateDisplay.text = GameData.instance.aldenCRIT.ToString();
    }

    public void OnBackButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameHubMenu");
    }
}
