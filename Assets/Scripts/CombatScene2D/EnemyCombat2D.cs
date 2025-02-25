using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyCombat2D : MonoBehaviour
{
    public int turnCounter = 10;
    public int turnSpeed = 1;
    public bool isPlayerCharacter = false;
    public int health = 100;
    public int maxHealth = 100;
    public Slider healthSlider = null;
    public GameObject selectMarker = null;
    public bool isDefeated = false;
    public bool isTaunted = false;
    public Animator enemyAnimator = null;

    // Damage Popup Components
    public Animator normalDamageAnimator = null;
    public Animator criticalDamageAnimator = null;
    public TMP_Text normalDamageText = null;
    public TMP_Text criticalDamageText = null;

    private void Start()
    {
        healthSlider.maxValue = maxHealth;
    }
    public void UpdateSelfState()
    {
        healthSlider.value = health;
    }

    public void Action_Attack()
    {
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
        int selectedTarget = 0;
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

    }

    public void Action_TakeDamage(int incomingDamage, bool isCritical)
    {
        // play damage animation
        enemyAnimator.SetTrigger("TakeDamage");

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
