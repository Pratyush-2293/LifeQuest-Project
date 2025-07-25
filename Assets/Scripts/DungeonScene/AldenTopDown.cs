using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AldenTopDown : MonoBehaviour
{
    [Header("Movement Variables")]
    public float moveSpeed = 5f;

    [Header("Alden Components")]
    public Animator aldenAnimator;
    public Button interactButton;
    public AudioSource footstepSource;

    // Private Variables
    private Rigidbody2D rigidBody;
    private Vector2 moveDirection = Vector2.zero;
    private bool isMoving = false;
    private bool isFrozen = false;

    private LeverController currentLeverController;
    private PickupItem currentPickupItem;
    private DoorController currentDoorController;

    private bool nearLever = false;
    private bool nearPickupItem = false;
    private bool nearDoor = false;

    private void Awake()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(isFrozen == false)
        {
            if (isMoving)
            {
                rigidBody.velocity = moveDirection * moveSpeed;
            }
            else
            {
                rigidBody.velocity = Vector2.zero;
            }
        }
        else
        {
            isMoving = false;
            rigidBody.velocity = Vector2.zero;
        }
    }

    void Update()
    {
        if (isMoving && !footstepSource.isPlaying)
            footstepSource.Play();
        else if (!isMoving && footstepSource.isPlaying)
            footstepSource.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lever"))
        {
            currentLeverController = collision.GetComponent<LeverController>();
            nearLever = true;
            if(currentLeverController.isInteractible == true)
            {
                interactButton.interactable = true;
                currentLeverController.interactPopup.gameObject.SetActive(true);
            }
        }
        else if (collision.CompareTag("Pickup"))
        {
            currentPickupItem = collision.GetComponent<PickupItem>();
            nearPickupItem = true;

            interactButton.interactable = true;
            currentPickupItem.interactPopup.gameObject.SetActive(true);
        }
        else if (collision.CompareTag("Door"))
        {
            currentDoorController = collision.GetComponent<DoorController>();
            if (currentDoorController.isInteractible)
            {
                nearDoor = true;

                interactButton.interactable = true;
                currentDoorController.interactPopup.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Lever"))
        {
            if (currentLeverController != null && collision.gameObject == currentLeverController.gameObject)
            {
                currentLeverController.interactPopup.gameObject.SetActive(false);
                currentLeverController = null;
                nearLever = false;
                interactButton.interactable = false;
            }
        }
        else if (collision.CompareTag("Pickup"))
        {
            if(currentPickupItem != null && collision.gameObject == currentPickupItem.gameObject)
            {
                currentPickupItem.interactPopup.gameObject.SetActive(false);
                currentPickupItem = null;
                nearPickupItem = false;
                interactButton.interactable = false;
            }
        }
        else if (collision.CompareTag("Door"))
        {
            if(currentDoorController != null && collision.gameObject == currentDoorController.gameObject)
            {
                currentDoorController.interactPopup.gameObject.SetActive(false);
                currentDoorController = null;
                nearDoor = false;
                interactButton.interactable = false;
            }
        }
    }

    public void FreezePlayer()
    {
        isFrozen = true;

        aldenAnimator.SetTrigger("StopRunning");
    }

    public void UnfreezePlayer()
    {
        isFrozen = false;
    }

    private void UpdateFootstepVolume()
    {
        footstepSource.volume = AudioManager.instance.sfxVolume;
    }

    // ---------------------------- BUTTON FUNCTIONS ----------------------------

    public void OnInteractButton()
    {
        if(nearLever == true)
        {
            currentLeverController.TurnLeverOn();
            interactButton.interactable = false;
        }
        else if(nearPickupItem == true)
        {
            currentPickupItem.PickUpItem();
            interactButton.interactable = false;
        }
        else if(nearDoor == true)
        {
            currentDoorController.EnterDoor();
            interactButton.interactable = false;
        }
    }

    public void OnDownButton()
    {
        UpdateFootstepVolume();

        isMoving = true;
        moveDirection = Vector2.down;
        aldenAnimator.SetTrigger("RunDown");
    }

    public void OnUpButton()
    {
        UpdateFootstepVolume();

        isMoving = true;
        moveDirection = Vector2.up;
        aldenAnimator.SetTrigger("RunUp");
    }

    public void OnLeftButton()
    {
        UpdateFootstepVolume();

        isMoving = true;
        moveDirection = Vector2.left;
        aldenAnimator.SetTrigger("RunLeft");
    }

    public void OnRightButton()
    {
        UpdateFootstepVolume();

        isMoving = true;
        moveDirection = Vector2.right;
        aldenAnimator.SetTrigger("RunRight");
    }

    public void OnReleaseButton()
    {
        isMoving = false;
        aldenAnimator.SetTrigger("StopRunning");
    }
}
