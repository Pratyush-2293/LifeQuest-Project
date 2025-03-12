using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MarketMenuManager : MonoBehaviour
{
    public static MarketMenuManager instance = null;

    [Header("Market Menu Components")]
    public TMP_Text goldCoinsDisplay;
    public GameObject merchantListPanel;
    public GameObject bromundChar;
    public Button leftMerchantButton;
    public Button rightMerchantButton;
    public GameObject backButton;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Trying to create more than one MarketMenuManager instance !");
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
        UpdateGoldCoinsDisplay();
    }
    public void UpdateGoldCoinsDisplay()
    {
        goldCoinsDisplay.text = GameData.instance.goldCoins.ToString();
    }

    // -------------------------- BUTTON FUNCTIONS --------------------------

    public void OnBackButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameHubMenu");
    }

    public void OnMerchantPanelCloseButton()
    {
        merchantListPanel.gameObject.SetActive(false);
        bromundChar.gameObject.SetActive(true);
    }
}
