using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AldenCombat2D : MonoBehaviour
{
    // Player Stats - Load them on awake
    public int level = 1;
    public int vitality = 5;
    public int defense = 5;
    public int strength = 5;
    private int critRate = 70; // x100 to get % value

    // Common stats - need to be calculated on awake
    public int maxHealth = 100;
    public int health = 100;
    public int mana = 100;

    // Attacks Scaling
    [Header("Attacks & Skills Scaling")]
    public int attackScaling = 1;
    public int skill1Scaling = 1;
    public int skill2Scaling = 1;
    public int skill3Scaling = 1;
    public int defendScaling = 1;

    // Actions info
    public int attackTimeCost = 30;
    public int defendTimeCost = 30;
    public int skill1TimeCost = 30;
    public int skill2TimeCost = 30;
    public int skill3TimeCost = 30;

    // Alden Components
    public Animator aldenAnimator = null;
    public Animator normalDamageAnimator = null;
    public TMP_Text normalDamageText = null;

    // Private Variables
    private int damageToDo = 1;
    private bool isCritical = false;
    private double randomNumber = 0;

    private void Start()
    {
        
    }

    public void AldenAttack(int targetPosition, int selfPosition)
    {
        isCritical = false;

        // CALCULATING DAMAGE
        // Scaling damage based on strength and the skill's scaling mulitplier
        damageToDo = strength * attackScaling;
        // Scaling damage based on active blessings
        // Scaling damage based on active afflictions
        // Checking to see if this attack will be a critical hit
        Random.InitState(System.DateTime.Now.Millisecond);
        randomNumber = Random.Range(1, 101);
        if (randomNumber <= critRate)
        {
            isCritical = true;
            damageToDo *= 2;
        }

        // Play the attack animation
        if(selfPosition == 1)
        {
            if (targetPosition == 0)
            {
                aldenAnimator.SetTrigger("Attack_1-1");  // Deal the damage by calling DealDamage() using animation event
            }
            else if (targetPosition == 1)
            {
                aldenAnimator.SetTrigger("Attack_1-2");
            }
            else if (targetPosition == 2)
            {
                aldenAnimator.SetTrigger("Attack_1-3");
            }
            else if (targetPosition == 3)
            {
                aldenAnimator.SetTrigger("Attack_1-4");
            }
        }
        else if(selfPosition == 2)
        {
            // same as above
        }
        else if (selfPosition == 3)
        {
            // same as above
        }
        else if (selfPosition == 4)
        {
            // same as above
        }

    }

    public void AldenTakeDamage(int incomingDamage)
    {
        // play damage animation
        aldenAnimator.SetTrigger("AldenHurt");

        // calculate def reductions
        incomingDamage -= defense;
        // calculate blessings reductions, if any

        // play damage number animation
        normalDamageText.text = incomingDamage.ToString();
        normalDamageAnimator.SetTrigger("DamagePopup");

        // reduce player health
        health -= incomingDamage;
    }

    public void DealDamage()
    {
        // Tell combat manager to deal damage to active enemy
        CombatManager.instance.HandleDealtDamage(damageToDo, isCritical);
    }

    public void RequestResumeTime()
    {
        CombatManager.instance.ResumeTime();
    }
}
