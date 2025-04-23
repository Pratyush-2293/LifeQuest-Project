using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class EnemyCombat2D : MonoBehaviour
{
    [Header("Enemy Info")]
    [Space(5)]
    public string enemyName = null;
    public Sprite enemyMarkerSprite = null;
    public Sprite enemyProfileSprite = null;
    public string enemyActions = null;

    [Header("Enemy Skills Config")]
    [Space(5)]
    public int attackDamage = 10;
    public int attackTimeCost = 70;
    [Space(10)]
    public int defendTimeCost = 50;
    public float defendStartHealthPercent = 0.3f;
    public int defendChancePercent = 60;

    [Header("Enemy Core Data")]
    [Space(5)]
    public int characterPosition = 1;
    [Space(10)]
    public int health = 100;
    public int maxHealth = 100;
    [Space(10)]
    public int turnCounter = 10;
    public int turnSpeed = 1;
    [Space(10)]
    public bool isPlayerCharacter = false;
    
    [Header("Enemy Components")]
    [Space(5)]
    public Slider healthSlider = null;
    public GameObject defendingIcon = null;
    [Space(10)]
    public GameObject selectMarker = null;
    [Space(10)]
    public Animator enemyAnimator = null;

    [Header("Enemy Status")]
    [Space(5)]
    public bool isDefeated = false;
    public bool isTaunted = false;

    [Header("Damage Floater Components")]
    [Space(5)]
    public Transform damageFloaterSpawnPoint;
    [Space(5)]
    public GameObject normalDamageFloater;
    public GameObject criticalDamageFloater;
    
    [Header("Hit VFX Components")]
    [Space(5)]
    public GameObject hitFXObject = null;

    // Private Variables
    private int damageToDo = 0;
    private int selectedTarget = 0;
    private int defendStartHealth = 0;
    private bool isDefending = false;
    private Animator hitFXAnimator = null;

    public List<Status> activeStatuses = new List<Status>();

    private void Start()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        defendStartHealth = (int)(maxHealth * defendStartHealthPercent);

        hitFXAnimator = hitFXObject.gameObject.GetComponent<Animator>();
    }
    public void UpdateSelfState()
    {
        if(health < 0)
        {
            health = 0;
        }
        healthSlider.value = health;

        // Death Condition
        if(health <= 0)
        {
            if (isDefeated == false)
            {
                CombatManager.instance.ReportDeath();
                enemyAnimator.SetTrigger("Death");
                selectMarker.gameObject.SetActive(false);
            }

            isDefeated = true;
        }
    }

    public void Action_PlayTurn()
    {
        // choose between defend and attack based on self health
        if (health <= defendStartHealth)
        {
            if(isDefending == false)
            {
                Random.InitState(System.DateTime.Now.Millisecond);
                int randomNumber = Random.Range(1, 101);
                if (randomNumber < defendChancePercent)
                {
                    // Play defending action
                    Action_Defend();
                }
                else
                {
                    Action_Attack();
                }
            }
            else
            {
                Action_Attack();  // attack
            }
        }
        else
        {
            Action_Attack();
        }
    }

    private void Action_Defend()
    {
        enemyAnimator.SetTrigger("Defend");
        isDefending = true;
        defendingIcon.gameObject.SetActive(true);
        AddTimeCost(defendTimeCost);
    }

    private void Action_Attack()
    {
        // CALCULATE DAMAGE TO DO
        // Can implement attack patterns here later, right now we only have 1 basic attack.

        //First reading alden's defense and subtracting it from the attack damaage to get the flat damage to do.
        int aldenDef = CombatManager.instance.playerCombatControllers[0].aldenCombatController.defense;
        damageToDo = attackDamage - aldenDef;

        // Reduce damage output based on active afflictions (conditions)
        if(activeStatuses.Any(status => status.statusName == Status.StatusName.Frailty)) // Reduce outgoing damage by 25% if affected by frailty
        {
            damageToDo -= (int)(damageToDo * 0.25f);
        }

        // Increase damage output based on active blessings (boons)
        if (activeStatuses.Any(status => status.statusName == Status.StatusName.Might)) // Increase outgoing damage by 25% if affected by might
        {
            damageToDo += (int)(damageToDo * 0.25f);
        }

        // check available player character
        int availableCharacters = 0;
        for (int i = 0; i < 4; i++)
        {
            if(CombatManager.instance.playerCombatControllers[i] != null)
            {
                if(CombatManager.instance.playerCombatControllers[i].isDefeated == false)
                {
                    availableCharacters++;
                }
            }
        }

        // choose a random character from available characters , unless taunted
        if(availableCharacters == 1)  // if only one char on field, i.e Alden only
        {
            selectedTarget = CombatManager.instance.aldenIndex;
        }
        else  // When more than 1 char on field
        {
            if (isTaunted == false)  // if not taunted, choose a random target
            {
                Random.InitState(System.DateTime.Now.Millisecond);
                selectedTarget = Random.Range(1, availableCharacters + 1);
            }
            else  // if taunted, choose valric as target
            {
                selectedTarget = CombatManager.instance.valricIndex;
            }
        }

        // Play attack animations & deal damage to target character
        int targetPosition = CombatManager.instance.playerCombatControllers[selectedTarget].characterPosition;

        if(characterPosition == 1)
        {
            switch (targetPosition)
            {
                case 1:
                    enemyAnimator.SetTrigger("Attack_1-1");
                    break;
                case 2:
                    enemyAnimator.SetTrigger("Attack_1-2");
                    break;
                case 3:
                    enemyAnimator.SetTrigger("Attack_1-3");
                    break;
                case 4:
                    enemyAnimator.SetTrigger("Attack_1-4");
                    break;
            }
        }
        else if(characterPosition == 2)
        {
            switch (targetPosition)
            {
                case 1:
                    enemyAnimator.SetTrigger("Attack_2-1");
                    break;
                case 2:
                    enemyAnimator.SetTrigger("Attack_2-2");
                    break;
                case 3:
                    enemyAnimator.SetTrigger("Attack_2-3");
                    break;
                case 4:
                    enemyAnimator.SetTrigger("Attack_2-4");
                    break;
            }
        }
        else if (characterPosition == 3)
        {
            switch (targetPosition)
            {
                case 1:
                    enemyAnimator.SetTrigger("Attack_3-1");
                    break;
                case 2:
                    enemyAnimator.SetTrigger("Attack_3-2");
                    break;
                case 3:
                    enemyAnimator.SetTrigger("Attack_3-3");
                    break;
                case 4:
                    enemyAnimator.SetTrigger("Attack_3-4");
                    break;
            }
        }
        else if (characterPosition == 4)
        {
            switch (targetPosition)
            {
                case 1:
                    enemyAnimator.SetTrigger("Attack_4-1");
                    break;
                case 2:
                    enemyAnimator.SetTrigger("Attack_4-2");
                    break;
                case 3:
                    enemyAnimator.SetTrigger("Attack_4-3");
                    break;
                case 4:
                    enemyAnimator.SetTrigger("Attack_4-4");
                    break;
            }
        }

        // Add time cost to turnCounter
        // will need to appropriately add cost based on skill performed, will come back to this later.
        AddTimeCost(attackTimeCost);
    }

    private void AddTimeCost(int timeCost)
    {
        turnCounter = 100 - timeCost;
    }

    public void DealDamage()
    {
        // Tell combat manager to deal damage to active enemy
        CombatManager.instance.HandleDealtDamage(damageToDo, selectedTarget);

        CameraShake.instance.StartShake(0);
    }

    public void RequestResumeTime()
    {
        CombatManager.instance.ResumeTime();
    }

    public void Action_TakeDamage(int incomingDamage, bool isCritical)
    {
        // play damage animation
        enemyAnimator.SetTrigger("TakeDamage");

        // play hit FX
        int randomRotation = Random.Range(0, 45);
        hitFXObject.transform.Rotate(0f, 0f, randomRotation);
        hitFXAnimator.SetTrigger("HitEffect");

        // Adjust incoming damage based on active statuses

        // Handling afflictions
        if(activeStatuses.Any(status => status.statusName == Status.StatusName.Exposed))
        {
            incomingDamage += (int)(incomingDamage * 0.25f);
        }

        // Handling blessings
        if(activeStatuses.Any(status => status.statusName == Status.StatusName.Barrier))
        {
            incomingDamage -= (int)(incomingDamage * 0.25f);
        }


        // Reduce damage by 50% if defending
        if (isDefending == true)
        {
            incomingDamage -= (int)(incomingDamage * 0.5f);
            isDefending = false;
            defendingIcon.gameObject.SetActive(false);
        }

        // play damage number animation
        if (isCritical == true)
        {
            // Instantiate a critical damage floater
            GameObject critDamageObject = Instantiate(criticalDamageFloater, damageFloaterSpawnPoint);

            // Setting the damage text
            critDamageObject.transform.GetChild(0).GetComponent<TMP_Text>().text = incomingDamage.ToString();
        }
        else
        {
            // Instantiate the normal damage floater
            GameObject normalDamageObject = Instantiate(normalDamageFloater, damageFloaterSpawnPoint);

            // Setting the damage text
            normalDamageObject.GetComponent<TMP_Text>().text = incomingDamage.ToString();
        }

        // reduce enemy health
        health -= incomingDamage;

        UpdateSelfState();
    }

    private void PlaySoundEffect(string sfxName)
    {
        AudioManager.instance.PlaySound(sfxName);
    }

    public void TickStatuses()
    {
        foreach(Status status in activeStatuses)
        {
            status.statusCurrentDuration--;
        }

        activeStatuses.RemoveAll(status => status.statusCurrentDuration <= 0);
    }
}
