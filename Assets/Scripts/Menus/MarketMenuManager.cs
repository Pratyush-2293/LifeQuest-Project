using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketMenuManager : MonoBehaviour
{

    // -------------------------- BUTTON FUNCTIONS --------------------------

    public void OnBackButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameHubMenu");
    }
}
