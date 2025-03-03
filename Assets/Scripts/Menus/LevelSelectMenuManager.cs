using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectMenuManager : MonoBehaviour
{
    public TMP_Text levelUnlockKeysDisplay = null;
    public GameObject touchBlockerPanel = null;
    public GameObject unlockLevelPromptPanel = null;
    public GameObject insufficientKeysPromptPanel = null;

    public List<Button> levelButtons = new List<Button>();

    private void Start()
    {
        levelUnlockKeysDisplay.text = GameData.instance.levelKeys.ToString();

        UpdateAvailableLevelButtons();
        UpdateCompletedLevelButtons();
    }

    // Need to check the max unlocked levels and make those buttons interactible
    private void UpdateAvailableLevelButtons()
    {
        for(int i = 0; i < levelButtons.Count; i++)
        {
            if (i <= GameData.instance.maxAvailableLevel - 1)
            {
                levelButtons[i].interactable = true;
            }
        }
    }

    private void UpdateCompletedLevelButtons()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (i <= GameData.instance.maxCompletedLevel - 1)
            {
                // change the button sprite here
            }
        }
    }

    public void OnLevelButton(int levelID)
    {
        if (levelID > GameData.instance.maxUnlockedLevel)
        {
            // open the unlock level prompt
            if (GameData.instance.levelKeys > 0)
            {
                touchBlockerPanel.gameObject.SetActive(true);
                unlockLevelPromptPanel.gameObject.SetActive(true);
            }
            else
            {
                // open insufficient keys prompt if unlock keys available are 0.
                touchBlockerPanel.gameObject.SetActive(true);
                insufficientKeysPromptPanel.gameObject.SetActive(true);
            }
        }
        else
        {
            SceneManager.LoadScene("Level_" + levelID.ToString() + "-1");
        }
    }

    public void OnUnlockButton()
    {
        // Increment the unlocked levels, decrement the level keys and save changes.
        GameData.instance.maxUnlockedLevel++;
        GameData.instance.levelKeys--;
        SaveManager.instance.SaveGameData();

        // update available keys display
        levelUnlockKeysDisplay.text = GameData.instance.levelKeys.ToString();

        // close the prompt panel
        unlockLevelPromptPanel.gameObject.SetActive(false);
        touchBlockerPanel.gameObject.SetActive(false);
    }

    public void OnCancelButton()
    {
        // close the panel
        unlockLevelPromptPanel.gameObject.SetActive(false);
        touchBlockerPanel.gameObject.SetActive(false);
    }

    public void OnOKButton()
    {
        // close the panel
        insufficientKeysPromptPanel.gameObject.SetActive(false);
        touchBlockerPanel.gameObject.SetActive(false);
    }
}
