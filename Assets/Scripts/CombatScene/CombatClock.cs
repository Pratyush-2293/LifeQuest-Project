using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatClock : MonoBehaviour
{
    public static CombatClock instance = null;
    public int timerTick = 0;
    private bool isPaused = false;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one instance of CombatClock!");
            Destroy(gameObject);
        }
        instance = this;
    }

    private void FixedUpdate()
    {
        if (!isPaused)
        {
            if (timerTick == 100)
            {
                timerTick = 0;
            }
            timerTick++;
        }
    }

    public void PauseClock()
    {
        isPaused = true;
    }

    public void ResumeClock()
    {
        isPaused = false;
    }
}
