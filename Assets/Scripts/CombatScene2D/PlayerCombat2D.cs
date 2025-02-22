using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat2D : MonoBehaviour
{
    public int turnCounter = 30;
    public int turnSpeed = 2;
    public bool isPlayerCharacter = true;
    public string characterName = "Alden";
    public int health = 100;
    public int mana = 100;
    public Animator playerAnimator = null;

    public void PlayAttackAnimation(int targetPosition)
    {
        if(targetPosition == 0)
        {
            playerAnimator.SetTrigger("Attack_1-1");
        }
        else if(targetPosition == 1)
        {
            playerAnimator.SetTrigger("Attack_1-2");
        }
        else if(targetPosition == 2)
        {
            playerAnimator.SetTrigger("Attack_1-3");
        }
        else if(targetPosition == 3)
        {
            playerAnimator.SetTrigger("Attack_1-4");
        }
    }
}
