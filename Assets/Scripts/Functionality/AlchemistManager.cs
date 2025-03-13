using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlchemistManager : MonoBehaviour
{
    public static AlchemistManager instance = null;

    [Header("Materials Available For Purchase")]
    [Space(5)]
    public List<ItemSO> purchaseItems;
    [HideInInspector] public List<PurchaseItemUI> purchaseItemUIs;

    [Header("Alchemist Character Components")]
    [Space(5)]
    public GameObject agathaChar;

    [Header("Menu Button Components")]
    [Space(5)]
    public GameObject mainMenuPanel;
    public GameObject enhanceOptionsPanel;
    public GameObject backButton;

    [Header("Merchant List Components")]
    [Space(5)]
    public GameObject merchantListPanel;
    public TMP_Text itemTypeLabelText;
    public Transform merchantListContainer;
    public GameObject enhanceListItemUI;
    public GameObject purchaseListItemUI;
    public GameObject sellListItemUI;

    [Header("Enhancing Menu Components")]
    [Space(5)]
    public GameObject enhancingDetailsPanelParent;
    public GameObject enhancingDetailsPanel;
    public Button enhancingDetailsEnhanceButton;
    public TMP_Text enhanceItemNameText;
    public Image enhanceItemIcon;
    public TMP_Text enhanceItemMainStatText;
    public TMP_Text enhanceItemSubStatText;
    public TMP_Text enhancedItemMainStatText;
    public TMP_Text enhancedItemSubStatText;
    public GameObject[] enhancingMaterialObjects = new GameObject[4];
    public Image[] enhancingMaterialIcons = new Image[4];
    public TMP_Text[] enhancingMaterialNames = new TMP_Text[4];
    public TMP_Text[] availableQuantityText = new TMP_Text[4];
    public TMP_Text enhancingCostText;
    public Color insufficientMaterialsColor;

    [Header("Enhancing Success Panel Components")]
    [Space(5)]
    public GameObject enhancingSuccessPanel;
    public Image enhancingSuccessItemIcon;
    public TMP_Text enhancingSuccessItemNameText;

    // Private Variables
    private int currentMenuID = 1;
    private Item activeEnhanceItem;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Trying to create more than one instance of AlchemistManager !");
            Destroy(gameObject);
        }
        instance = this;
    }

    public void OpenEnhancingDetails(Item item)
    {

    }
    /*
    public void OpenEnhancingDetails(Item enhanceItem)
    {
        // Save the enhanceItem to a global variable for other functions' use
        activeEnhanceItem = enhanceItem;

        // Start a material availability counter, which checks if all items are available for crafting
        int totalMaterialsAvailable = 0;
        bool sufficientGoldAvailable = false;

        // Load the data into the enhancing details panel's UI
        enhanceItemNameText.text = enhanceItem.itemName;
        if (enhanceItem.itemIcon != null)
        {
            enhanceItemIcon.sprite = enhanceItem.itemIcon;
        }

        // ------------- Load the main stat text -------------
        if (enhanceItem.mainStatType == Item.MainStatType.ATK)
        {
            enhanceItemMainStatText.text = "ATK ";
        }
        else
        {
            enhanceItemMainStatText.text = "DEF ";
        }
        // Add the main stat value to the main stat text
        enhanceItemMainStatText.text += "+" + enhanceItem.mainStatValue.ToString();
        // ---------------------------------------------------

        // ------------- Load the sub stat text -------------
        if (enhanceItem.subStatType == Item.SubStatType.None)
        {
            enhanceItemSubStatText.text = "-";
        }
        else if (enhanceItem.subStatType == Item.SubStatType.CR)
        {
            enhanceItemSubStatText.text = "CR ";
        }
        else if (enhanceItem.subStatType == Item.SubStatType.CD)
        {
            enhanceItemSubStatText.text = "CD ";
        }
        else if (enhanceItem.subStatType == Item.SubStatType.ATK)
        {
            enhanceItemSubStatText.text = "ATK ";
        }
        else if (enhanceItem.subStatType == Item.SubStatType.DEF)
        {
            enhanceItemSubStatText.text = "DEF ";
        }
        else if (enhanceItem.subStatType == Item.SubStatType.MP)
        {
            enhanceItemSubStatText.text = "MP ";
        }
        // Add the sub stat value to the sub stat text
        enhanceItemSubStatText.text += "+" + enhanceItem.subStatValue.ToString();
        // Add a % symbol if the substat is CR or CD
        if (enhanceItem.subStatType == Item.SubStatType.CR || enhanceItem.subStatType == Item.SubStatType.CD)
        {
            enhanceItemSubStatText.text += "%";
        }
        // --------------------------------------------------

        // ------------------ Load the Material Icons, Names and Quantities ------------------
        // Load the first material
        if (enhanceItem.totalMaterialTypes >= 1)
        {
            // Setting the material active
            craftingMaterialObjects[0].gameObject.SetActive(true);

            // loading the icon
            craftMaterialIcons[0].sprite = craftItem.craftingMaterial1.itemIcon;

            // loading the name
            craftMaterialNames[0].text = craftItem.craftingMaterial1.itemName;

            // loading the quanities into the name
            craftMaterialNames[0].text += " X " + craftItem.materialQuantity1.ToString();

            // loading the available quantities of the material from player inventory
            int availableQuantity = 0;
            foreach (Item item in GameData.instance.inventory)
            {
                if (item.itemName == craftItem.craftingMaterial1.itemName)
                {
                    availableQuantity = item.itemQuantity;
                }
            }
            availableQuantityText[0].text = availableQuantity.ToString();
            // if available quantity is less than required, highlight it in red
            if (availableQuantity < craftItem.materialQuantity1)
            {
                availableQuantityText[0].color = insufficientMaterialsColor;
            }
            else
            {
                availableQuantityText[0].color = Color.white;
                totalMaterialsAvailable++;
            }
        }

        // Load the second material
        if (craftItem.totalMaterialTypes >= 2) // Load the second material
        {
            // Setting the material active
            craftingMaterialObjects[1].gameObject.SetActive(true);

            // loading the icon
            craftMaterialIcons[1].sprite = craftItem.craftingMaterial2.itemIcon;

            // loading the name
            craftMaterialNames[1].text = craftItem.craftingMaterial2.itemName;

            // loading the quanities into the name
            craftMaterialNames[1].text += " X " + craftItem.materialQuantity2.ToString();

            // loading the available quantities of the material from player inventory
            int availableQuantity = 0;
            foreach (Item item in GameData.instance.inventory)
            {
                if (item.itemName == craftItem.craftingMaterial2.itemName)
                {
                    availableQuantity = item.itemQuantity;
                }
            }
            availableQuantityText[1].text = availableQuantity.ToString();
            // if available quantity is less than required, highlight it in red
            if (availableQuantity < craftItem.materialQuantity2)
            {
                availableQuantityText[1].color = insufficientMaterialsColor;
            }
            else
            {
                availableQuantityText[1].color = Color.white;
                totalMaterialsAvailable++;
            }
        }

        // Load the third material
        if (craftItem.totalMaterialTypes >= 3)
        {
            // Setting the material active
            craftingMaterialObjects[2].gameObject.SetActive(true);

            // loading the icon
            craftMaterialIcons[2].sprite = craftItem.craftingMaterial3.itemIcon;

            // loading the name
            craftMaterialNames[2].text = craftItem.craftingMaterial3.itemName;

            // loading the quanities into the name
            craftMaterialNames[2].text += " X " + craftItem.materialQuantity3.ToString();

            // loading the available quantities of the material from player inventory
            int availableQuantity = 0;
            foreach (Item item in GameData.instance.inventory)
            {
                if (item.itemName == craftItem.craftingMaterial3.itemName)
                {
                    availableQuantity = item.itemQuantity;
                }
            }
            availableQuantityText[2].text = availableQuantity.ToString();
            // if available quantity is less than required, highlight it in red
            if (availableQuantity < craftItem.materialQuantity3)
            {
                availableQuantityText[2].color = insufficientMaterialsColor;
            }
            else
            {
                availableQuantityText[2].color = Color.white;
                totalMaterialsAvailable++;
            }
        }

        // Load the fourth material
        if (craftItem.totalMaterialTypes >= 4)
        {
            // Setting the material active
            craftingMaterialObjects[3].gameObject.SetActive(true);

            // loading the icon
            craftMaterialIcons[3].sprite = craftItem.craftingMaterial4.itemIcon;

            // loading the name
            craftMaterialNames[3].text = craftItem.craftingMaterial4.itemName;

            // loading the quanities into the name
            craftMaterialNames[3].text += " X " + craftItem.materialQuantity4.ToString();

            // loading the available quantities of the material from player inventory
            int availableQuantity = 0;
            foreach (Item item in GameData.instance.inventory)
            {
                if (item.itemName == craftItem.craftingMaterial4.itemName)
                {
                    availableQuantity = item.itemQuantity;
                }
            }
            availableQuantityText[3].text = availableQuantity.ToString();
            // if available quantity is less than required, highlight it in red
            if (availableQuantity < craftItem.materialQuantity4)
            {
                availableQuantityText[3].color = insufficientMaterialsColor;
            }
            else
            {
                availableQuantityText[3].color = Color.white;
                totalMaterialsAvailable++;
            }
        }
        // -----------------------------------------------------------------------------------

        // Load the crafting cost text
        craftingCostText.text = craftItem.craftingCost.ToString();
        // highlight the text in red if gold is insufficient
        if (craftItem.craftingCost > GameData.instance.goldCoins)
        {
            craftingCostText.color = insufficientMaterialsColor;
        }
        else
        {
            craftingCostText.color = Color.white;
            sufficientGoldAvailable = true;
        }

        // Set the craft button active if all conditions are met, else disable it
        if (totalMaterialsAvailable == craftItem.totalMaterialTypes && sufficientGoldAvailable)
        {
            craftDetailsCraftButton.interactable = true;
        }
        else
        {
            craftDetailsCraftButton.interactable = false;
        }

        // Turn off Bromund's Character sprite
        bromundChar.gameObject.SetActive(false);

        // Open the crafting details panel
        craftDetailsPanelParent.gameObject.SetActive(true);
    }
    */

    // ------------------------ BUTTON FUNCTIONS ------------------------

    public void OnEnhanceButton()
    {
        mainMenuPanel.gameObject.SetActive(false);
        enhanceOptionsPanel.gameObject.SetActive(true);

        backButton.gameObject.SetActive(true);
        currentMenuID = 2;
    }

    public void OnWeaponsButton()
    {
        // Need to load all the available weapons in the players inventory, as well as weapons that are equipped by characters

        // First we turn off agatha's sprite
        agathaChar.gameObject.SetActive(false);

        // Clear all old objects in the items list panel if they exist
        ClearMerchantListPanel();

        // Next we instantiate all weapon items in the inventory and also those equipped by player characters

        foreach (Item enhanceItem in GameData.instance.inventory) // Adding items from inventory
        {
            EnhanceItemUI enhanceItemUIObject = Instantiate(enhanceListItemUI, merchantListContainer).GetComponent<EnhanceItemUI>();
            enhanceItemUIObject.Initialise(enhanceItem, false, EnhanceItemUI.EquippedCharName.None);
        }

        // Next we update the item type label of the merchant panel
        itemTypeLabelText.text = "Weapon Enhancement";

        // Next we display the list with all craftable swords
        merchantListPanel.gameObject.SetActive(true);
    }

    private void ClearMerchantListPanel()
    {
        foreach (Transform child in merchantListContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
