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
    public GameObject agathaChar;
    public Button leftMerchantButton;
    public Button rightMerchantButton;
    public GameObject blackSmithBackButton;
    public GameObject alchemistBackButton;
    public TMP_Text merchantNameText;
    public Sprite blacksmithShopBG;
    public Sprite alchemistShopBG;
    public SpriteRenderer backgroundImage;

    [Header("Navigation Components")]
    public Image navDot1;
    public Image navDot2;
    public Image navDot3;
    public Color navDotActiveColor;
    public Color navDotInactiveColor;

    [Header("Blacksmith Menus")]
    public GameObject blacksmithMainMenu;
    public GameObject blacksmithCraftMenu;
    public GameObject blacksmithWeightOptionsMenu;
    public GameObject blacksmithArmorTypesMenu;

    [Header("Alchemist Menus")]
    public GameObject alchemistMainMenu;
    public GameObject alchemistEnhanceMenu;

    [Header("Misc Components")]
    public SceneTransition sceneTransition;

    private int merchantPageNumber = 1;

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

    private void SwitchMenus(int currentMerchantPageNumber, int nextMerchantPageNumber)
    {
        if(currentMerchantPageNumber == 1 && nextMerchantPageNumber == 2) // If current merchant is blacksmith & moving to alchemist
        {
            // turn off all menus for the blacksmith
            blacksmithMainMenu.gameObject.SetActive(false);
            blacksmithCraftMenu.gameObject.SetActive(false);
            blacksmithWeightOptionsMenu.gameObject.SetActive(false);
            blacksmithArmorTypesMenu.gameObject.SetActive(false);

            // turn on the main menu for the alchemist
            alchemistMainMenu.gameObject.SetActive(true);

            // update the merchant's name title
            merchantNameText.text = "Agatha's Enchantry";

            // Change the merchant character
            bromundChar.gameObject.SetActive(false);
            agathaChar.gameObject.SetActive(true);

            // Change the merchant BG
            backgroundImage.sprite = alchemistShopBG;

            // Disable the blacksmith back button
            blackSmithBackButton.gameObject.SetActive(false);
        }

        if(currentMerchantPageNumber == 2 && nextMerchantPageNumber == 1) // If current merchant is alchemist and moving to blacksmith
        {
            // turn off all menus for the alchemist
            alchemistMainMenu.gameObject.SetActive(false);
            alchemistEnhanceMenu.gameObject.SetActive(false);

            // turn on the main menu for the blacksmith
            blacksmithMainMenu.gameObject.SetActive(true);

            // update the merchant's name title
            merchantNameText.text = "Bromund's Forge";

            // Change the merchant character
            bromundChar.gameObject.SetActive(true);
            agathaChar.gameObject.SetActive(false);

            // Change the merchant BG
            backgroundImage.sprite = blacksmithShopBG;

            // Disable the alchemist back button
            alchemistBackButton.gameObject.SetActive(false);
        }
    }

    private void HandleNavbarDots()
    {
        if(merchantPageNumber == 1)
        {
            navDot1.color = navDotActiveColor;
            navDot2.color = navDotInactiveColor;
            navDot3.color = navDotInactiveColor;
        }
        else if(merchantPageNumber == 2)
        {
            navDot1.color = navDotInactiveColor;
            navDot2.color = navDotActiveColor;
            navDot3.color = navDotInactiveColor;
        }
        else if(merchantPageNumber == 3)
        {
            navDot1.color = navDotInactiveColor;
            navDot2.color = navDotInactiveColor;
            navDot3.color = navDotActiveColor;
        }
    }

    // -------------------------- BUTTON FUNCTIONS --------------------------

    public void OnBackButton()
    {
        AudioManager.instance.PlayUISound("uiClick2");

        sceneTransition.LoadSceneWithTransition("GameHubMenu");
    }

    public void OnMerchantPanelCloseButton()
    {
        AudioManager.instance.PlayUISound("uiClick2");

        merchantListPanel.gameObject.SetActive(false);
        if(merchantPageNumber == 1)
        {
            bromundChar.gameObject.SetActive(true);
        }
        else if(merchantPageNumber == 2)
        {
            agathaChar.gameObject.SetActive(true);
        }
    }

    public void OnRightMerchantButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        // provide the switch menu function with current and next merchant's page numbers
        SwitchMenus(merchantPageNumber, merchantPageNumber + 1);

        // change the merchant page number to the active one
        merchantPageNumber++;

        // Enable the left button
        leftMerchantButton.interactable = true;

        // check to see if we have reached at the last page, if yes the we disable the right button

        if(merchantPageNumber == 2) // change this when new merchant type is added
        {
            rightMerchantButton.interactable = false;
        }

        HandleNavbarDots();
    }

    public void OnLeftMerchantButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        // provide the switch menu function with current and next merchant's page numbers
        SwitchMenus(merchantPageNumber, merchantPageNumber - 1);

        // change the merchant page number to the active one
        merchantPageNumber--;

        // Enable the right button
        rightMerchantButton.interactable = true;

        // check to see if we have reached at the first page, if yes the we disable the left button

        if (merchantPageNumber == 1)
        {
            leftMerchantButton.interactable = false;
        }

        HandleNavbarDots();
    }
}
