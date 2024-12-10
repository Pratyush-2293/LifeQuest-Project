using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanelsManager : MonoBehaviour
{
    public static ActionPanelsManager instance = null;

    [Header("Hero Character Components")]
    public GameObject heroActionPanel = null;
    public Slider heroTurnSlider = null;
    public Button attackButton;
    public Button defendButton;
    public Button skipTurnButton;
    public Button skill1Button;
    public Button skill2Button;
    public Button skill3Button;
    public Button selectUpButton;
    public Button selectDownButton;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one ActionPanelsManager instance!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Hero Button Functions
    public void OnHeroAttackButton()
    {
        HeroCombat.instance.Attack();
    }

    // Selection input: 1 for Up, 2 for down.
    public void OnSelectUpButton()
    {
        CombatSceneScript.instance.UpdateSelectedTarget(1);
    }

    public void OnSelectDownButton()
    {
        CombatSceneScript.instance.UpdateSelectedTarget(2);
    }
}
