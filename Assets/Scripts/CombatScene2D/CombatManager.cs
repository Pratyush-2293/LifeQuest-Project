using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance = null;

    [Header("Level Info")]
    public float levelID;
    public string loadNextSceneName;
    public bool isRepeatableScene = false;

    [Header("Player Characters")]
    [Space(5)]
    public GameObject[] playerCharacters = new GameObject[4];


    [Header("Enemy Characters")]
    [Space(5)]
    public GameObject[] enemyCharacters = new GameObject[4];


    [Header("Action Panel")]
    [Space(5)]
    public Animator actionPanelAnimator = null;
    public TMP_Text nameLabel = null;
    public Slider HPSlider = null;
    public Slider MPSlider = null;
    public Button skill1Button = null;
    public Button skill2Button = null;
    public Button skill3Button = null;
    public GameObject actionPanelBlocker;

    // Player Status Display Icons
    [Space(10)]
    public GameObject playerStatusBar;
    // Boons
    public Image playerMight;
    public Image playerFocus;
    public Image playerHaste;
    public Image playerClarity;
    public Image playerBarrier;
    [Space(5)]
    //Conditions
    public Image playerFrailty;
    public Image playerSlowed;
    public Image playerConfusion;
    public Image playerExposed;


    [Header("Selected Enemy Panel")]
    [Space(5)]
    public Animator enemyInfoPanelAnimator = null;
    public Image enemyProfileImageDisplay = null;
    public TMP_Text enemyNameDisplay = null;
    public TMP_Text enemyActionsDisplay = null;
    public Slider enemyHealthDisplay = null;
    public TMP_Text enemyHealthValueDisplay = null;

    // Enemy Status Display Icons
    [Space(10)]
    public GameObject enemyStatusBar;
    [Space(10)]
    // Boons
    public Image enemyMight;
    public Image enemyFocus;
    public Image enemyHaste;
    public Image enemyClarity;
    public Image enemyBarrier;
    [Space(10)]
    //Conditions
    public Image enemyFrailty;
    public Image enemySlowed;
    public Image enemyConfusion;
    public Image enemyExposed;


    [Header("End Scene Panels")]
    [Space(5)]
    public GameObject touchBlockerPanel = null;
    public Animator victoryPanelAnimator = null;
    public Animator defeatPanelAnimator = null;


    [Header("Time Sliders")]
    [Space(5)]
    // player character sliders
    public Slider aldenTimeSlider = null;
    public Slider valricTimeSlider = null;

    // enemy sliders
    public List<Slider> enemySliders = new List<Slider>();

    // enemy slider markers
    public List<Sprite> enemySliderMarkers = new List<Sprite>();

    [Header("Other Components")]
    public RewardsManager rewardsManager;

    // Storing script files for all characters on field
    [HideInInspector] public PlayerCombat2D[] playerCombatControllers = new PlayerCombat2D[4];
    [HideInInspector] public EnemyCombat2D[] enemyCombatControllers = new EnemyCombat2D[4];

    // Private Variables
    private bool timeStart = true;

    public bool isAldenTurn = false;
    public bool isValricTurn = false;
    public bool isOsmirTurn = false;
    public bool isAssassinTurn = false;

    public int aldenIndex = -1;
    public int valricIndex = -1;
    public int osmirIndex = -1;
    public int assassinIndex = -1;

    private int currentSelectedTarget = 0;
    private int enemiesAlive = 0;
    private int playersAlive = 0;

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

        // Counting alive enemies and players
        for(int i = 0; i < 4; i++)
        {
            if(enemyCombatControllers[i] != null)
            {
                enemiesAlive++;
            }

            if(playerCombatControllers[i] != null)
            {
                playersAlive++;
            }
        }

        // Updating enemy markers on the time sliders
        UpdateEnemySliderMarkers();

        // Start a function that ticks turn count for all characters
        StartCoroutine(TickTime());
    }

    public IEnumerator TickTime()
    {
        while(timeStart == true)
        {
            // Check is all players are dead, DEFEAT
            if(playersAlive <= 0)
            {
                Debug.Log("Defeat");
                timeStart = false;
                StartCoroutine(DisplayDefeatPanel());
            }

            // Check if all enemies are dead, VICTORY
            if (enemiesAlive <= 0)
            {
                Debug.Log("Victory");
                timeStart = false;
                rewardsManager.LoadLevelCompletePanel(levelID > GameData.instance.combatCompletedID ? true : false);
                if (isRepeatableScene == false)
                {
                    GameData.instance.combatCompletedID = levelID;
                }
                StartCoroutine(DisplayVictoryPanel());
            }

            // Check if a character has reached turn cap
            for (int i = 0; i < 4; i++)
            {
                if(timeStart == true)
                {
                    if (playerCombatControllers[i] != null)
                    {
                        if(playerCombatControllers[i].isDefeated == false)
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

            // Tick time, mana and statuses for all characters on field
            if(timeStart == true)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (playerCombatControllers[i] != null)
                    {
                        if(playerCombatControllers[i].isDefeated == false)
                        {
                            playerCombatControllers[i].TickTurnCounter();
                            playerCombatControllers[i].TickStatuses();
                            playerCombatControllers[i].AddManaTick();
                        }
                        else
                        {
                            playerCombatControllers[i].turnCounter = 0;
                        }
                    }

                    if (enemyCombatControllers[i] != null)
                    {
                        if(enemyCombatControllers[i].isDefeated == false)
                        {
                            enemyCombatControllers[i].turnCounter += enemyCombatControllers[i].turnSpeed;
                            enemyCombatControllers[i].TickStatuses();
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

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ResumeTime()
    {
        timeStart = true;
        StartCoroutine(TickTime());
    }

    public IEnumerator DisplayVictoryPanel()
    {
        touchBlockerPanel.gameObject.SetActive(true);
        AudioManager.instance.PlayVictoryMusic();
        yield return new WaitForSeconds(0.8f);
        victoryPanelAnimator.SetTrigger("Enter");
    }

    public IEnumerator DisplayDefeatPanel()
    {
        touchBlockerPanel.gameObject.SetActive(true);
        AudioManager.instance.PlayDefeatMusic();
        yield return new WaitForSeconds(0.8f);
        defeatPanelAnimator.SetTrigger("Enter");
    }

    private void ShowUpdatedActionPanel()
    {
        // Turn off all previous statuses on player bar
        foreach (Transform child in playerStatusBar.transform)
        {
            child.gameObject.SetActive(false);
        }

        // Updating action panel with active character's stats
        if (isAldenTurn == true)
        {
            // updating HP and MP bars
            HPSlider.maxValue = playerCombatControllers[aldenIndex].GetMaxHealthValue();
            HPSlider.value = playerCombatControllers[aldenIndex].GetHealthValue();
            MPSlider.value = playerCombatControllers[aldenIndex].GetManaValue();

            // updating name label
            nameLabel.text = playerCombatControllers[aldenIndex].characterName;

            // Updating skill button availability
            playerCombatControllers[aldenIndex].UpdateSkillButtons();

            // Loading the active statuses
            foreach (Status status in playerCombatControllers[aldenIndex].aldenCombatController.activeStatuses)
            {
                // handling conditions
                if (status.statusName == Status.StatusName.Frailty)
                {
                    playerFrailty.gameObject.SetActive(true);
                    playerFrailty.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
                else if (status.statusName == Status.StatusName.Slowed)
                {
                    playerSlowed.gameObject.SetActive(true);
                    playerSlowed.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
                else if (status.statusName == Status.StatusName.Confusion)
                {
                    playerConfusion.gameObject.SetActive(true);
                    playerConfusion.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
                else if (status.statusName == Status.StatusName.Exposed)
                {
                    playerExposed.gameObject.SetActive(true);
                    playerExposed.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }

                // handling boons
                if (status.statusName == Status.StatusName.Might)
                {
                    playerMight.gameObject.SetActive(true);
                    playerMight.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
                else if (status.statusName == Status.StatusName.Focus)
                {
                    playerFocus.gameObject.SetActive(true);
                    playerFocus.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
                else if (status.statusName == Status.StatusName.Haste)
                {
                    playerHaste.gameObject.SetActive(true);
                    playerHaste.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
                else if (status.statusName == Status.StatusName.Barrier)
                {
                    playerBarrier.gameObject.SetActive(true);
                    playerBarrier.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
                else if (status.statusName == Status.StatusName.Clarity)
                {
                    playerClarity.gameObject.SetActive(true);
                    playerClarity.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
            }
        }
        // TODO: Add functionality for other characters


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

        // Updating & Showing the Selected Enemy Info Panel
        UpdateInfoPanel();
        enemyInfoPanelAnimator.SetTrigger("SlideIn");

        actionPanelAnimator.SetTrigger("SlideUp");

        // Disabling the Touch Blocker for the action panel
        actionPanelBlocker.gameObject.SetActive(false);
    }

    private void UpdateInfoPanel()
    {
        if(enemyCombatControllers[currentSelectedTarget] != null)
        {
            // Update the profile image
            if(enemyCombatControllers[currentSelectedTarget].enemyProfileSprite != null)
            {
                enemyProfileImageDisplay.sprite = enemyCombatControllers[currentSelectedTarget].enemyProfileSprite;
            }

            // Update the enemy status display if any status exists
            foreach(Transform child in enemyStatusBar.transform)
            {
                child.gameObject.SetActive(false);
            }

            foreach(Status status in enemyCombatControllers[currentSelectedTarget].activeStatuses)
            {
                // handling conditions
                if(status.statusName == Status.StatusName.Frailty)
                {
                    enemyFrailty.gameObject.SetActive(true);
                    enemyFrailty.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
                else if(status.statusName == Status.StatusName.Slowed)
                {
                    enemySlowed.gameObject.SetActive(true);
                    enemySlowed.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
                else if (status.statusName == Status.StatusName.Confusion)
                {
                    enemyConfusion.gameObject.SetActive(true);
                    enemyConfusion.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
                else if (status.statusName == Status.StatusName.Exposed)
                {
                    enemyExposed.gameObject.SetActive(true);
                    enemyExposed.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }

                // handling boons
                if (status.statusName == Status.StatusName.Might)
                {
                    enemyMight.gameObject.SetActive(true);
                    enemyMight.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
                else if (status.statusName == Status.StatusName.Focus)
                {
                    enemyFocus.gameObject.SetActive(true);
                    enemyFocus.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
                else if (status.statusName == Status.StatusName.Haste)
                {
                    enemyHaste.gameObject.SetActive(true);
                    enemyHaste.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
                else if (status.statusName == Status.StatusName.Barrier)
                {
                    enemyBarrier.gameObject.SetActive(true);
                    enemyBarrier.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
                else if (status.statusName == Status.StatusName.Clarity)
                {
                    enemyClarity.gameObject.SetActive(true);
                    enemyClarity.fillAmount = (float)status.statusCurrentDuration / status.statusDuration;
                }
            }
            
            // Update the enemy name
            enemyNameDisplay.text = enemyCombatControllers[currentSelectedTarget].enemyName;

            // Update the enemy actions
            enemyActionsDisplay.text = enemyCombatControllers[currentSelectedTarget].enemyActions;

            // Update the enemy health slider
            enemyHealthDisplay.maxValue = enemyCombatControllers[currentSelectedTarget].maxHealth;
            enemyHealthDisplay.value = enemyCombatControllers[currentSelectedTarget].health;

            // Update the health text
            enemyHealthValueDisplay.text = enemyCombatControllers[currentSelectedTarget].health.ToString();
        }
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
                if(playerCombatControllers[i].isDefeated == false)
                {
                    aldenTimeSlider.value = playerCombatControllers[aldenIndex].turnCounter;
                    valricTimeSlider.value = playerCombatControllers[valricIndex].turnCounter;
                    // similar for osmir
                    // similar for assassin girl
                }
                // player slider turn off on defeat managed in the ReportDeath() function.
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

    // Use to inform combatmanager about dead characters
    public void ReportDeath()
    {
        enemiesAlive--;
    }

    public void ReportDeath(string charName)
    {
        // handle player char death
        if(charName == "Alden")
        {
            playerCombatControllers[aldenIndex].isDefeated = true;
            aldenTimeSlider.gameObject.SetActive(false);
        }
        else if (charName == "Valric")
        {
            playerCombatControllers[valricIndex].isDefeated = true;
            // update this for valric too
        }
        else if (charName == "Osmir")
        {
            playerCombatControllers[osmirIndex].isDefeated = true;
        }
        else if (charName == "Assassin")
        {
            playerCombatControllers[assassinIndex].isDefeated = true;
        }

        playersAlive--;
    }

    // Updating enemy markers on the sliders
    private void UpdateEnemySliderMarkers()
    {
        for(int i = 0; i < 4; i++)
        {
            if(enemyCombatControllers[i] != null)
            {
                if(enemyCombatControllers[i].enemyMarkerSprite != null)
                {
                    enemySliderMarkers[i] = enemyCombatControllers[i].enemyMarkerSprite;
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

    public void HandleDealtAffliction(Status.StatusName statusName, int statusDuration)
    {
        if(statusName == Status.StatusName.Frailty)
        {
            // Check if the enemy already has the frailty status
            Status frailty = enemyCombatControllers[currentSelectedTarget].activeStatuses.Find(status => status.statusName == Status.StatusName.Frailty);

            // If enemy already has frailty status, then we just increase the duration
            if(frailty != null)
            {
                frailty.statusDuration += statusDuration;
                frailty.statusCurrentDuration += statusDuration;
            }
            else  // Otherwise, we initialise a new one
            {
                // Initialise the status
                frailty = new Status();
                frailty.statusName = Status.StatusName.Frailty;
                frailty.statusDuration = statusDuration;
                frailty.statusCurrentDuration = statusDuration;

                // Add the status to the enemy
                enemyCombatControllers[currentSelectedTarget].activeStatuses.Add(frailty);
            }
        }
        else if(statusName == Status.StatusName.Slowed)
        {
            // Check if the enemy already has the slowed status
            Status slowed = enemyCombatControllers[currentSelectedTarget].activeStatuses.Find(status => status.statusName == Status.StatusName.Slowed);

            // If enemy already has slowed status, then we just increase the duration
            if (slowed != null)
            {
                slowed.statusDuration += statusDuration;
                slowed.statusCurrentDuration += statusDuration;
            }
            else  // Otherwise, we initialise a new one
            {
                // Initialise the status
                slowed = new Status();
                slowed.statusName = Status.StatusName.Frailty;
                slowed.statusDuration = statusDuration;
                slowed.statusCurrentDuration = statusDuration;

                // Add the status to the enemy
                enemyCombatControllers[currentSelectedTarget].activeStatuses.Add(slowed);
            }
        }
        else if (statusName == Status.StatusName.Confusion)
        {
            // Check if the enemy already has the confusion status
            Status confusion = enemyCombatControllers[currentSelectedTarget].activeStatuses.Find(status => status.statusName == Status.StatusName.Confusion);

            // If enemy already has confusion status, then we just increase the duration
            if (confusion != null)
            {
                confusion.statusDuration += statusDuration;
                confusion.statusCurrentDuration += statusDuration;
            }
            else  // Otherwise, we initialise a new one
            {
                // Initialise the status
                confusion = new Status();
                confusion.statusName = Status.StatusName.Frailty;
                confusion.statusDuration = statusDuration;
                confusion.statusCurrentDuration = statusDuration;

                // Add the status to the enemy
                enemyCombatControllers[currentSelectedTarget].activeStatuses.Add(confusion);
            }
        }
        else if (statusName == Status.StatusName.Exposed)
        {
            // Check if the enemy already has the exposed status
            Status exposed = enemyCombatControllers[currentSelectedTarget].activeStatuses.Find(status => status.statusName == Status.StatusName.Exposed);

            // If enemy already has exposed status, then we just increase the duration
            if (exposed != null)
            {
                exposed.statusDuration += statusDuration;
                exposed.statusCurrentDuration += statusDuration;
            }
            else  // Otherwise, we initialise a new one
            {
                // Initialise the status
                exposed = new Status();
                exposed.statusName = Status.StatusName.Frailty;
                exposed.statusDuration = statusDuration;
                exposed.statusCurrentDuration = statusDuration;

                // Add the status to the enemy
                enemyCombatControllers[currentSelectedTarget].activeStatuses.Add(exposed);
            }
        }
    }

    // Blessings are granted to all allies in party
    public void HandleGrantedBlessing(Status.StatusName statusName, int statusDuration)
    {
        if(statusName == Status.StatusName.Might)
        {
            // Adding blessing to Alden
            if(aldenIndex != -1)
            {
                if(playerCombatControllers[aldenIndex].isDefeated == false)
                {
                    // Check if Alden already has the Might status
                    Status might = playerCombatControllers[aldenIndex].aldenCombatController.activeStatuses.Find(status => status.statusName == Status.StatusName.Might);

                    // If alden already has the Might status, we simply add it's duration
                    if (might != null)
                    {
                        might.statusDuration += statusDuration;
                        might.statusCurrentDuration += statusDuration;
                    }
                    else  // Otherwise, we initialise a new one
                    {
                        // Initialise the status
                        might = new Status();
                        might.statusName = Status.StatusName.Might;
                        might.statusDuration = statusDuration;
                        might.statusCurrentDuration = statusDuration;
                        might.isBlessing = true;

                        // Add the status to Alden
                        playerCombatControllers[aldenIndex].aldenCombatController.activeStatuses.Add(might);
                    }
                }
            }

            // Adding blessings to Valric - Same as above
            // Adding blessings to Osmir - Same as above
            // Adding blessings to Assassin - Same as above
        }
        else if(statusName == Status.StatusName.Focus)
        {
            // Adding blessing to Alden
            if (aldenIndex != -1)
            {
                if (playerCombatControllers[aldenIndex].isDefeated == false)
                {
                    // Check if Alden already has the Focus status
                    Status focus = playerCombatControllers[aldenIndex].aldenCombatController.activeStatuses.Find(status => status.statusName == Status.StatusName.Focus);

                    // If alden already has the Focus status, we simply add it's duration
                    if (focus != null)
                    {
                        focus.statusDuration += statusDuration;
                        focus.statusCurrentDuration += statusDuration;
                    }
                    else  // Otherwise, we initialise a new one
                    {
                        // Initialise the status
                        focus = new Status();
                        focus.statusName = Status.StatusName.Focus;
                        focus.statusDuration = statusDuration;
                        focus.statusCurrentDuration = statusDuration;
                        focus.isBlessing = true;

                        // Add the status to Alden
                        playerCombatControllers[aldenIndex].aldenCombatController.activeStatuses.Add(focus);
                    }
                }
            }

            // Adding blessings to Valric - Same as above
            // Adding blessings to Osmir - Same as above
            // Adding blessings to Assassin - Same as above
        }
        else if (statusName == Status.StatusName.Haste)
        {
            // Adding blessing to Alden
            if (aldenIndex != -1)
            {
                if (playerCombatControllers[aldenIndex].isDefeated == false)
                {
                    // Check if Alden already has the Haste status
                    Status haste = playerCombatControllers[aldenIndex].aldenCombatController.activeStatuses.Find(status => status.statusName == Status.StatusName.Haste);

                    // If alden already has the Haste status, we simply add it's duration
                    if (haste != null)
                    {
                        haste.statusDuration += statusDuration;
                        haste.statusCurrentDuration += statusDuration;
                    }
                    else  // Otherwise, we initialise a new one
                    {
                        // Initialise the status
                        haste = new Status();
                        haste.statusName = Status.StatusName.Haste;
                        haste.statusDuration = statusDuration;
                        haste.statusCurrentDuration = statusDuration;
                        haste.isBlessing = true;

                        // Add the status to Alden
                        playerCombatControllers[aldenIndex].aldenCombatController.activeStatuses.Add(haste);
                    }
                }
            }

            // Adding blessings to Valric - Same as above
            // Adding blessings to Osmir - Same as above
            // Adding blessings to Assassin - Same as above
        }
        else if (statusName == Status.StatusName.Clarity)
        {
            // Adding blessing to Alden
            if (aldenIndex != -1)
            {
                if (playerCombatControllers[aldenIndex].isDefeated == false)
                {
                    // Check if Alden already has the Clarity status
                    Status clarity = playerCombatControllers[aldenIndex].aldenCombatController.activeStatuses.Find(status => status.statusName == Status.StatusName.Clarity);

                    // If alden already has the Clarity status, we simply add it's duration
                    if (clarity != null)
                    {
                        clarity.statusDuration += statusDuration;
                        clarity.statusCurrentDuration += statusDuration;
                    }
                    else  // Otherwise, we initialise a new one
                    {
                        // Initialise the status
                        clarity = new Status();
                        clarity.statusName = Status.StatusName.Clarity;
                        clarity.statusDuration = statusDuration;
                        clarity.statusCurrentDuration = statusDuration;
                        clarity.isBlessing = true;

                        // Add the status to Alden
                        playerCombatControllers[aldenIndex].aldenCombatController.activeStatuses.Add(clarity);
                    }
                }
            }

            // Adding blessings to Valric - Same as above
            // Adding blessings to Osmir - Same as above
            // Adding blessings to Assassin - Same as above
        }
        else if (statusName == Status.StatusName.Barrier)
        {
            // Adding blessing to Alden
            if (aldenIndex != -1)
            {
                if (playerCombatControllers[aldenIndex].isDefeated == false)
                {
                    // Check if Alden already has the Barrier status
                    Status barrier = playerCombatControllers[aldenIndex].aldenCombatController.activeStatuses.Find(status => status.statusName == Status.StatusName.Barrier);

                    // If alden already has the Barrier status, we simply add it's duration
                    if (barrier != null)
                    {
                        barrier.statusDuration += statusDuration;
                        barrier.statusCurrentDuration += statusDuration;
                    }
                    else  // Otherwise, we initialise a new one
                    {
                        // Initialise the status
                        barrier = new Status();
                        barrier.statusName = Status.StatusName.Clarity;
                        barrier.statusDuration = statusDuration;
                        barrier.statusCurrentDuration = statusDuration;
                        barrier.isBlessing = true;

                        // Add the status to Alden
                        playerCombatControllers[aldenIndex].aldenCombatController.activeStatuses.Add(barrier);
                    }
                }
            }

            // Adding blessings to Valric - Same as above
            // Adding blessings to Osmir - Same as above
            // Adding blessings to Assassin - Same as above
        }
    }

    // Strips (removes) the first blessing it finds in the enemies' active statuses
    public void StripEnemyBlessing()
    {
        Status blessing = enemyCombatControllers[currentSelectedTarget].activeStatuses.Find(status => status.isBlessing == true);

        if(blessing != null)
        {
            enemyCombatControllers[currentSelectedTarget].activeStatuses.Remove(blessing);
        }
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
            // Play attack animation according to selected target
            playerCombatControllers[valricIndex].DoAction("Attack", currentSelectedTarget);
        }

        // When attack button is pressed during Osmir's turn
        if (isOsmirTurn)
        {

        }

        // When attack button is pressed during Assassin's Turn
        if (isAssassinTurn)
        {

        }


        AfterButtonPressHandler();
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
        // When Skill 1 button is pressed during Alden's turn
        if (isAldenTurn)
        {
            // Play Skill 1 animation according to selected target
            playerCombatControllers[aldenIndex].DoAction("Skill1", currentSelectedTarget);
        }

        // When Skill 1 button is pressed during Valric's turn
        if (isValricTurn)
        {

        }

        // When Skill 1 button is pressed during Osmir's turn
        if (isOsmirTurn)
        {

        }

        // When Skill 1 button is pressed during Assassin's Turn
        if (isAssassinTurn)
        {

        }

        // Hide the skill buttons (to reset for next time the drawer shows up)
        actionPanelAnimator.SetTrigger("SkillHide");

        AfterButtonPressHandler();
    }

    public void OnSkill2Button()
    {
        // When Skill 2 button is pressed during Alden's turn
        if (isAldenTurn)
        {
            // Play Skill 2 animation according to selected target
            playerCombatControllers[aldenIndex].DoAction("Skill2", currentSelectedTarget);
        }

        // When Skill 1 button is pressed during Valric's turn
        if (isValricTurn)
        {

        }

        // When Skill 1 button is pressed during Osmir's turn
        if (isOsmirTurn)
        {

        }

        // When Skill 1 button is pressed during Assassin's Turn
        if (isAssassinTurn)
        {

        }

        // Hide the skill buttons (to reset for next time the drawer shows up)
        actionPanelAnimator.SetTrigger("SkillHide");

        AfterButtonPressHandler();
    }

    public void OnSkill3Button()
    {
        // When Skill 3 button is pressed during Alden's turn
        if (isAldenTurn)
        {
            // Play Skill 3 animation according to selected target
            playerCombatControllers[aldenIndex].DoAction("Skill3", currentSelectedTarget);
        }

        // When Skill 1 button is pressed during Valric's turn
        if (isValricTurn)
        {

        }

        // When Skill 1 button is pressed during Osmir's turn
        if (isOsmirTurn)
        {

        }

        // When Skill 1 button is pressed during Assassin's Turn
        if (isAssassinTurn)
        {

        }

        // Hide the skill buttons (to reset for next time the drawer shows up)
        actionPanelAnimator.SetTrigger("SkillHide");

        AfterButtonPressHandler();
    }

    public void OnDefendButton()
    {
        // When defend button is pressed during Alden's turn
        if (isAldenTurn)
        {
            // Play defend animation
            playerCombatControllers[aldenIndex].DoAction("Defend", currentSelectedTarget);
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


        AfterButtonPressHandler();
    }

    private void AfterButtonPressHandler()
    {
        // Turn on action panel touch blocker
        actionPanelBlocker.gameObject.SetActive(true);

        enemyCombatControllers[currentSelectedTarget].selectMarker.gameObject.SetActive(false);
        actionPanelAnimator.SetTrigger("SlideDown");
        enemyInfoPanelAnimator.SetTrigger("SlideOut");
        
        // Turn off all statuses on enemy bar
        foreach(Transform child in enemyStatusBar.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void OnContinueButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadNextSceneName);
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

        UpdateInfoPanel();
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

        UpdateInfoPanel();
    }
}
