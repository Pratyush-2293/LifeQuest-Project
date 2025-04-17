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

    // Private Variables
    private Rigidbody2D rigidBody;
    private Vector2 moveDirection = Vector2.zero;
    private bool isMoving = false;

    private LeverController currentLeverController;
    private bool nearLever = false;

    private void Awake()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
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
    }

    // ---------------------------- BUTTON FUNCTIONS ----------------------------

    public void OnInteractButton()
    {
        if(nearLever == true)
        {
            currentLeverController.TurnLeverOn();
            interactButton.interactable = false;
        }
    }

    public void OnDownButton()
    {
        isMoving = true;
        moveDirection = Vector2.down;
        aldenAnimator.SetTrigger("RunDown");
    }

    public void OnUpButton()
    {
        isMoving = true;
        moveDirection = Vector2.up;
        aldenAnimator.SetTrigger("RunUp");
    }

    public void OnLeftButton()
    {
        isMoving = true;
        moveDirection = Vector2.left;
        aldenAnimator.SetTrigger("RunLeft");
    }

    public void OnRightButton()
    {
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
