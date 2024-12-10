using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Slider healthBar;
    public GameObject selectMarker;

    public int health = 100;

    public int position = 0;

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
