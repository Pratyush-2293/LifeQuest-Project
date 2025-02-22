using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        healthSlider.maxValue = maxHealth;
    }
    public void UpdateSelfState()
    {
        healthSlider.value = health;
    }
}
