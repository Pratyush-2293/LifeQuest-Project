using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Header("Character Components")]
    [Space(5)]
    public Image characterImage;
    public Animator characterAnimator;
    public bool isLeftCharacter = false;

    [Header("Expression Sprites")]
    [Space(5)]
    public Sprite neutralFace;
    public Sprite happyFace;
    public Sprite laughingFace;
    public Sprite seriousFace;
    public Sprite angryFace;
    public Sprite sadFace;

    public void UpdateExpression(VN_Manager.Expression expression)
    {
        if(expression == VN_Manager.Expression.Neutral)
        {
            characterImage.sprite = neutralFace;
        }
        else if(expression == VN_Manager.Expression.Happy)
        {
            characterImage.sprite = happyFace;
        }
        else if (expression == VN_Manager.Expression.Laughing)
        {
            characterImage.sprite = laughingFace;
        }
        else if (expression == VN_Manager.Expression.Serious)
        {
            characterImage.sprite = seriousFace;
        }
        else if (expression == VN_Manager.Expression.Angry)
        {
            characterImage.sprite = angryFace;
        }
        else if (expression == VN_Manager.Expression.Sad)
        {
            characterImage.sprite = sadFace;
        }
    }

    public void CharacterSlideIn()
    {
        if (isLeftCharacter)
        {
            characterAnimator.SetTrigger("SlideInFromLeft");
        }
        else
        {
            characterAnimator.SetTrigger("SlideInFromRight");
        }
    }

    public void CharacterSlideOut()
    {
        if (isLeftCharacter)
        {
            characterAnimator.SetTrigger("SlideOutFromLeft");
        }
        else
        {
            characterAnimator.SetTrigger("SlideOutFromRight");
        }
    }
}
