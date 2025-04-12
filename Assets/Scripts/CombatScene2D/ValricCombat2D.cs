using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class ValricCombat2D : MonoBehaviour
{
    // Player Stats - Load them on awake
    public int level = 1;
    public int vitality = 5;
    public int defense = 5;
    public int strength = 5;
    private int critRate = 50; // x100 to get % value

    // Common stats - need to be calculated on awake
    public int maxHealth = 100;
    public int health = 100;
    public int mana = 100;

    // List of Active Statuses
    public List<Status> activeStatuses = new List<Status>();

    // Attacks Scaling
    [Header("Attacks & Skills Scaling")]
    public float attackScaling = 1;
    public float skill1Scaling = 1.5f;
    public float skill2Scaling = 1;
    public float skill3Scaling = 1.5f;
    public float defendScaling = 1;

    // Actions info
    public int attackTimeCost = 30;
    public int defendTimeCost = 30;
    public int skill1TimeCost = 50;
    public int skill2TimeCost = 30;
    public int skill3TimeCost = 30;

    // Skill Mana Costs
    public int skill1ManaCost = 20;
    public int skill2ManaCost = 30;
    public int skill3ManaCost = 40;

    // Skill Unlock Levels
    public int skill1UnlockLevel = 1;
    public int skill2UnlockLevel = 2;
    public int skill3UnlockLevel = 3;

    // Alden Components
    public Animator valricAnimator = null;
    public Transform damageFloaterSpawnpoint;
    public GameObject normalDamageFloater;
    public GameObject hitFXObject = null;
    public Animator shieldAuraAnimator = null;

    // Private Variables
    private int dealDamageSequenceCounter = 1;
    private int damageToDo_1 = 1;
    private int damageToDo_2 = 1;
    private int damageToDo_3 = 1;
    private bool isCritical_1 = false;
    private bool isCritical_2 = false;
    private bool isCritical_3 = false;

    private double randomNumber_1 = 0;
    private Animator hitFXAnimator = null;
    private bool isDefending = false;

    private void Start()
    {
        hitFXAnimator = hitFXObject.gameObject.GetComponent<Animator>();
    }

    public void ValricAttack(int targetPosition, int selfPosition)
    {
        isCritical_1 = false;

        // CALCULATING DAMAGE

        // Scaling damage based on strength and the skill's scaling mulitplier
        damageToDo_1 = (int)(strength * attackScaling);

        // Scaling damage based on active afflictions
        if (activeStatuses.Any(status => status.statusName == Status.StatusName.Frailty)) // If affected by frailty, reduce outgoing damage by 25%
        {
            damageToDo_1 -= (int)(damageToDo_1 * 0.25f);
        }

        // Scaling damage based on active blessings
        if (activeStatuses.Any(status => status.statusName == Status.StatusName.Might)) // If affected by might, increase outgoing damage by 25%
        {
            damageToDo_1 += (int)(damageToDo_1 * 0.25f);
        }

        // Checking to see if this attack will be a critical hit
        randomNumber_1 = Random.Range(1, 101);
        if (randomNumber_1 <= critRate)
        {
            isCritical_1 = true;
            damageToDo_1 *= 2;
        }

        // Play the attack animation
        if (selfPosition == 1)
        {
            if (targetPosition == 0)
            {
                valricAnimator.SetTrigger("Attack_1-1");  // Deal the damage by calling DealDamage() using animation event
            }
            else if (targetPosition == 1)
            {
                valricAnimator.SetTrigger("Attack_1-2");
            }
            else if (targetPosition == 2)
            {
                valricAnimator.SetTrigger("Attack_1-3");
            }
            else if (targetPosition == 3)
            {
                valricAnimator.SetTrigger("Attack_1-4");
            }
        }
        else if (selfPosition == 2)
        {
            if (targetPosition == 0)
            {
                valricAnimator.SetTrigger("Attack_2-1");  // Deal the damage by calling DealDamage() using animation event
            }
            else if (targetPosition == 1)
            {
                valricAnimator.SetTrigger("Attack_2-2");
            }
            else if (targetPosition == 2)
            {
                valricAnimator.SetTrigger("Attack_2-3");
            }
            else if (targetPosition == 3)
            {
                valricAnimator.SetTrigger("Attack_2-4");
            }
        }
        else if (selfPosition == 3)
        {
            // same as above
        }
        else if (selfPosition == 4)
        {
            // same as above
        }

        // time cost will be added in playercombat
    }

    public void ValricDefend()
    {
        valricAnimator.SetTrigger("ValricDefend");
        isDefending = true;
        shieldAuraAnimator.SetTrigger("ShieldAuraActive");
        // time cost will be added in playercombat
    }

    public void ValricTakeDamage(int incomingDamage)
    {
        // play damage animation
        valricAnimator.SetTrigger("ValricHurt");

        // play hit FX
        int randomRotation = Random.Range(0, 45);
        hitFXObject.transform.Rotate(0f, 0f, randomRotation);
        hitFXAnimator.SetTrigger("HitEffect");

        // calculate def reductions
        incomingDamage -= defense;

        // calculate incoming damage by status modifiers

        // Calculating damage increase due to afflictions
        if (activeStatuses.Any(status => status.statusName == Status.StatusName.Exposed)) // If affected by exposed, increase incoming damage by 25%
        {
            incomingDamage += (int)(incomingDamage * 0.25);
        }

        // Calculating damage reductions due to blessings
        if (activeStatuses.Any(status => status.statusName == Status.StatusName.Barrier)) // If affected by barrier, reduce incoming damage by 25%
        {
            incomingDamage -= (int)(incomingDamage * 0.25);
        }

        // Reduce damage if defending
        if (isDefending == true)
        {
            incomingDamage -= (int)(incomingDamage * 0.5f);
            isDefending = false;
            shieldAuraAnimator.SetTrigger("ShieldAuraIdle");
        }

        // play damage number animation
        GameObject normalDamageObject = Instantiate(normalDamageFloater, damageFloaterSpawnpoint);
        normalDamageObject.GetComponent<TMP_Text>().text = incomingDamage.ToString();

        // reduce player health
        health -= incomingDamage;

        CheckDeathCondition();
    }

    private void CheckDeathCondition()
    {
        if (health < 0)
        {
            health = 0;
        }

        // Death Condition
        if (health <= 0)
        {
            //isDefeated = true;
            valricAnimator.SetTrigger("ValricDeath");
            CombatManager.instance.ReportDeath("Valric");
        }
    }

    public void DealDamage()
    {
        // Tell combat manager to deal damage to active enemy
        if (dealDamageSequenceCounter == 1)
        {
            CombatManager.instance.HandleDealtDamage(damageToDo_1, isCritical_1);
            dealDamageSequenceCounter++;

            if (isCritical_1)
            {
                CameraShake.instance.StartShake(1);
                HitStop.instance.StartHitStop(0);
            }
            else
            {
                CameraShake.instance.StartShake(0);
            }
        }
        else if (dealDamageSequenceCounter == 2)
        {
            CombatManager.instance.HandleDealtDamage(damageToDo_2, isCritical_2);
            dealDamageSequenceCounter++;

            if (isCritical_2)
            {
                CameraShake.instance.StartShake(1);
                HitStop.instance.StartHitStop(0);
            }
            else
            {
                CameraShake.instance.StartShake(0);
            }
        }
        else if (dealDamageSequenceCounter == 3)
        {
            CombatManager.instance.HandleDealtDamage(damageToDo_3, isCritical_3);
            dealDamageSequenceCounter++;

            if (isCritical_3)
            {
                CameraShake.instance.StartShake(1);
                HitStop.instance.StartHitStop(0);
            }
            else
            {
                CameraShake.instance.StartShake(0);
            }
        }
    }

    private void ResetDamageSequence()
    {
        dealDamageSequenceCounter = 1;
    }

    public void RemoveEnemyBlessing()
    {
        CombatManager.instance.StripEnemyBlessing();
    }

    public void RequestResumeTime()
    {
        ResetDamageSequence();
        CombatManager.instance.ResumeTime();
        CombatManager.instance.isValricTurn = false;
    }

    private void PlaySoundEffect(string sfxName)
    {
        AudioManager.instance.PlaySound(sfxName);
    }


    // TODO: Edit this later to enable changing the button sprite based on the case
    public void EnableSkillButtons()
    {
        // Checking for skill button 1
        if (level >= skill1UnlockLevel)
        {
            if (mana >= skill1ManaCost)
            {
                CombatManager.instance.skill1Button.interactable = true;
            }
            else
            {
                CombatManager.instance.skill1Button.interactable = false;
            }
        }
        else
        {
            CombatManager.instance.skill1Button.interactable = false;
        }

        // Checking for skill button 2
        if (level >= skill2UnlockLevel)
        {
            if (mana >= skill2ManaCost)
            {
                CombatManager.instance.skill2Button.interactable = true;
            }
            else
            {
                CombatManager.instance.skill2Button.interactable = false;
            }
        }
        else
        {
            CombatManager.instance.skill2Button.interactable = false;
        }

        // Checking for skill button 3
        if (level >= skill3UnlockLevel)
        {
            if (mana >= skill3ManaCost)
            {
                CombatManager.instance.skill3Button.interactable = true;
            }
            else
            {
                CombatManager.instance.skill3Button.interactable = false;
            }
        }
        else
        {
            CombatManager.instance.skill3Button.interactable = false;
        }
    }
}
