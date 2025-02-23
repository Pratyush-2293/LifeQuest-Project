using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AldenCombat2D : MonoBehaviour
{
    // Player Stats - Load them on awake
    public int level = 1;
    public int vitality = 5;
    public int defense = 5;
    public int strength = 5;
    private int critRate = 10; // x100 to get % value

    // Common stats - need to be calculated on awake
    public int health = 100;
    public int mana = 100;

    // Attacks Scaling
    [Header("Attacks & Skills Scaling")]
    public int attackScaling = 1;
    public int skill1Scaling = 1;
    public int skill2Scaling = 1;
    public int skill3Scaling = 1;
    public int defendScaling = 1;

    // Alden Components
    public Animator aldenAnimator = null;

    // Private Variables
    private int damageToDo = 1;
    private bool isCritical = false;
    private double randomNumber = 0;

    private void Start()
    {
        
    }

    public void AldenAttack(int targetPosition)
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
        Debug.Log(randomNumber);
        if (randomNumber <= critRate)
        {
            isCritical = true;
            damageToDo *= 2;
        }

        // Play the attack animation
        if (targetPosition == 0)
        {
            aldenAnimator.SetTrigger("Attack_1-1");
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

    public void DealDamage()
    {
        // Tell combat manager to deal damage to active enemy
        Debug.Log(isCritical);
        CombatManager.instance.HandleDealtDamage(damageToDo, isCritical);
    }
}
