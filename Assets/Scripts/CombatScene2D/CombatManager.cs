using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CombatManager : MonoBehaviour
{
    [Header("Player Characters")]
    public GameObject[] playerCharacters = new GameObject[4];

    [Header("Enemy Characters")]
    public GameObject[] enemyCharacters = new GameObject[4];

    [Header("Alden's Action Panel")]
    public Animator aldenActionPanelAnimator = null;
    public Slider aldenHPSlider = null;
    public Slider aldenMPSlider = null;
    public Slider aldenTimeSlider = null;

    // Storing script files for all characters on field
    private PlayerCombat2D[] playerCombatControllers = new PlayerCombat2D[4];
    private EnemyCombat2D[] enemyCombatControllers = new EnemyCombat2D[4];

    private bool timeStart = true;

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

                        // show action panel
                        if(playerCombatControllers[i].characterName == "Alden")
                        {
                            aldenActionPanelAnimator.SetTrigger("SlideUp");
                        }
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

    private void UpdateTimeSliders()
    {
        for(int i = 0; i < 4; i++)
        {
            if(playerCombatControllers[i] != null)
            {
                if(playerCombatControllers[i].characterName == "Alden")
                {
                    aldenTimeSlider.value = playerCombatControllers[i].turnCounter;
                }
                if(playerCombatControllers[i].characterName == "Valric")
                {
                    // update valric's slider
                }
                if(playerCombatControllers[i].characterName == "Osmir")
                {
                    // update Osmir's slider
                }
                if(playerCombatControllers[i].characterName == "Assassin Girl")
                {
                    // update assassin girl's slider
                }
            }

            if(enemyCombatControllers[i] != null)
            {
                // update enemy sliders
            }
        }
    }
}
