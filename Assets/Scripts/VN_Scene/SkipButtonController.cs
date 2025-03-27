using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipButtonController : MonoBehaviour
{
    public VN_Manager vnManager;
    public Button skipButton;

    private void Start()
    {
        SkipButtonEnabler();
    }

    public void SkipButtonEnabler() // Checks to see if skip buttons should be enabled, and enables if necessary
    {
        if (vnManager.levelID <= GameData.instance.levelViewedID)
        {
            skipButton.interactable = true;
        }
    }

    public void OnSkipButton()
    {
        // Load the combat scene provided in the last dialogue
        vnManager.SkipToDialogue();
    }
}
