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
    public Color defaultTextColor;

    [Header("Enhancing Success Panel Components")]
    [Space(5)]
    public GameObject enhancingSuccessPanel;
    public Image enhancingSuccessItemIcon;
    public TMP_Text enhancingSuccessItemNameText;

    [Header("Materials Dictionary")]
    [Space(5)]
    public List<ItemSO> materialsList = new List<ItemSO>();
    private Dictionary<string, ItemSO> materialDictionary = new Dictionary<string, ItemSO>();

    // Private Variables
    private int currentMenuID = 1;

    private Item activeEnhanceItem;
    private EnhanceItemUI activeEnhanceItemUI;
    private bool isCurrentlyEquipped = false;
    private EnhanceItemUI.EquippedCharName currentlyEquippedCharName = EnhanceItemUI.EquippedCharName.None;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Trying to create more than one instance of AlchemistManager !");
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
        foreach(var material in materialsList)
        {
            materialDictionary.Add(material.itemName, material);
        }
    }

    public void OpenEnhancingDetails(Item enhanceItem, bool isEquipped, EnhanceItemUI.EquippedCharName equippedCharName, EnhanceItemUI enhanceItemUI)
    {
        // Save the reference to enhanceItem to a global variable for other functions' use
        activeEnhanceItem = enhanceItem;
        activeEnhanceItemUI = enhanceItemUI;

        // Save equip data to global variables
        isCurrentlyEquipped = isEquipped;
        currentlyEquippedCharName = equippedCharName;

        // Start a material availability counter, which checks if all items are available for enhancing
        int totalMaterialsAvailable = 0;
        bool sufficientGoldAvailable = false;

        // Load the data into the enhancing details panel's UI
        enhanceItemNameText.text = enhanceItem.itemName + " +" + enhanceItem.enhancementLevel.ToString();
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

        // ------------- Load the enhanced main stat preview text -------------
        enhancedItemMainStatText.text = FindEnhancedMainStatValue(activeEnhanceItem.mainStatValue).ToString();
        bool subStatIsCritRate = false;
        if(activeEnhanceItem.subStatType == Item.SubStatType.CR)
        {
            subStatIsCritRate = true;
        }
        enhancedItemSubStatText.text = FindEnhancedSubStatValue(activeEnhanceItem.subStatValue, subStatIsCritRate).ToString();

        // ------------------ Load the Material Icons, Names and Quantities ------------------
        int enhancementLevel = enhanceItem.enhancementLevel;

        // Load the first material
        if (enhanceItem.upgradeMaterialsList[enhancementLevel].totalMaterialTypes >= 1)
        {
            // Setting the material active
            enhancingMaterialObjects[0].gameObject.SetActive(true);

            // loading the icon
            enhancingMaterialIcons[0].sprite = materialDictionary[enhanceItem.upgradeMaterialsList[enhancementLevel].materialName1].itemIcon;

            // loading the name
            enhancingMaterialNames[0].text = enhanceItem.upgradeMaterialsList[enhancementLevel].materialName1;

            // loading the quanities into the name
            enhancingMaterialNames[0].text += " X " + enhanceItem.upgradeMaterialsList[enhancementLevel].materialQuantity1.ToString();

            // loading the available quantities of the material from player inventory
            int availableQuantity = 0;
            foreach (Item item in GameData.instance.inventory)
            {
                if (item.itemName == enhanceItem.upgradeMaterialsList[enhancementLevel].materialName1)
                {
                    availableQuantity = item.itemQuantity;
                }
            }
            availableQuantityText[0].text = availableQuantity.ToString();
            // if available quantity is less than required, highlight it in red
            if (availableQuantity < enhanceItem.upgradeMaterialsList[enhancementLevel].materialQuantity1)
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
        if (enhanceItem.upgradeMaterialsList[enhancementLevel].totalMaterialTypes >= 2)
        {
            // Setting the material active
            enhancingMaterialObjects[1].gameObject.SetActive(true);

            // loading the icon
            enhancingMaterialIcons[1].sprite = materialDictionary[enhanceItem.upgradeMaterialsList[enhancementLevel].materialName2].itemIcon;

            // loading the name
            enhancingMaterialNames[1].text = enhanceItem.upgradeMaterialsList[enhancementLevel].materialName2;

            // loading the quanities into the name
            enhancingMaterialNames[1].text += " X " + enhanceItem.upgradeMaterialsList[enhancementLevel].materialQuantity2.ToString();

            // loading the available quantities of the material from player inventory
            int availableQuantity = 0;
            foreach (Item item in GameData.instance.inventory)
            {
                if (item.itemName == enhanceItem.upgradeMaterialsList[enhancementLevel].materialName2)
                {
                    availableQuantity = item.itemQuantity;
                }
            }
            availableQuantityText[1].text = availableQuantity.ToString();
            // if available quantity is less than required, highlight it in red
            if (availableQuantity < enhanceItem.upgradeMaterialsList[enhancementLevel].materialQuantity2)
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
        if (enhanceItem.upgradeMaterialsList[enhancementLevel].totalMaterialTypes >= 3)
        {
            // Setting the material active
            enhancingMaterialObjects[2].gameObject.SetActive(true);

            // loading the icon
            enhancingMaterialIcons[2].sprite = materialDictionary[enhanceItem.upgradeMaterialsList[enhancementLevel].materialName3].itemIcon;

            // loading the name
            enhancingMaterialNames[2].text = enhanceItem.upgradeMaterialsList[enhancementLevel].materialName3;

            // loading the quanities into the name
            enhancingMaterialNames[2].text += " X " + enhanceItem.upgradeMaterialsList[enhancementLevel].materialQuantity3.ToString();

            // loading the available quantities of the material from player inventory
            int availableQuantity = 0;
            foreach (Item item in GameData.instance.inventory)
            {
                if (item.itemName == enhanceItem.upgradeMaterialsList[enhancementLevel].materialName3)
                {
                    availableQuantity = item.itemQuantity;
                }
            }
            availableQuantityText[2].text = availableQuantity.ToString();
            // if available quantity is less than required, highlight it in red
            if (availableQuantity < enhanceItem.upgradeMaterialsList[enhancementLevel].materialQuantity3)
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
        if (enhanceItem.upgradeMaterialsList[enhancementLevel].totalMaterialTypes >= 4)
        {
            // Setting the material active
            enhancingMaterialObjects[3].gameObject.SetActive(true);

            // loading the icon
            enhancingMaterialIcons[3].sprite = materialDictionary[enhanceItem.upgradeMaterialsList[enhancementLevel].materialName4].itemIcon;

            // loading the name
            enhancingMaterialNames[3].text = enhanceItem.upgradeMaterialsList[enhancementLevel].materialName4;

            // loading the quanities into the name
            enhancingMaterialNames[3].text += " X " + enhanceItem.upgradeMaterialsList[enhancementLevel].materialQuantity4.ToString();

            // loading the available quantities of the material from player inventory
            int availableQuantity = 0;
            foreach (Item item in GameData.instance.inventory)
            {
                if (item.itemName == enhanceItem.upgradeMaterialsList[enhancementLevel].materialName4)
                {
                    availableQuantity = item.itemQuantity;
                }
            }
            availableQuantityText[3].text = availableQuantity.ToString();
            // if available quantity is less than required, highlight it in red
            if (availableQuantity < enhanceItem.upgradeMaterialsList[enhancementLevel].materialQuantity4)
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

        // Load the enhancing cost text
        enhancingCostText.text = enhanceItem.upgradeMaterialsList[enhancementLevel].upgradeCost.ToString();
        // highlight the text in red if gold is insufficient
        if (enhanceItem.upgradeMaterialsList[enhancementLevel].upgradeCost > GameData.instance.goldCoins)
        {
            enhancingCostText.color = insufficientMaterialsColor;
        }
        else
        {
            enhancingCostText.color = defaultTextColor;
            sufficientGoldAvailable = true;
        }

        // Set the enhance button active if all conditions are met, else disable it
        if (totalMaterialsAvailable == enhanceItem.upgradeMaterialsList[enhancementLevel].totalMaterialTypes && sufficientGoldAvailable)
        {
            enhancingDetailsEnhanceButton.interactable = true;
        }
        else
        {
            enhancingDetailsEnhanceButton.interactable = false;
        }

        // Turn off Agatha's Character sprite
        agathaChar.gameObject.SetActive(false);

        // Open the enhancing details panel
        enhancingDetailsPanelParent.gameObject.SetActive(true);
    }


    // ------------------------ BUTTON FUNCTIONS ------------------------

    public void OnEnhanceButton()
    {
        mainMenuPanel.gameObject.SetActive(false);
        enhanceOptionsPanel.gameObject.SetActive(true);

        backButton.gameObject.SetActive(true);
        currentMenuID = 2;
    }

    public void OnPurchaseButton()
    {
        // First we turn off bromund's sprite
        agathaChar.gameObject.SetActive(false);

        // Clear all old objects in the items list panel if they exist
        ClearMerchantListPanel();

        // Next we instantiate all pruchasable items which are available at current level and child them to item list container
        foreach (ItemSO purchaseItem in purchaseItems)
        {
            if (purchaseItem.unlockLevel <= GameData.instance.aldenLevel)
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
        // First we turn off bromund's sprite
        agathaChar.gameObject.SetActive(false);

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

    public void OnWeaponsButton()
    {
        // Need to load all the available weapons in the players inventory, as well as weapons that are equipped by characters

        // First we turn off agatha's sprite
        agathaChar.gameObject.SetActive(false);

        // Clear all old objects in the items list panel if they exist
        ClearMerchantListPanel();

        // Next we instantiate all weapon items in the inventory and also those equipped by player characters

        // Adding Alden's equipped weapon to the list
        if(GameData.instance.aldenEquippedWeapon.itemType != Item.ItemType.None) // checking if the equipped item isn't empty
        {
            EnhanceItemUI aldenEnhanceItemUIObject = Instantiate(enhanceListItemUI, merchantListContainer).GetComponent<EnhanceItemUI>();
            aldenEnhanceItemUIObject.Initialise(GameData.instance.aldenEquippedWeapon, true, EnhanceItemUI.EquippedCharName.Alden);
        }
        // ADD HANDLING FOR OTHER CHARACTERS HERE

        // Adding all weapon items from inventory

        // We sort the inventory list into a new list so that we can display the items with highest level at the top
        List<Item> weaponItemsList = new List<Item>();
        foreach (Item item in GameData.instance.inventory) 
        {
            if (item.itemType == Item.ItemType.Weapon)
            {
                weaponItemsList.Add(item);
            }
        }

        // Sort the list to show the weapon with the highest unlock level at the top
        weaponItemsList.Sort((a, b) => b.mainStatValue.CompareTo(a.unlockLevel));

        foreach (Item enhanceItem in weaponItemsList) // Instantiate all items from the sorted weaponItemsList onto the merchant panel
        {
            EnhanceItemUI enhanceItemUIObject = Instantiate(enhanceListItemUI, merchantListContainer).GetComponent<EnhanceItemUI>();
            enhanceItemUIObject.Initialise(enhanceItem, false, EnhanceItemUI.EquippedCharName.None);
        }

        // Next we update the item type label of the merchant panel
        itemTypeLabelText.text = "Weapon Enhancement";

        // Next we display the list with all craftable swords
        merchantListPanel.gameObject.SetActive(true);
    }


    public void OnEnhancingDetailsEnhanceButton()
    {
        // Changing the equipment's stats to the new, enhanced stats:

        // We keep a copy of the base values for reference
        int originalMainStatValue = activeEnhanceItem.mainStatValue;
        int originalSubStatValue = activeEnhanceItem.subStatValue;

        // We create variables to hold new values
        bool substatIsCritRate = false;
        if(activeEnhanceItem.subStatType == Item.SubStatType.CR)
        {
            substatIsCritRate = true;
        }

        int newMainStatValue = originalMainStatValue;
        int newSubStatValue = originalSubStatValue;

        // next, we check the equipment's rarity and then enhance it's values by the tier's step multiplier to get new, upgraded values.
        // Note: if the stat is crit rate, we do not modify it and let it remain static.

        newMainStatValue = FindEnhancedMainStatValue(originalMainStatValue);
        newSubStatValue = FindEnhancedSubStatValue(originalSubStatValue, substatIsCritRate);

        // Update the item with the new stats
        activeEnhanceItem.mainStatValue = newMainStatValue;
        activeEnhanceItem.subStatValue = newSubStatValue;

        // If the item was equipped, then we also need to modify the character's stats who equipped it
        if (isCurrentlyEquipped)
        {
            if(currentlyEquippedCharName == EnhanceItemUI.EquippedCharName.Alden) // Handling for alden
            {
                // we must first subtract the weapon's previous value from it's new value to find the difference in stats (the stat increase value)
                int mainStatDifference = newMainStatValue - originalMainStatValue;
                int subStatDifference = newSubStatValue - originalSubStatValue;

                // we then add this stat increase value to the equipping character's stats

                // Adding Main Stat values
                if (activeEnhanceItem.mainStatType == Item.MainStatType.ATK)
                {
                    GameData.instance.aldenATK += mainStatDifference;
                }
                else
                {
                    GameData.instance.aldenDEF += mainStatDifference;
                }

                // Adding sub stat values
                if(activeEnhanceItem.subStatType == Item.SubStatType.ATK)
                {
                    GameData.instance.aldenATK += subStatDifference;
                }
                else if(activeEnhanceItem.subStatType == Item.SubStatType.CD)
                {
                    GameData.instance.aldenCD += subStatDifference;
                }
                else if (activeEnhanceItem.subStatType == Item.SubStatType.DEF)
                {
                    GameData.instance.aldenDEF += subStatDifference;
                }
                else if (activeEnhanceItem.subStatType == Item.SubStatType.MP)
                {
                    GameData.instance.aldenMP += subStatDifference;
                }
            }
            // CONTINUE THIS CHAIN TO HANDLE OTHER CHARACTERS
        }

        // Deducting the enhancing materials required from the player's inventory
        foreach (Item item in GameData.instance.inventory)
        {
            if (activeEnhanceItem.upgradeMaterialsList[activeEnhanceItem.enhancementLevel].totalMaterialTypes >= 1)
            {
                if (activeEnhanceItem.upgradeMaterialsList[activeEnhanceItem.enhancementLevel].materialName1 == item.itemName)
                {
                    item.itemQuantity -= activeEnhanceItem.upgradeMaterialsList[activeEnhanceItem.enhancementLevel].materialQuantity1;
                }
            }

            if (activeEnhanceItem.upgradeMaterialsList[activeEnhanceItem.enhancementLevel].totalMaterialTypes >= 2)
            {
                if (activeEnhanceItem.upgradeMaterialsList[activeEnhanceItem.enhancementLevel].materialName2 == item.itemName)
                {
                    item.itemQuantity -= activeEnhanceItem.upgradeMaterialsList[activeEnhanceItem.enhancementLevel].materialQuantity2;
                }
            }

            if (activeEnhanceItem.upgradeMaterialsList[activeEnhanceItem.enhancementLevel].totalMaterialTypes >= 3)
            {
                if (activeEnhanceItem.upgradeMaterialsList[activeEnhanceItem.enhancementLevel].materialName3 == item.itemName)
                {
                    item.itemQuantity -= activeEnhanceItem.upgradeMaterialsList[activeEnhanceItem.enhancementLevel].materialQuantity3;
                }
            }

            if (activeEnhanceItem.upgradeMaterialsList[activeEnhanceItem.enhancementLevel].totalMaterialTypes == 4)
            {
                if (activeEnhanceItem.upgradeMaterialsList[activeEnhanceItem.enhancementLevel].materialName4 == item.itemName)
                {
                    item.itemQuantity -= activeEnhanceItem.upgradeMaterialsList[activeEnhanceItem.enhancementLevel].materialQuantity4;
                }
            }
        }

        // Deduct the crafting cost & update it on the gold coins display
        GameData.instance.goldCoins -= activeEnhanceItem.upgradeMaterialsList[activeEnhanceItem.enhancementLevel].upgradeCost;
        MarketMenuManager.instance.UpdateGoldCoinsDisplay();

        // Increment the equipment's enhancement level
        activeEnhanceItem.enhancementLevel++;

        // Display a panel which shows 'Enhancement success' along with item icon and name
        enhancingDetailsPanel.gameObject.SetActive(false);
        enhancingSuccessPanel.gameObject.SetActive(true);

        // Now load the icon and the name into the success panel
        enhancingSuccessItemIcon.sprite = activeEnhanceItem.itemIcon;
        enhancingSuccessItemNameText.text = activeEnhanceItem.itemName;
        enhancingSuccessItemNameText.text += " +" + activeEnhanceItem.enhancementLevel.ToString();

        // Update the enhance item in the merchant list to enable or disable it's enhance button
        activeEnhanceItemUI.UpdateEnhanceButtonActive();

        // update the name of the enhanced item in the merchant list
        activeEnhanceItemUI.UpdateUI();

        // Save the changes
    }

    public void OnEnhancingDetailsCancelButton()
    {
        enhancingDetailsPanelParent.gameObject.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            enhancingMaterialObjects[i].gameObject.SetActive(false);
        }
    }

    public void OnEnhancingSuccessConfirmButton()
    {
        enhancingSuccessPanel.gameObject.SetActive(false);
        enhancingDetailsPanel.gameObject.SetActive(true);
        enhancingDetailsPanelParent.gameObject.SetActive(false);
    }

    public void OnBackButton()
    {
        if (currentMenuID == 2)
        {
            enhanceOptionsPanel.gameObject.SetActive(false);
            mainMenuPanel.gameObject.SetActive(true);

            backButton.gameObject.SetActive(false);
            currentMenuID = 1;
        }
    }

    private void ClearMerchantListPanel()
    {
        foreach (Transform child in merchantListContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private int FindEnhancedMainStatValue(int originalMainStatValue)
    {
        int newMainStatValue = originalMainStatValue;

        if (activeEnhanceItem.itemRarity == Item.ItemRarity.Common) // handling Common rarity
        {
            // Enhancing main stat
            newMainStatValue = Mathf.CeilToInt(originalMainStatValue + (originalMainStatValue * 0.03f));
        }
        else if (activeEnhanceItem.itemRarity == Item.ItemRarity.Uncommon) // handling Uncommon rarity
        {
            // Enhancing main stat
            newMainStatValue = Mathf.CeilToInt(originalMainStatValue + (originalMainStatValue * 0.05f));
        }
        else if (activeEnhanceItem.itemRarity == Item.ItemRarity.Rare) // handling Rare rarity
        {
            // Enhancing main stat
            newMainStatValue = Mathf.CeilToInt(originalMainStatValue + (originalMainStatValue * 0.07f));
        }
        else if (activeEnhanceItem.itemRarity == Item.ItemRarity.Mystic) // handling Mystic rarity
        {
            // Enhancing main stat
            newMainStatValue = Mathf.CeilToInt(originalMainStatValue + (originalMainStatValue * 0.10f));
        }
        else if (activeEnhanceItem.itemRarity == Item.ItemRarity.Epic) // handling Epic rarity
        {
            // Enhancing main stat
            newMainStatValue = Mathf.CeilToInt(originalMainStatValue + (originalMainStatValue * 0.12f));
        }
        else if (activeEnhanceItem.itemRarity == Item.ItemRarity.Legendary) // handling Legendary rarity
        {
            // Enhancing main stat
            newMainStatValue = Mathf.CeilToInt(originalMainStatValue + (originalMainStatValue * 0.15f));
        }

        return newMainStatValue;
    }

    private int FindEnhancedSubStatValue(int originalSubStatValue, bool substatIsCritRate)
    {
        int newSubStatValue = originalSubStatValue;

        if (activeEnhanceItem.itemRarity == Item.ItemRarity.Common) // handling Common rarity
        {
            // Enhancing sub stat
            if (substatIsCritRate == false)
            {
                newSubStatValue = Mathf.CeilToInt(originalSubStatValue + (originalSubStatValue * 0.03f));
            }
        }
        else if (activeEnhanceItem.itemRarity == Item.ItemRarity.Uncommon) // handling Uncommon rarity
        {
            // Enhancing sub stat
            if (substatIsCritRate == false)
            {
                newSubStatValue = Mathf.CeilToInt(originalSubStatValue + (originalSubStatValue * 0.05f));
            }
        }
        else if (activeEnhanceItem.itemRarity == Item.ItemRarity.Rare) // handling Rare rarity
        {
            // Enhancing sub stat
            if (substatIsCritRate == false)
            {
                newSubStatValue = Mathf.CeilToInt(originalSubStatValue + (originalSubStatValue * 0.07f));
            }
        }
        else if (activeEnhanceItem.itemRarity == Item.ItemRarity.Mystic) // handling Mystic rarity
        {
            // Enhancing sub stat
            if (substatIsCritRate == false)
            {
                newSubStatValue = Mathf.CeilToInt(originalSubStatValue + (originalSubStatValue * 0.10f));
            }
        }
        else if (activeEnhanceItem.itemRarity == Item.ItemRarity.Epic) // handling Epic rarity
        {
            // Enhancing sub stat
            if (substatIsCritRate == false)
            {
                newSubStatValue = Mathf.CeilToInt(originalSubStatValue + (originalSubStatValue * 0.12f));
            }
        }
        else if (activeEnhanceItem.itemRarity == Item.ItemRarity.Legendary) // handling Legendary rarity
        {
            // Enhancing sub stat
            if (substatIsCritRate == false)
            {
                newSubStatValue = Mathf.CeilToInt(originalSubStatValue + (originalSubStatValue * 0.15f));
            }
        }

        return newSubStatValue;
    }
}