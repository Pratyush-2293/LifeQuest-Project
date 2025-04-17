using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Data")]
    public Sprite doorOpenSprite;

    public bool isInteractible = false;

    [Header("Object Components")]
    [Space(5)]
    public SpriteRenderer doorImage;

    public void OpenDoor()
    {
        // Change the door sprite to open
        doorImage.sprite = doorOpenSprite;
        // Make the door interactible
        isInteractible = true;
    }

    public void EnterDoor()
    {
        // Load the next level scene
    }
}
