using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Data")]
    [Space(5)]
    public int teleportToLevel = 1;
    public Transform teleportPosition;
    [Space(10)]
    public Sprite doorOpenSprite;
    public bool isInteractible = false;

    [Header("Object Components")]
    [Space(5)]
    public SpriteRenderer doorImage;
    public GameObject interactPopup;

    public void OpenDoor()
    {
        // Change the door sprite to open
        doorImage.sprite = doorOpenSprite;
        // Make the door interactible
        isInteractible = true;
    }

    public void EnterDoor()
    {
        // Move to next level start point
        DungeonManager.instance.DoorEntered(teleportToLevel, teleportPosition);
    }
}
