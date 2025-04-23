using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class AldenCombat2D : MonoBehaviour
{
    [Header("Character Stats")]
    [Space(5)]
    public bool overrideStats = false;
    [Space(10)]
    // Player Stats - Load them on awake
    public int level = 1;
    public int defense = 5;
    public int attack = 5;
    public int critRate = 50; 
    public int critDamage = 100;
    [Space(10)]
    // Common stats - need to be calculated on awake
    public int maxHealth = 100;
    public int health = 100;
    public int mana = 100;

    // List of Active Statuses
    [HideInInspector] public List<Status> activeStatuses = new List<Status>();

    // Attacks Scaling
    [Header("Attacks & Skills Scaling")]
    [Space(5)]
    public float attackScaling = 1;
    public float skill1Scaling = 1.5f;
    public float skill2Scaling = 1;
    public float skill3Scaling = 1.5f;
    public float defendScaling = 1;

    // Actions info
    [Header("Actions Time Cost")]
    [Space(5)]
    public int attackTimeCost = 30;
    public int defendTimeCost = 30;
    public int skill1TimeCost = 50;
    public int skill2TimeCost = 30;
    public int skill3TimeCost = 30;

    // Skill Mana Costs
    [Header("Skills Mana Cost")]
    [Space(5)]
    public int skill1ManaCost = 20;
    public int skill2ManaCost = 30;
    public int skill3ManaCost = 40;

    // Skill Unlock Levels
    [Header("Skill Unlock Levels")]
    [Space(5)]
    public int skill1UnlockLevel = 1;
    public int skill2UnlockLevel = 2;
    public int skill3UnlockLevel = 3;

    // Alden Components
    [Header("Alden Components")]
    [Space(5)]
    public Animator aldenAnimator = null;
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
    private double randomNumber_2 = 0;
    private double randomNumber_3 = 0;
    private Animator hitFXAnimator = null;
    private bool isDefending = false;

    private void Start()
    {
        hitFXAnimator = hitFXObject.gameObject.GetComponent<Animator>();

        if(overrideStats == false)
        {
            level = GameData.instance.aldenLevel;
            defense = GameData.instance.aldenDEF;
            attack = GameData.instance.aldenATK;
            critRate = GameData.instance.aldenCR;
            critDamage = GameData.instance.aldenCD;
        }
    }

    public void AldenAttack(int targetPosition, int selfPosition)
    {
        isCritical_1 = false;

        // CALCULATING DAMAGE

        // Scaling damage based on strength and the skill's scaling mulitplier
        damageToDo_1 = (int)(attack * attackScaling);

        // Scaling damage based on active afflictions
        if(activeStatuses.Any(status => status.statusName == Status.StatusName.Frailty)) // If affected by frailty, reduce outgoing damage by 25%
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
            damageToDo_1 += Mathf.CeilToInt(damageToDo_1 * (critDamage * 0.01f));
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

        // time cost will be added in playercombat
    }

    public void AldenDefend()
    {
        aldenAnimator.SetTrigger("AldenDefend");
        isDefending = true;
        shieldAuraAnimator.SetTrigger("ShieldAuraActive");
        // time cost will be added in playercombat
    }

    public void AldenSkill1(int targetPosition, int selfPosition)
    {
        isCritical_1 = false;

        // CALCULATING DAMAGE
        // Scaling damage based on strength and the skill's scaling multiplier
        damageToDo_1 = (int)(attack * skill1Scaling);

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
            damageToDo_1 += Mathf.CeilToInt(damageToDo_1 * (critDamage * 0.01f));
        }

        // Play the attack animation
        if (selfPosition == 1)
        {
            if (targetPosition == 0)
            {
                aldenAnimator.SetTrigger("CripplingCleave_1-1");  // Deal the damage by calling DealDamage() using animation event
            }
            else if (targetPosition == 1)
            {
                aldenAnimator.SetTrigger("CripplingCleave_1-2");
            }
            else if (targetPosition == 2)
            {
                aldenAnimator.SetTrigger("CripplingCleave_1-3");
            }
            else if (targetPosition == 3)
            {
                aldenAnimator.SetTrigger("CripplingCleave_1-4");
            }
        }
        else if (selfPosition == 2)
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

        // Deduct mana cost
        mana -= skill1ManaCost;

        // time cost will be added in playercombat

        // Update the mana bar visually only (for visual feedback of mana being used)
        CombatManager.instance.MPSlider.value -= skill1ManaCost;
    }

    public void AldenSkill2(int targetPosition, int selfPosition)
    {
        isCritical_1 = false;
        isCritical_2 = false;

        // CALCULATING DAMAGE
        // Scaling damage based on strength and the skill's scaling multiplier
        damageToDo_1 = (int)(attack * skill1Scaling);

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

        // Assigning damage value to other attacks
        damageToDo_2 = damageToDo_1;

        // Checking to see if the attacks will be critical hits
        randomNumber_1 = Random.Range(1, 101);
        randomNumber_2 = Random.Range(1, 101);
        if (randomNumber_1 <= critRate)
        {
            isCritical_1 = true;
            damageToDo_1 += Mathf.CeilToInt(damageToDo_1 * (critDamage * 0.01f));
        }
        if (randomNumber_2 <= critRate)
        {
            isCritical_2 = true;
            damageToDo_2 += Mathf.CeilToInt(damageToDo_2 * (critDamage * 0.01f));
        }

        // Play the attack animation
        if (selfPosition == 1)
        {
            if (targetPosition == 0)
            {
                aldenAnimator.SetTrigger("BladeSurge_1-1");  // Deal the damage by calling DealDamage() using animation event
            }
            else if (targetPosition == 1)
            {
                aldenAnimator.SetTrigger("BladeSurge_1-2");
            }
            else if (targetPosition == 2)
            {
                aldenAnimator.SetTrigger("BladeSurge_1-3");
            }
            else if (targetPosition == 3)
            {
                aldenAnimator.SetTrigger("BladeSurge_1-4");
            }
        }
        else if (selfPosition == 2)
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

        // Deduct mana cost
        mana -= skill2ManaCost;

        // time cost will be added in playercombat

        // Update the mana bar visually only (for visual feedback of mana being used)
        CombatManager.instance.MPSlider.value -= skill2ManaCost;
    }

    public void AldenSkill3(int targetPosition, int selfPosition)
    {
        isCritical_1 = false;
        isCritical_2 = false;
        isCritical_3 = false;

        // CALCULATING DAMAGE
        // Scaling damage based on strength and the skill's scaling multiplier
        damageToDo_1 = (int)(attack * skill1Scaling);

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

        // Increase outgoing damage by 25% if under effect of quickness
        if (activeStatuses.Any(status => status.statusName == Status.StatusName.Haste)) // If affected by haste, further increase outgoing damage by 25%
        {
            damageToDo_1 += (int)(damageToDo_1 * 0.25f);
        }

        // Strip a boon from the enemy if under effect of quickness - Handled in animation event

        // Assigning damage value to other attacks
        damageToDo_2 = damageToDo_1;
        damageToDo_3 = damageToDo_1;

        // Checking to see if the attacks will be critical hits
        randomNumber_1 = Random.Range(1, 101);
        randomNumber_2 = Random.Range(1, 101);
        randomNumber_3 = Random.Range(1, 101);
        if (randomNumber_1 <= critRate)
        {
            isCritical_1 = true;
            damageToDo_1 += Mathf.CeilToInt(damageToDo_1 * (critDamage * 0.01f));
        }
        if (randomNumber_2 <= critRate)
        {
            isCritical_2 = true;
            damageToDo_2 += Mathf.CeilToInt(damageToDo_2 * (critDamage * 0.01f));
        }
        if (randomNumber_3 <= critRate)
        {
            isCritical_3 = true;
            damageToDo_3 += Mathf.CeilToInt(damageToDo_3 * (critDamage * 0.01f));
        }

        // Play the attack animation
        if (selfPosition == 1)
        {
            if (targetPosition == 0)
            {
                aldenAnimator.SetTrigger("BladeStorm_1-1");  // Deal the damage by calling DealDamage() using animation event
            }
            else if (targetPosition == 1)
            {
                aldenAnimator.SetTrigger("BladeStorm_1-2");
            }
            else if (targetPosition == 2)
            {
                aldenAnimator.SetTrigger("BladeStorm_1-3");
            }
            else if (targetPosition == 3)
            {
                aldenAnimator.SetTrigger("BladeStorm_1-4");
            }
        }
        else if (selfPosition == 2)
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

        // Deduct mana cost
        mana -= skill3ManaCost;

        // time cost will be added in playercombat

        // Update the mana bar visually only (for visual feedback of mana being used)
        CombatManager.instance.MPSlider.value -= skill3ManaCost;
    }

    public void AldenTakeDamage(int incomingDamage)
    {
        // play damage animation
        aldenAnimator.SetTrigger("AldenHurt");

        // play hit FX
        int randomRotation = Random.Range(0, 45);
        hitFXObject.transform.Rotate(0f, 0f, randomRotation);
        hitFXAnimator.SetTrigger("HitEffect");

        // calculate def reductions - already done by enemy

        // calculate incoming damage by status modifiers

        // Calculating damage increase due to afflictions
        if(activeStatuses.Any(status => status.statusName == Status.StatusName.Exposed)) // If affected by exposed, increase incoming damage by 25%
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

        if(incomingDamage <= (int)(maxHealth * 0.05f))
        {
            incomingDamage = (int)(maxHealth * 0.05f);
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
            aldenAnimator.SetTrigger("AldenDeath");
            CombatManager.instance.ReportDeath("Alden");
        }
    }

    public void DealDamage()
    {
        // Tell combat manager to deal damage to active enemy
        if(dealDamageSequenceCounter == 1)
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
        else if(dealDamageSequenceCounter == 2)
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
        else if(dealDamageSequenceCounter == 3)
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
        CombatManager.instance.isAldenTurn = false;
    }

    private void PlaySoundEffect(string sfxName)
    {
        AudioManager.instance.PlaySound(sfxName);
    }


    // TODO: Edit this later to enable changing the button sprite based on the case
    public void EnableSkillButtons()
    {
        // Checking for skill button 1
        if(level >= skill1UnlockLevel)
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
        if(level >= skill2UnlockLevel)
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
        if(level >= skill3UnlockLevel)
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
