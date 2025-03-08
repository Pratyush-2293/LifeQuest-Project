using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HeroesMenuManager : MonoBehaviour
{
    public static HeroesMenuManager instance = null;

    public int pageNumber = 0;

    public TMP_Text expPointsDisplay = null;
    public TMP_Text charNameDisplay = null;
    public TMP_Text charLevelDisplay = null;
    public Slider charExpSliderDisplay = null;
    public TMP_Text charExpDisplay = null;

    public TMP_Text charAtkDisplay = null;
    public TMP_Text charDefDisplay = null;
    public TMP_Text charMaxHPDisplay = null;
    public TMP_Text charMaxMPDisplay = null;
    public TMP_Text charCritRateDisplay = null;
    public TMP_Text charCritDamageDisplay = null;

    public enum ActiveCharacter { Alden, Valric, Osmir, Assassin };
    public ActiveCharacter activeCharacter = ActiveCharacter.Alden;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Trying to create more than one HeroesMenuManager instance!");
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
        // Load EXP points to display
        expPointsDisplay.text = GameData.instance.expPoints.ToString();

        // Load alden's stats to start with
        LoadAldenStats();
    }

    public void UpdateStatsDisplay()
    {
        if(activeCharacter == ActiveCharacter.Alden)
        {
            LoadAldenStats();
        }
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
        charAtkDisplay.text = GameData.instance.aldenATK.ToString();
        charDefDisplay.text = GameData.instance.aldenDEF.ToString();
        charMaxHPDisplay.text = GameData.instance.aldenHP.ToString();
        charMaxMPDisplay.text = GameData.instance.aldenMP.ToString();
        charCritRateDisplay.text = GameData.instance.aldenCR.ToString() + "%";
        charCritDamageDisplay.text = GameData.instance.aldenCD.ToString() + "%";
    }

    public void OnBackButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameHubMenu");
    }
}
