using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Slider healthBar;

    public int health = 100;

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    private void UpdateHealthBar()
    {
        healthBar.value = health;
    }
}
