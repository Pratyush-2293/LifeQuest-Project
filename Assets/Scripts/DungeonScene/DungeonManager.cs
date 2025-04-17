using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    [HideInInspector] public static DungeonManager instance = null;

    [Header("Levers In Scene")]
    [Space(5)]
    public int totalLevers = 0;
    public LeverController[] leverControllers;

    [Header("Door In Scene")]
    [Space(5)]
    public DoorController doorController;

    [Header("Alden Controls")]
    [Space(5)]
    public Button aldenInteractButton;

    [Header("Dungeon Components")]
    [Space(5)]
    public GameObject dungeonRootObject;
    public SceneTransition sceneTransition;

    // Private Variables
    private int leversTriggered = 0;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Trying to create more than once instance of DungeonManager!");
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
        // GameManager.instance.dungeonRootObject = dungeonRootObject;
    }

    public void LeverTriggered()
    {
        // Increment the levers triggered
        leversTriggered++;

        // Check if all levers are triggered to open the door
        CheckDoorOpenCondition();
    }

    private void CheckDoorOpenCondition()
    {
        if(leversTriggered == totalLevers)
        {
            doorController.OpenDoor();
        }
    }

    public void LoadDungeonCombatScene(string sceneName)
    {
        // Play the screen transition
        sceneTransition.LoadSceneWithTransitionAdditive(sceneName, dungeonRootObject);
    }
}
