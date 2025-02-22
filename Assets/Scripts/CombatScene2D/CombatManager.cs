using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class CombatManager : MonoBehaviour
{
    [Header("Player Characters")]
    public GameObject[] playerCharacters = new GameObject[4];

    [Header("Enemy Characters")]
    public GameObject[] enemyCharacters = new GameObject[4];

    [Header("Action Panel")]
    public Animator actionPanelAnimator = null;
    public TMP_Text nameLabel = null;
    public Slider HPSlider = null;
    public Slider MPSlider = null;

    [Header("Time Sliders")]
    public Slider aldenTimeSlider = null;

    // Storing script files for all characters on field
    private PlayerCombat2D[] playerCombatControllers = new PlayerCombat2D[4];
    private EnemyCombat2D[] enemyCombatControllers = new EnemyCombat2D[4];

    private bool timeStart = true;

    private bool isAldenTurn = false;
    private bool isValricTurn = false;
    private bool isOsmirTurn = false;
    private bool isAssassinTurn = false;

    private int aldenIndex = -1;
    private int valricIndex = -1;
    private int osmirIndex = -1;
    private int assassinIndex = -1;

    private void Start()
    {
        // Initialising player & enemy controller scripts
        for(int i=0; i < 4; i++)
        {
            if(playerCharacters[i].gameObject != null)
            {
                playerCombatControllers[i] = playerCharacters[i].gameObject.GetComponent<PlayerCombat2D>();
            }

            if (enemyCharacters[i].gameObject != null)
            {
                enemyCombatControllers[i] = enemyCharacters[i].gameObject.GetComponent<EnemyCombat2D>();
            }
        }

        // Assigning Index values for Player Characters (to save time later by skipping a linear search every time we reference)
        for(int i = 0; i < 4; i++)
        {
            if(playerCombatControllers[i] != null)
            {
                if (playerCombatControllers[i].characterName == "Alden")
                {
                    aldenIndex = i;
                }
                if (playerCombatControllers[i].characterName == "Valric")
                {
                    valricIndex = i;
                }
                if (playerCombatControllers[i].characterName == "Osmir")
                {
                    osmirIndex = i;
                }
                if (playerCombatControllers[i].characterName == "Assassin")
                {
                    assassinIndex = i;
                }
            }
        }

        // Start a function that ticks turn count for all characters
        StartCoroutine(TickTime());
    }

    public IEnumerator TickTime()
    {
        while(timeStart == true)
        {
            // Check if a character has reached turn cap
            for (int i = 0; i < 4; i++)
            {
                if (playerCombatControllers[i] != null)
                {
                    if(playerCombatControllers[i].turnCounter >= 100)
                    {
                        // Turn starts for ith character
                        // stop time
                        timeStart = false;

                        // setting the active character
                        if(playerCombatControllers[i].characterName == "Alden")
                        {
                            isAldenTurn = true;
                        }
                        else if(playerCombatControllers[i].characterName == "Valric")
                        {
                            isValricTurn = true;
                        }
                        else if(playerCombatControllers[i].characterName == "Osmir")
                        {
                            isOsmirTurn = true;
                        }
                        else if(playerCombatControllers[i].characterName == "Assassin")
                        {
                            isAssassinTurn = true;
                        }

                        // show action panel & update for active character
                        ShowUpdatedActionPanel();
                    }

                }

                if (enemyCombatControllers[i] != null)
                {
                    enemyCombatControllers[i].turnCounter += enemyCombatControllers[i].turnSpeed;
                }
            }

            // Tick time for all characters on field
            for (int i = 0; i < 4; i++)
            {
                if (playerCombatControllers[i] != null)
                {
                    playerCombatControllers[i].turnCounter += playerCombatControllers[i].turnSpeed;
                }

                if (enemyCombatControllers[i] != null)
                {
                    enemyCombatControllers[i].turnCounter += enemyCombatControllers[i].turnSpeed;
                }

            }

            // Update time slider position for all characters
            UpdateTimeSliders();

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ShowUpdatedActionPanel()
    {
        // Updating action panel with active character's stats
        if(isAldenTurn == true)
        {
            // updating HP and MP bars
            HPSlider.value = playerCombatControllers[aldenIndex].health;
            MPSlider.value = playerCombatControllers[aldenIndex].mana;

            // updating name label
            nameLabel.text = playerCombatControllers[aldenIndex].characterName;
        }

        actionPanelAnimator.SetTrigger("SlideUp");
    }

    private void UpdateTimeSliders()
    {
        for(int i = 0; i < 4; i++)
        {
            if(playerCombatControllers[i] != null)
            {
                aldenTimeSlider.value = playerCombatControllers[aldenIndex].turnCounter;
                // similar for valric
                // similar for osmir
                // similar for assassin girl
            }

            if(enemyCombatControllers[i] != null)
            {
                // update enemy sliders
            }
        }
    }

    // Handling Action Panel Commands

    public void OnAttackButton()
    {
        // When attack button is pressed during Alden's turn
        if (isAldenTurn)
        {

        }

        // When attack button is pressed during Valric's turn
        if (isValricTurn)
        {

        }

        // When attack button is pressed during Osmir's turn
        if (isOsmirTurn)
        {

        }

        // When attack button is pressed during Assassin's Turn
        if (isAssassinTurn)
        {

        }
    }

    public void OnSkillButton()
    {

    }

    public void OnDefendButton()
    {

    }

    public void OnLeftButton()
    {

    }

    public void OnRightButton()
    {

    }
}
