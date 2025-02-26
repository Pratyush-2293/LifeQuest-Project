using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyCombat2D : MonoBehaviour
{
    public int turnCounter = 10;
    public int turnSpeed = 1;
    public int characterPosition = 1;
    public bool isPlayerCharacter = false;
    public int health = 100;
    public int maxHealth = 100;
    public Slider healthSlider = null;
    public GameObject defendingIcon = null;
    public GameObject selectMarker = null;
    public bool isDefeated = false;
    public bool isTaunted = false;
    public Animator enemyAnimator = null;

    // Skills Data
    public int attackDamage = 10;
    public int attackTimeCost = 70;
    public int defendTimeCost = 50;
    public float defendStartHealthPercent = 0.3f;
    public int defendChancePercent = 60;

    // Damage Popup Components
    public Animator normalDamageAnimator = null;
    public Animator criticalDamageAnimator = null;
    public TMP_Text normalDamageText = null;
    public TMP_Text criticalDamageText = null;

    // Private Variables
    private int damageToDo = 0;
    private int selectedTarget = 0;
    private int defendStartHealth = 0;
    private bool isDefending = false;

    private void Start()
    {
        healthSlider.maxValue = maxHealth;
        defendStartHealth = (int)(maxHealth * defendStartHealthPercent);
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
            isDefeated = true;
            enemyAnimator.SetTrigger("Death");
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
        damageToDo = attackDamage;

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
    }

    public void RequestResumeTime()
    {
        CombatManager.instance.ResumeTime();
    }

    public void Action_TakeDamage(int incomingDamage, bool isCritical)
    {
        // play damage animation
        enemyAnimator.SetTrigger("TakeDamage");

        // Reduce damage by 50% if defending
        if(isDefending == true)
        {
            incomingDamage = (int)(incomingDamage * 0.5f);
            isDefending = false;
            defendingIcon.gameObject.SetActive(false);
        }

        // Calculate reductions from blessings if any

        // play damage number animation
        if (isCritical == true)
        {

            criticalDamageText.text = incomingDamage.ToString();
            criticalDamageAnimator.SetTrigger("CritDamagePopup");
        }
        else
        {
            normalDamageText.text = incomingDamage.ToString();
            normalDamageAnimator.SetTrigger("DamagePopup");
        }

        // reduce enemy health
        health -= incomingDamage;

        UpdateSelfState();
    }

}
