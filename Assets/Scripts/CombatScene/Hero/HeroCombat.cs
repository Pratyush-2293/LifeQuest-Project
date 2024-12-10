using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCombat : MonoBehaviour
{
    public static HeroCombat instance = null;

    private GameObject actionsPanel = null;
    private Slider turnSlider = null;
    public int timer = 0;
    public int turnThreshold = 500; // Turn active at 1000 pts
    public int turnSpeed = 10;
    public bool isTurn = false;

    public bool initialTargetSelected = false;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one HeroCombat instance!");
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
        if (ActionPanelsManager.instance)
        {
            actionsPanel = ActionPanelsManager.instance.heroActionPanel;
            turnSlider = ActionPanelsManager.instance.heroTurnSlider;
        }
        
    }

    private void Update()
    {
        UpdateTurnSlider();
        CheckTurn();

        // Turn Active
        if (isTurn)
        {
            CombatClock.instance.PauseClock();

            if (actionsPanel)
            {
                actionsPanel.SetActive(true);
            }

            if (initialTargetSelected == false)
            {
                CombatSceneScript.instance.SelectInitialTarget();
                initialTargetSelected = true;
            }
        }
        else
        {
            ProgressTimer();
        }
    }

    public void Attack()
    {
        // do attack animation
        // do damage to enemy

        //resume game timer
        isTurn = false;
        turnThreshold = 0;
        actionsPanel.SetActive(false);
        CombatClock.instance.ResumeClock();
    }

    private void ProgressTimer()
    {
        if (CombatClock.instance)
        {
            if (timer != CombatClock.instance.timerTick)  // Updates things when there is a mismatch b/w internal clock and main clock, syncing stuff every frame with a one frame delay.
            {
                timer = CombatClock.instance.timerTick;
                turnThreshold += turnSpeed;
            }
        }
    }


    private void CheckTurn()
    {
        if (turnThreshold >= 1000)
        {
            isTurn = true;
            turnThreshold = 0;
            timer = 0;
        }
    }

    private void UpdateTurnSlider()
    {
        if (!isTurn)
        {
            turnSlider.value = turnThreshold;
        }
        else
        {
            turnSlider.value = turnSlider.maxValue;
        }
    }
}
