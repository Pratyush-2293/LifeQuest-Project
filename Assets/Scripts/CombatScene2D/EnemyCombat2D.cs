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
        // choose a random character from available characters , unless taunted
        // deal damage to randomly selected character
        // if taunted, deal damage to valric
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
