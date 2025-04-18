using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonSelectMenuManager : MonoBehaviour
{
    public TMP_Text dungeonUnlockKeysDisplay = null;
    public GameObject touchBlockerPanel = null;
    public GameObject enterDungeonPromptPanel = null;
    public GameObject insufficientKeysPromptPanel = null;

    public SceneTransition sceneTransition;

    //private variables
    private int currentDungeonID = 1;

    private void Start()
    {
        dungeonUnlockKeysDisplay.text = GameData.instance.dungeonKeys.ToString();
    }

    // Need to check the max unlocked levels and make those buttons interactible

    public void OnDungeonEnterButton(int dungeonID)
    {
        currentDungeonID = dungeonID;

        if (dungeonID == 1) // Old Branstead Catacombs
        {
            // open the enter dungoen prompt
            if (GameData.instance.dungeonKeys > 0)
            {
                touchBlockerPanel.gameObject.SetActive(true);
                enterDungeonPromptPanel.gameObject.SetActive(true);
            }
            else
            {
                // open insufficient keys prompt if unlock keys available are 0.
                touchBlockerPanel.gameObject.SetActive(true);
                insufficientKeysPromptPanel.gameObject.SetActive(true);
            }
        }
    }

    public void OnEnterButton()
    {
        // Decrement the level keys and save changes.
        GameData.instance.dungeonKeys--;
        SaveManager.instance.SaveGameData();

        // update available keys display
        dungeonUnlockKeysDisplay.text = GameData.instance.dungeonKeys.ToString();

        // load the dungeon
        if(currentDungeonID == 1)
        {
            sceneTransition.LoadSceneWithTransition("OldBransteadCatacombs");
        }
        // Continue here for adding more dungeons
    }

    public void OnCancelButton()
    {
        // close the panel
        enterDungeonPromptPanel.gameObject.SetActive(false);
        touchBlockerPanel.gameObject.SetActive(false);
    }

    public void OnOKButton()
    {
        // close the panel
        insufficientKeysPromptPanel.gameObject.SetActive(false);
        touchBlockerPanel.gameObject.SetActive(false);
    }

    public void OnBackButton()
    {
        sceneTransition.LoadSceneWithTransition("GameHubMenu");
    }
}
