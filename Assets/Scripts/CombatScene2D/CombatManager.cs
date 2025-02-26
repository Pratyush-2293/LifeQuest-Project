using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance = null;

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
    // player character sliders
    public Slider aldenTimeSlider = null;

    // enemy sliders
    public List<Slider> enemySliders = new List<Slider>();

    // Storing script files for all characters on field
    public PlayerCombat2D[] playerCombatControllers = new PlayerCombat2D[4];
    public EnemyCombat2D[] enemyCombatControllers = new EnemyCombat2D[4];

    private bool timeStart = true;

    private bool isAldenTurn = false;
    private bool isValricTurn = false;
    private bool isOsmirTurn = false;
    private bool isAssassinTurn = false;

    public int aldenIndex = -1;
    public int valricIndex = -1;
    public int osmirIndex = -1;
    public int assassinIndex = -1;

    private int currentSelectedTarget = 0;
    private int enemiesAlive = 0;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one CombatManager instance!");
            Destroy(gameObject);
        }
        instance = this;
    }

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

        // Counting alive enemies
        for(int i = 0; i < 4; i++)
        {
            if(enemyCombatControllers[i] != null)
            {
                enemiesAlive++;
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
                if(timeStart == true)
                {
                    if (playerCombatControllers[i] != null)
                    {
                        if (playerCombatControllers[i].turnCounter >= 100)
                        {
                            // Turn starts for ith character
                            // stop time
                            timeStart = false;

                            // setting the active character
                            if (playerCombatControllers[i].characterName == "Alden")
                            {
                                isAldenTurn = true;
                            }
                            else if (playerCombatControllers[i].characterName == "Valric")
                            {
                                isValricTurn = true;
                            }
                            else if (playerCombatControllers[i].characterName == "Osmir")
                            {
                                isOsmirTurn = true;
                            }
                            else if (playerCombatControllers[i].characterName == "Assassin")
                            {
                                isAssassinTurn = true;
                            }

                            // show action panel & update for active character
                            ShowUpdatedActionPanel();

                            break;
                        }
                    }
                }
                
                if(timeStart == true)
                {
                    // Check if an enemy has reached turn cap
                    if (enemyCombatControllers[i] != null)
                    {
                        if(enemyCombatControllers[i].isDefeated == false)
                        {
                            if (enemyCombatControllers[i].turnCounter >= 100)
                            {
                                // Turn starts for ith enemy
                                // stop time
                                timeStart = false;

                                // Make the enemy attack a player character
                                enemyCombatControllers[i].Action_PlayTurn();

                                break;
                            }
                        }
                    }
                }
            }

            // Tick time for all characters on field
            if(timeStart == true)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (playerCombatControllers[i] != null)
                    {
                        playerCombatControllers[i].turnCounter += playerCombatControllers[i].turnSpeed;
                    }

                    if (enemyCombatControllers[i] != null)
                    {
                        if(enemyCombatControllers[i].isDefeated == false)
                        {
                            enemyCombatControllers[i].turnCounter += enemyCombatControllers[i].turnSpeed;
                        }
                        else
                        {
                            enemyCombatControllers[i].turnCounter = 0;
                        }
                    }

                }
            }
            

            // Update time slider position for all characters
            UpdateTimeSliders();

            // Check if all enemies are dead
            if (enemiesAlive <= 0)
            {
                Debug.Log("Victory");
                timeStart = false;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ResumeTime()
    {
        timeStart = true;
        StartCoroutine(TickTime());
    }

    private void ShowUpdatedActionPanel()
    {
        // Updating action panel with active character's stats
        if(isAldenTurn == true)
        {
            // updating HP and MP bars
            HPSlider.maxValue = playerCombatControllers[aldenIndex].GetMaxHealthValue();
            HPSlider.value = playerCombatControllers[aldenIndex].GetHealthValue();
            MPSlider.value = playerCombatControllers[aldenIndex].GetManaValue();

            // updating name label
            nameLabel.text = playerCombatControllers[aldenIndex].characterName;
        }

        // Setting initial selected target
        int activeEnemies = CheckActiveEnemies();
        for(int i = 0; i < activeEnemies; i++)
        {
            if(enemyCombatControllers[i].isDefeated == false)
            {
                enemyCombatControllers[i].selectMarker.gameObject.SetActive(true);
                currentSelectedTarget = i;
                break;
            }
        }

        actionPanelAnimator.SetTrigger("SlideUp");
    }

    private int CheckActiveEnemies()
    {
        int activeEnemies = 0;
        for(int i = 0; i < 4; i++)
        {
            if(enemyCombatControllers[i] != null)
            {
                activeEnemies++;
            }
        }
        return activeEnemies;
    }

    private void UpdateTimeSliders()
    {
        for(int i = 0; i < 4; i++)
        {
            // update player sliders
            if(playerCombatControllers[i] != null)
            {
                aldenTimeSlider.value = playerCombatControllers[aldenIndex].turnCounter;
                // similar for valric
                // similar for osmir
                // similar for assassin girl
            }

            // update enemy sliders
            if (enemyCombatControllers[i] != null)
            {
                if(enemyCombatControllers[i].isDefeated == false)
                {
                    enemySliders[i].value = enemyCombatControllers[i].turnCounter;
                }
                else
                {
                    enemySliders[i].gameObject.SetActive(false);
                }
            }
        }
    }


    // Overloaded HandleDealtDamage function
    public void HandleDealtDamage(int dealtDamage, bool isCritical)  // this one is used by player char to deal damage to enemy
    {
        enemyCombatControllers[currentSelectedTarget].Action_TakeDamage(dealtDamage, isCritical);
    }

    public void HandleDealtDamage(int dealtDamage, int selectedTarget) // this one is used by enemy char to deal damage to player
    {
        playerCombatControllers[selectedTarget].Action_TakeDamage(dealtDamage);
    }

    // Handling Action Panel Commands
    public void OnAttackButton()
    {
        // When attack button is pressed during Alden's turn
        if (isAldenTurn)
        {
            // Play attack animation according to selected target
            playerCombatControllers[aldenIndex].DoAction("Attack" ,currentSelectedTarget);
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


        enemyCombatControllers[currentSelectedTarget].selectMarker.gameObject.SetActive(false);
        actionPanelAnimator.SetTrigger("SlideDown");
    }

    public void OnSkillButton()
    {
        actionPanelAnimator.SetTrigger("SkillShow");
    }

    public void OnBackButton()
    {
        actionPanelAnimator.SetTrigger("SkillHide");
    }

    public void OnSkill1Button()
    {

    }

    public void OnSkill2Button()
    {

    }

    public void OnDefendButton()
    {

    }

    public void OnSkill3Button()
    {

    }

    public void OnLeftButton()
    {
        if(currentSelectedTarget == 0) // Selecting next target when current target is 0
        {
            if(enemyCombatControllers[3] != null)
            {
                if(enemyCombatControllers[3].isDefeated == false)
                {
                    enemyCombatControllers[3].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[0].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 3;
                }
                else if(enemyCombatControllers[2].isDefeated == false)
                {
                    enemyCombatControllers[2].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[0].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 2;
                }
                else if(enemyCombatControllers[1].isDefeated == false)
                {
                    enemyCombatControllers[1].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[0].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 1;
                }
            }
            else if(enemyCombatControllers[2] != null)
            {
                if(enemyCombatControllers[2].isDefeated == false)
                {
                    enemyCombatControllers[2].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[0].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 2;
                }
                else if(enemyCombatControllers[1].isDefeated == false)
                {
                    enemyCombatControllers[1].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[0].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 1;
                }
            }
            else if(enemyCombatControllers[1] != null)
            {
                if(enemyCombatControllers[1].isDefeated == false)
                {
                    enemyCombatControllers[1].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[0].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 1;
                }
            }
        }
        else if(currentSelectedTarget == 1) // Selecting next target when current target is 1
        {
            if (enemyCombatControllers[0].isDefeated == false)
            {
                enemyCombatControllers[0].selectMarker.gameObject.SetActive(true);
                enemyCombatControllers[1].selectMarker.gameObject.SetActive(false);
                currentSelectedTarget = 0;
            }
            else if (enemyCombatControllers[3] != null)
            {
                if (enemyCombatControllers[3].isDefeated == false)
                {
                    enemyCombatControllers[3].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[1].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 3;
                }
                else if (enemyCombatControllers[2].isDefeated == false)
                {
                    enemyCombatControllers[2].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[1].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 2;
                }
            }
            else if (enemyCombatControllers[2] != null)
            {
                if (enemyCombatControllers[2].isDefeated == false)
                {
                    enemyCombatControllers[2].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[1].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 2;
                }
            }
        }
        else if(currentSelectedTarget == 2) // Selecting next target when current target is 2
        {
            if(enemyCombatControllers[1].isDefeated == false)
            {
                enemyCombatControllers[1].selectMarker.gameObject.SetActive(true);
                enemyCombatControllers[2].selectMarker.gameObject.SetActive(false);
                currentSelectedTarget = 1;
            }
            else if(enemyCombatControllers[0].isDefeated == false)
            {
                enemyCombatControllers[0].selectMarker.gameObject.SetActive(true);
                enemyCombatControllers[2].selectMarker.gameObject.SetActive(false);
                currentSelectedTarget = 0;
            }
            else if(enemyCombatControllers[3] != null)
            {
                if(enemyCombatControllers[3].isDefeated == false)
                {
                    enemyCombatControllers[3].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[2].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 3;
                }
            }
        }
        else if(currentSelectedTarget == 3) // Selecting next target when current target is 3
        {
            if(enemyCombatControllers[2].isDefeated == false)
            {
                enemyCombatControllers[2].selectMarker.gameObject.SetActive(true);
                enemyCombatControllers[3].selectMarker.gameObject.SetActive(false);
                currentSelectedTarget = 2;
            }
            else if(enemyCombatControllers[1].isDefeated == false)
            {
                enemyCombatControllers[1].selectMarker.gameObject.SetActive(true);
                enemyCombatControllers[3].selectMarker.gameObject.SetActive(false);
                currentSelectedTarget = 1;
            }
            else if(enemyCombatControllers[0].isDefeated == false)
            {
                enemyCombatControllers[0].selectMarker.gameObject.SetActive(true);
                enemyCombatControllers[3].selectMarker.gameObject.SetActive(false);
                currentSelectedTarget = 0;
            }
        }
    }

    public void OnRightButton()
    {
        if (currentSelectedTarget == 0) // Selecting next target when current target is 0
        {
            if (enemyCombatControllers[1] != null)
            {
                if (enemyCombatControllers[1].isDefeated == false)
                {
                    enemyCombatControllers[1].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[0].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 1;
                }
                else if(enemyCombatControllers[2] != null)
                {
                    if(enemyCombatControllers[2].isDefeated == false)
                    {
                        enemyCombatControllers[2].selectMarker.gameObject.SetActive(true);
                        enemyCombatControllers[0].selectMarker.gameObject.SetActive(false);
                        currentSelectedTarget = 2;
                    }
                    else if(enemyCombatControllers[3] != null)
                    {
                        if(enemyCombatControllers[3].isDefeated == false)
                        {
                            enemyCombatControllers[3].selectMarker.gameObject.SetActive(true);
                            enemyCombatControllers[0].selectMarker.gameObject.SetActive(false);
                            currentSelectedTarget = 3;
                        }
                    }
                }
            }
        }
        else if(currentSelectedTarget == 1) // Selecting next target when current target is 1
        {
            if (enemyCombatControllers[2] != null)
            {
                if (enemyCombatControllers[2].isDefeated == false)
                {
                    enemyCombatControllers[2].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[1].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 2;
                }
                else if (enemyCombatControllers[3] != null)
                {
                    if (enemyCombatControllers[3].isDefeated == false)
                    {
                        enemyCombatControllers[3].selectMarker.gameObject.SetActive(true);
                        enemyCombatControllers[1].selectMarker.gameObject.SetActive(false);
                        currentSelectedTarget = 3;
                    }
                    else if (enemyCombatControllers[0].isDefeated == false)
                    {
                        enemyCombatControllers[0].selectMarker.gameObject.SetActive(true);
                        enemyCombatControllers[1].selectMarker.gameObject.SetActive(false);
                        currentSelectedTarget = 0;
                    }
                }
                else if (enemyCombatControllers[0].isDefeated == false)
                {
                    enemyCombatControllers[0].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[1].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 0;
                }
            }
        }
        else if(currentSelectedTarget == 2) // Selecting next target when current target is 2
        {
            if (enemyCombatControllers[3] != null)
            {
                if (enemyCombatControllers[3].isDefeated == false)
                {
                    enemyCombatControllers[3].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[2].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 3;
                }
                else if (enemyCombatControllers[0].isDefeated == false)
                {
                    enemyCombatControllers[0].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[2].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 0;
                }
                else if (enemyCombatControllers[1].isDefeated == false)
                {
                    enemyCombatControllers[1].selectMarker.gameObject.SetActive(true);
                    enemyCombatControllers[2].selectMarker.gameObject.SetActive(false);
                    currentSelectedTarget = 1;
                }
            }
            else if (enemyCombatControllers[0].isDefeated == false)
            {
                enemyCombatControllers[0].selectMarker.gameObject.SetActive(true);
                enemyCombatControllers[2].selectMarker.gameObject.SetActive(false);
                currentSelectedTarget = 0;
            }
            else if (enemyCombatControllers[1].isDefeated == false)
            {
                enemyCombatControllers[1].selectMarker.gameObject.SetActive(true);
                enemyCombatControllers[2].selectMarker.gameObject.SetActive(false);
                currentSelectedTarget = 1;
            }
        }
        else if(currentSelectedTarget == 3) // Selecting next target when current target is 3
        {
            if (enemyCombatControllers[0].isDefeated == false)
            {
                enemyCombatControllers[0].selectMarker.gameObject.SetActive(true);
                enemyCombatControllers[3].selectMarker.gameObject.SetActive(false);
                currentSelectedTarget = 0;
            }
            else if (enemyCombatControllers[1].isDefeated == false)
            {
                enemyCombatControllers[1].selectMarker.gameObject.SetActive(true);
                enemyCombatControllers[3].selectMarker.gameObject.SetActive(false);
                currentSelectedTarget = 1;
            }
            else if (enemyCombatControllers[2].isDefeated == false)
            {
                enemyCombatControllers[2].selectMarker.gameObject.SetActive(true);
                enemyCombatControllers[3].selectMarker.gameObject.SetActive(false);
                currentSelectedTarget = 2;
            }
        }
    }
}
