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

    public ParticleSystem expParticles = null;
    public Animator levelUpAnnounceAnimator = null;

    public Button addEXPButton = null;

    public enum ActiveCharacter { Alden, Valric, Osmir, Assassin };
    public ActiveCharacter activeCharacter = ActiveCharacter.Alden;

    private int expToAdd = 50;
    private int baseExpToAdd = 50;
    private Coroutine saveCoroutine;    // NOTE: Remember to save data when exiting to main menu

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

        // Set the exp to add multiplier based on active character level
        SetEXPToAdd();

        // Turn off xp button's interactability if no xp points in pool
        if (GameData.instance.expPoints <= 0)
        {
            addEXPButton.interactable = false;
        }
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

    public void OnAddEXPButton()  //NOTE: Also need to update xp bar and xp text upon adding xp
    {
        if(activeCharacter == ActiveCharacter.Alden) // Handling for Alden
        {
            if (GameData.instance.expPoints <= expToAdd) // Check if there is enough xp in the xp pool to add to char, if not then add what is remaining in the pool
            {
                expToAdd = GameData.instance.expPoints;
            }

            if(expToAdd > 0) // if there is some exp to add, we proceed
            {
                if (GameData.instance.aldenEXP + expToAdd <= GameData.instance.aldenEXPForNextLevel)  // Check to see if the xp we are adding is not more than the xp cap
                {
                    GameData.instance.aldenEXP += expToAdd;  // if it isn't, we add the full amount
                    GameData.instance.expPoints -= expToAdd; // deduct the added xp from the xp pool.

                    if(GameData.instance.aldenEXP >= GameData.instance.aldenEXPForNextLevel) // Check to see if level cap reached
                    {
                        AldenLevelUp(); // call the level up function
                    }
                }
                else // We only add the amount required to reach level cap, then start a function which handles leveling up.
                {
                    int expForNextLevel = GameData.instance.aldenEXPForNextLevel - GameData.instance.aldenEXP;
                    GameData.instance.aldenEXP += expForNextLevel;
                    GameData.instance.expPoints -= expForNextLevel;

                    // Trigger the function that handles leveling up
                    AldenLevelUp();
                }
            }
        }
        // CONTINUE THIS SCRIPT TO HANDLE OTHER CHARACTERS

        // Update the level info UI
        UpdateLevelInfoUI();

        // At the end, we check if the xp pool is empty, if it is then we disable the add xp button.
        if (GameData.instance.expPoints <= 0)
        {
            addEXPButton.interactable = false;
        }

        // Play the particle effects
        expParticles.Stop();         // stopping it first in case it was already running
        expParticles.Play();

        // Trigger the debounced save
        TriggerSaveWithDelay();
    }

    private void AldenLevelUp()
    {
        // Set alden current xp to zero
        GameData.instance.aldenEXP = 0;

        // Increase the char's stats based on level increase.

        // First we deduct the character's raw stats from his total stats based on current level
        GameData.instance.aldenHP -= GameData.instance.aldenLevel * 10;                          
        GameData.instance.aldenATK -= GameData.instance.aldenLevel * 8; // The multiplier values are saved in documentation for reference
        GameData.instance.aldenDEF -= GameData.instance.aldenLevel * 6;

        // then we add the new stats based on the next level (current level + 1)
        GameData.instance.aldenHP += (GameData.instance.aldenLevel + 1) * 10;
        GameData.instance.aldenATK += (GameData.instance.aldenLevel + 1) * 8;
        GameData.instance.aldenDEF += (GameData.instance.aldenLevel + 1) * 6;

        // increase alden's level
        GameData.instance.aldenLevel++;

        // change value of xp required to level up
        GameData.instance.aldenEXPForNextLevel = (int)(100 * GameData.instance.aldenLevel * 0.75f);

        // update the xp bar, xp text and level text
        UpdateLevelInfoUI();

        // update the stats diplay
        UpdateStatsDisplay();

        // Refresh the EXP to add with new level value
        SetEXPToAdd();

        // Show level up announce text
        levelUpAnnounceAnimator.SetTrigger("SlideIn");
    }

    private void UpdateLevelInfoUI()
    {
        if(activeCharacter == ActiveCharacter.Alden) // Handling for Alden
        {
            // Update the xp bar slider
            charExpSliderDisplay.maxValue = GameData.instance.aldenEXPForNextLevel;
            charExpSliderDisplay.value = GameData.instance.aldenEXP;

            // Update the xp bar text
            charExpDisplay.text = GameData.instance.aldenEXP.ToString() + " / " + GameData.instance.aldenEXPForNextLevel.ToString();

            // Update the xp pool text
            expPointsDisplay.text = GameData.instance.expPoints.ToString();

            // Update the level display text
            if (GameData.instance.aldenLevel < 10)
            {
                charLevelDisplay.text = "0" + GameData.instance.aldenLevel.ToString();
            }
            else
            {
                charLevelDisplay.text = GameData.instance.aldenLevel.ToString();
            }
        }
        
    }

    private void SetEXPToAdd()
    {
        if(activeCharacter == ActiveCharacter.Alden)
        {
            if(GameData.instance.aldenLevel < 10)
            {
                expToAdd = baseExpToAdd * 1;
            }
            else if(GameData.instance.aldenLevel >= 10 && GameData.instance.aldenLevel < 20)
            {
                expToAdd = baseExpToAdd * 2;
            }
            else if (GameData.instance.aldenLevel >= 20 && GameData.instance.aldenLevel < 30)
            {
                expToAdd = baseExpToAdd * 3;
            }
            else if (GameData.instance.aldenLevel >= 30 && GameData.instance.aldenLevel < 40)
            {
                expToAdd = baseExpToAdd * 4;
            }
            else if (GameData.instance.aldenLevel >= 40 && GameData.instance.aldenLevel < 50)
            {
                expToAdd = baseExpToAdd * 5;
            }
            else if (GameData.instance.aldenLevel >= 50 && GameData.instance.aldenLevel < 60)
            {
                expToAdd = baseExpToAdd * 6;
            }
            else if (GameData.instance.aldenLevel >= 60 && GameData.instance.aldenLevel < 70)
            {
                expToAdd = baseExpToAdd * 7;
            }
            else if (GameData.instance.aldenLevel >= 70 && GameData.instance.aldenLevel < 80)
            {
                expToAdd = baseExpToAdd * 8;
            }
        }
        // CONTINUE THIS CHAIN TO HANDLE OTHER CHARACTERS
    }


    // Debounced Save Mechanism
    private void TriggerSaveWithDelay()
    {
        if (saveCoroutine != null)
        {
            StopCoroutine(saveCoroutine);
        }
        saveCoroutine = StartCoroutine(DelayedSave());
    }

    private IEnumerator DelayedSave()
    {
        yield return new WaitForSeconds(2f); // Wait 2 second before saving
        SaveManager.instance.SaveGameData();
    }
}
