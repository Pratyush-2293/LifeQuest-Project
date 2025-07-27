using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlacksmithManager : MonoBehaviour
{
    public static BlacksmithManager instance = null;

    [Header("Swords Available For Crafting")]
    [Space(5)]
    public List<BlacksmithCraftItemSO> swordsCraftItems;

    [Header("Shields Available For Crafting")]
    [Space(5)]
    public List<BlacksmithCraftItemSO> shieldsCraftItems;

    [Header("Medium Armor Available For Crafting")]
    [Space(5)]
    public List<BlacksmithCraftItemSO> mediumHeadgearCraftItems;
    public List<BlacksmithCraftItemSO> mediumChestpieceCraftItems;
    public List<BlacksmithCraftItemSO> mediumLegguardsCraftItems;
    public List<BlacksmithCraftItemSO> mediumBootsCraftItems;

    [Header("Materials Available For Purchase")]
    [Space(5)]
    public List<ItemSO> purchaseItems;
    [HideInInspector] public List<PurchaseItemUI> purchaseItemUIs;

    [Header("Blacksmith Character Components")]
    [Space(5)]
    public GameObject bromundChar;

    [Header("Menu Button Components")]
    [Space(5)]
    public GameObject mainMenuPanel;
    public GameObject craftOptionsPanel;
    public GameObject armorWeightOptionsPanel;
    public GameObject armorTypeOptionsPanel;
    public GameObject backButton;

    [Header("Merchant List Components")]
    [Space(5)]
    public GameObject merchantListPanel;
    public TMP_Text itemTypeLabelText;
    public Transform merchantListContainer;
    public GameObject craftListItemUI;
    public GameObject purchaseListItemUI;
    public GameObject sellListItemUI;

    [Header("Crafting Menu Components")]
    [Space(5)]
    public GameObject craftDetailsPanelParent;
    public GameObject craftDetailsPanel;
    public Button craftDetailsCraftButton;
    public TMP_Text craftItemNameText;
    public Image craftItemIcon;
    public TMP_Text craftItemMainStatText;
    public TMP_Text craftItemSubStatText;
    public GameObject[] craftingMaterialObjects = new GameObject[4];
    public Image[] craftMaterialIcons = new Image[4];
    public TMP_Text[] craftMaterialNames = new TMP_Text[4];
    public TMP_Text[] availableQuantityText = new TMP_Text[4];
    public TMP_Text craftingCostText;
    public Color insufficientMaterialsColor;
    public Color defaultTextColor;

    [Header("Craft Success Panel Components")]
    [Space(5)]
    public GameObject craftSuccessPanel;
    public Image craftSuccessItemIcon;
    public TMP_Text craftSuccessItemNameText;

    // Private Variables
    private BlacksmithCraftItemSO activeCraftItem;
    private int currentMenuID = 1;
    private enum SelectedWeightType { Light, Medium, Heavy};
    private SelectedWeightType selectedWeightType = SelectedWeightType.Medium;


    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Trying to create more than one instance of BlacksmithManager !");
            Destroy(gameObject);
        }
        instance = this;
    }

    public void OpenCraftingDetails(BlacksmithCraftItemSO craftItem)
    {
        // Save the craftItem to a global variable for other functions' use
        activeCraftItem = craftItem;

        // Start a material availability counter, which checks if all items are available for crafting
        int totalMaterialsAvailable = 0;
        bool sufficientGoldAvailable = false;

        // Load the data into the crafting details panel's UI
        craftItemNameText.text = craftItem.itemName;
        if(craftItem.itemIcon != null)
        {
            craftItemIcon.sprite = craftItem.itemIcon;
        }

        // ------------- Load the main stat text -------------
        if (craftItem.mainStatType == Item.MainStatType.ATK)
        {
            craftItemMainStatText.text = "ATK ";
        }
        else
        {
            craftItemMainStatText.text = "DEF ";
        }
        // Add the main stat value to the main stat text
        craftItemMainStatText.text += "+" + craftItem.mainStatValue.ToString();
        // ---------------------------------------------------

        // ------------- Load the sub stat text -------------
        if (craftItem.subStatType == Item.SubStatType.None)
        {
            craftItemSubStatText.text = "-";
        }
        else if (craftItem.subStatType == Item.SubStatType.CR)
        {
            craftItemSubStatText.text = "CR ";
        }
        else if (craftItem.subStatType == Item.SubStatType.CD)
        {
            craftItemSubStatText.text = "CD ";
        }
        else if (craftItem.subStatType == Item.SubStatType.ATK)
        {
            craftItemSubStatText.text = "ATK ";
        }
        else if (craftItem.subStatType == Item.SubStatType.DEF)
        {
            craftItemSubStatText.text = "DEF ";
        }
        else if (craftItem.subStatType == Item.SubStatType.MP)
        {
            craftItemSubStatText.text = "MP ";
        }
        // Add the sub stat value to the sub stat text
        craftItemSubStatText.text += "+" + craftItem.subStatValue.ToString();
        // Add a % symbol if the substat is CR or CD
        if (craftItem.subStatType == Item.SubStatType.CR || craftItem.subStatType == Item.SubStatType.CD)
        {
            craftItemSubStatText.text += "%";
        }
        // --------------------------------------------------

        // ------------------ Load the Material Icons, Names and Quantities ------------------
        // Load the first material
        if (craftItem.totalMaterialTypes >= 1) 
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
            foreach(Item item in GameData.instance.inventory)
            {
                if(item.itemName == craftItem.craftingMaterial1.itemName)
                {
                    availableQuantity = item.itemQuantity;
                }
            }
            availableQuantityText[0].text = availableQuantity.ToString();
            // if available quantity is less than required, highlight it in red
            if(availableQuantity < craftItem.materialQuantity1)
            {
                availableQuantityText[0].color = insufficientMaterialsColor;
            }
            else
            {
                availableQuantityText[0].color = defaultTextColor;
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
                availableQuantityText[1].color = defaultTextColor;
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
                availableQuantityText[2].color = defaultTextColor;
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
                availableQuantityText[3].color = defaultTextColor;
                totalMaterialsAvailable++;
            }
        }
        // -----------------------------------------------------------------------------------

        // Load the crafting cost text
        craftingCostText.text = craftItem.craftingCost.ToString();
        // highlight the text in red if gold is insufficient
        if(craftItem.craftingCost > GameData.instance.goldCoins)
        {
            craftingCostText.color = insufficientMaterialsColor;
        }
        else
        {
            craftingCostText.color = defaultTextColor;
            sufficientGoldAvailable = true;
        }

        // Set the craft button active if all conditions are met, else disable it
        if(totalMaterialsAvailable == craftItem.totalMaterialTypes && sufficientGoldAvailable)
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


    // ------------------------ BUTTON FUNCTIONS ------------------------

    public void OnBackButton()
    {
        AudioManager.instance.PlayUISound("uiClick2");

        if(currentMenuID == 2)
        {
            craftOptionsPanel.gameObject.SetActive(false);
            mainMenuPanel.gameObject.SetActive(true);

            backButton.gameObject.SetActive(false);
            currentMenuID = 1;
        }
        else if(currentMenuID == 3)
        {
            armorWeightOptionsPanel.gameObject.SetActive(false);
            craftOptionsPanel.gameObject.SetActive(true);
            currentMenuID = 2;
        }
        else if(currentMenuID == 4)
        {
            armorTypeOptionsPanel.gameObject.SetActive(false);
            armorWeightOptionsPanel.gameObject.SetActive(true);
            currentMenuID = 3;
        }
    }

    public void OnCraftButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        mainMenuPanel.gameObject.SetActive(false);
        craftOptionsPanel.gameObject.SetActive(true);

        backButton.gameObject.SetActive(true);
        currentMenuID = 2;
    }

    public void OnPurchaseButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        // First we turn off bromund's sprite
        bromundChar.gameObject.SetActive(false);

        // Clear all old objects in the items list panel if they exist
        ClearMerchantListPanel();

        // Next we instantiate all pruchasable items which are available at current level and child them to item list container
        foreach (ItemSO purchaseItem in purchaseItems)
        {
            if(purchaseItem.unlockLevel <= GameData.instance.aldenLevel)
            {
                PurchaseItemUI purchaseItemUIObject = Instantiate(purchaseListItemUI, merchantListContainer).GetComponent<PurchaseItemUI>();
                // Save the script reference for later use
                purchaseItemUIs.Add(purchaseItemUIObject);
                purchaseItemUIObject.Initialise(purchaseItem);
            }
        }

        // Next we update the item type label of the merchant panel
        itemTypeLabelText.text = "Purchase Materials";

        // Next we display the list with all purchasable items
        merchantListPanel.gameObject.SetActive(true);
    }

    public void OnSellButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        // First we turn off bromund's sprite
        bromundChar.gameObject.SetActive(false);

        // Clear all old objects in the items list panel if they exist
        ClearMerchantListPanel();

        // Next we instantiate all items in the inventory (excluding equipped items) and child them to item list container
        foreach (Item sellItem in GameData.instance.inventory)
        {
            SellItemUI sellItemUIObject = Instantiate(sellListItemUI, merchantListContainer).GetComponent<SellItemUI>();
            sellItemUIObject.Initialise(sellItem);
        }

        // Next we update the item type label of the merchant panel
        itemTypeLabelText.text = "Sell Items";

        // Next we display the list with all purchasable items
        merchantListPanel.gameObject.SetActive(true);
    }

    public void OnSwordsButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        // First we turn off bromund's sprite
        bromundChar.gameObject.SetActive(false);

        // Clear all old objects in the items list panel if they exist
        ClearMerchantListPanel();

        // Next we instantiate all craftable items of sword type which are available at current level and child them to item list container
        foreach(BlacksmithCraftItemSO swordCraftItem in swordsCraftItems)
        {
            if(swordCraftItem.unlockLevel <= GameData.instance.aldenLevel)
            {
                CraftItemUI craftItemUIObject = Instantiate(craftListItemUI, merchantListContainer).GetComponent<CraftItemUI>();
                craftItemUIObject.Initialise(swordCraftItem);
            }
        }

        // Next we update the item type label of the merchant panel
        itemTypeLabelText.text = "Swords";

        // Next we display the list with all craftable swords
        merchantListPanel.gameObject.SetActive(true);
    }

    public void OnCraftDetailsCraftButton()
    {
        AudioManager.instance.PlayUISound("enhanceComplete");

        // Adding the crafted item into the player's inventory
        Item craftedItem = new Item(activeCraftItem.craftingReward);
        GameData.instance.inventory.Add(craftedItem);

        // Deducting the crafting materials required from the player's inventory
        foreach(Item item in GameData.instance.inventory)
        {
            if (activeCraftItem.totalMaterialTypes >= 1)
            {
                if(activeCraftItem.craftingMaterial1.itemName == item.itemName)
                {
                    item.itemQuantity -= activeCraftItem.materialQuantity1;
                }
            }

            if (activeCraftItem.totalMaterialTypes >= 2)
            {
                if (activeCraftItem.craftingMaterial2.itemName == item.itemName)
                {
                    item.itemQuantity -= activeCraftItem.materialQuantity2;
                }
            }

            if (activeCraftItem.totalMaterialTypes >= 3)
            {
                if (activeCraftItem.craftingMaterial3.itemName == item.itemName)
                {
                    item.itemQuantity -= activeCraftItem.materialQuantity3;
                }
            }

            if (activeCraftItem.totalMaterialTypes == 4)
            {
                if (activeCraftItem.craftingMaterial4.itemName == item.itemName)
                {
                    item.itemQuantity -= activeCraftItem.materialQuantity4;
                }
            }
        }

        // Removing items from inventory if their quantity becomes 0
        GameData.instance.inventory.RemoveAll(item => item.itemQuantity < 1);        // Could cause potential issues, if it doesn't operate as intended (Marked for later changes)

        // Deduct the crafting cost & update it on the gold coins display
        GameData.instance.goldCoins -= activeCraftItem.craftingCost;
        MarketMenuManager.instance.UpdateGoldCoinsDisplay();

        // Display a panel which shows 'crafting success' along with item icon and name
        craftDetailsPanel.gameObject.SetActive(false);
        craftSuccessPanel.gameObject.SetActive(true);

        // Now load the icon and the name into the success panel
        craftSuccessItemIcon.sprite = craftedItem.itemIcon;
        craftSuccessItemNameText.text = craftedItem.itemName;
    }

    public void OnCraftSuccessConfirmButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        craftSuccessPanel.gameObject.SetActive(false);
        craftDetailsPanel.gameObject.SetActive(true);
        craftDetailsPanelParent.gameObject.SetActive(false);
    }

    public void OnCraftDetailsCancelButton()
    {
        AudioManager.instance.PlayUISound("uiClick2");

        craftDetailsPanelParent.gameObject.SetActive(false);

        for(int i = 0; i < 4; i++)
        {
            craftingMaterialObjects[i].gameObject.SetActive(false);
        }
    }

    public void OnArmorsButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        craftOptionsPanel.gameObject.SetActive(false);
        armorWeightOptionsPanel.gameObject.SetActive(true);

        currentMenuID = 3;
    }

    public void OnMediumArmorButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        selectedWeightType = SelectedWeightType.Medium;

        armorWeightOptionsPanel.gameObject.SetActive(false);
        armorTypeOptionsPanel.gameObject.SetActive(true);

        currentMenuID = 4;
    }

    public void OnHeadgearButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        // First we turn off bromund's sprite
        bromundChar.gameObject.SetActive(false);

        // Clear all old objects in the items list panel if they exist
        ClearMerchantListPanel();

        // loading list of all medium headgears
        if (selectedWeightType == SelectedWeightType.Medium)
        {
            // Next we instantiate all craftable items of Headgear type which are available at current level and child them to item list container
            foreach (BlacksmithCraftItemSO mediumHeadgearCraftItem in mediumHeadgearCraftItems)
            {
                if (mediumHeadgearCraftItem.unlockLevel <= GameData.instance.aldenLevel)
                {
                    CraftItemUI craftItemUIObject = Instantiate(craftListItemUI, merchantListContainer).GetComponent<CraftItemUI>();
                    craftItemUIObject.Initialise(mediumHeadgearCraftItem);
                }
            }
        }
        // CONTINUE THIS CHAIN TO HANDLE OTHER WEIGHT TYPES

        // Next we update the item type label of the merchant panel
        itemTypeLabelText.text = "Medium Headgear";

        // Next we display the list with all craftable Headgear
        merchantListPanel.gameObject.SetActive(true);
    }

    public void OnChestpieceButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        // First we turn off bromund's sprite
        bromundChar.gameObject.SetActive(false);

        // Clear all old objects in the items list panel if they exist
        ClearMerchantListPanel();

        // loading list of all medium Chestpiece
        if (selectedWeightType == SelectedWeightType.Medium)
        {
            // Next we instantiate all craftable items of Chestpiece type which are available at current level and child them to item list container
            foreach (BlacksmithCraftItemSO mediumChestpieceCraftItem in mediumChestpieceCraftItems)
            {
                if (mediumChestpieceCraftItem.unlockLevel <= GameData.instance.aldenLevel)
                {
                    CraftItemUI craftItemUIObject = Instantiate(craftListItemUI, merchantListContainer).GetComponent<CraftItemUI>();
                    craftItemUIObject.Initialise(mediumChestpieceCraftItem);
                }
            }
        }
        // CONTINUE THIS CHAIN TO HANDLE OTHER WEIGHT TYPES

        // Next we update the item type label of the merchant panel
        itemTypeLabelText.text = "Medium Chestpiece";

        // Next we display the list with all craftable Chestpiece
        merchantListPanel.gameObject.SetActive(true);
    }

    public void OnLegguardsButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        // First we turn off bromund's sprite
        bromundChar.gameObject.SetActive(false);

        // Clear all old objects in the items list panel if they exist
        ClearMerchantListPanel();

        // loading list of all medium Legguards
        if (selectedWeightType == SelectedWeightType.Medium)
        {
            // Next we instantiate all craftable items of Legguards type which are available at current level and child them to item list container
            foreach (BlacksmithCraftItemSO mediumLegguardsCraftItem in mediumLegguardsCraftItems)
            {
                if (mediumLegguardsCraftItem.unlockLevel <= GameData.instance.aldenLevel)
                {
                    CraftItemUI craftItemUIObject = Instantiate(craftListItemUI, merchantListContainer).GetComponent<CraftItemUI>();
                    craftItemUIObject.Initialise(mediumLegguardsCraftItem);
                }
            }
        }
        // CONTINUE THIS CHAIN TO HANDLE OTHER WEIGHT TYPES

        // Next we update the item type label of the merchant panel
        itemTypeLabelText.text = "Medium Legguards";

        // Next we display the list with all craftable Legguards
        merchantListPanel.gameObject.SetActive(true);
    }

    public void OnBootsButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        // First we turn off bromund's sprite
        bromundChar.gameObject.SetActive(false);

        // Clear all old objects in the items list panel if they exist
        ClearMerchantListPanel();

        // loading list of all medium Boots
        if (selectedWeightType == SelectedWeightType.Medium)
        {
            // Next we instantiate all craftable items of Boots type which are available at current level and child them to item list container
            foreach (BlacksmithCraftItemSO mediumBootsCraftItem in mediumBootsCraftItems)
            {
                if (mediumBootsCraftItem.unlockLevel <= GameData.instance.aldenLevel)
                {
                    CraftItemUI craftItemUIObject = Instantiate(craftListItemUI, merchantListContainer).GetComponent<CraftItemUI>();
                    craftItemUIObject.Initialise(mediumBootsCraftItem);
                }
            }
        }
        // CONTINUE THIS CHAIN TO HANDLE OTHER WEIGHT TYPES

        // Next we update the item type label of the merchant panel
        itemTypeLabelText.text = "Medium Boots";

        // Next we display the list with all craftable Boots
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
