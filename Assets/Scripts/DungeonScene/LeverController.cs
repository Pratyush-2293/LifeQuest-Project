using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    [Header("Lever Data")]
    [Space(5)]
    public Sprite leverOnSprite;

    public bool isInteractible = true;

    [Header("Object Components")]
    public SpriteRenderer leverImage;
    public GameObject interactPopup;

    public void TurnLeverOn()
    {
        // Change the sprite of the lever
        leverImage.sprite = leverOnSprite;

        // Notify the dungeon manager of the lever trigger
        DungeonManager.instance.LeverTriggered();

        // Turn off the player's interactability with the lever.
        isInteractible = false;

        // Turn off the interact popup
        interactPopup.gameObject.SetActive(false);

        // Play lever pull sound
        AudioManager.instance.PlaySound("leverPull");
    }
}
