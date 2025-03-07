using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance = null;

    public Image weaponImage = null;
    public Image offhandImage = null;
    public Image headgearImage = null;
    public Image chestpieceImage = null;
    public Image legguardsImage = null;
    public Image bootsImage = null;

    public GameObject aldenChar = null;
    public GameObject inventoryPanel = null;
    public GameObject equipButtons = null;

    public GameObject itemUI = null;
    public GameObject equippedItemUI = null;
    public GameObject emptyWeaponItemUI = null;
    public Transform itemListContainer = null;
    public Transform equippedItemPosition = null;

    public ItemSO emptyItemSO = null;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Trying to create more than one InventoryManager instance!");
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
        // Fetch alden's equipped items and display their icons on the equipment buttons, we only need this for alden since he is always loaded first.
        FetchAldenEquippedItems();
    }

    private void FetchAldenEquippedItems()
    {
        if (GameData.instance.aldenEquippedWeapon.itemIcon != null)
        {
            weaponImage.sprite = GameData.instance.aldenEquippedWeapon.itemIcon;
        }
        if (GameData.instance.aldenEquippedOffHand.itemIcon != null)
        {
            offhandImage.sprite = GameData.instance.aldenEquippedOffHand.itemIcon;
        }
        if (GameData.instance.aldenEquippedHeadgear.itemIcon != null)
        {
            headgearImage.sprite = GameData.instance.aldenEquippedHeadgear.itemIcon;
        }
        if (GameData.instance.aldenEquippedChestpiece.itemIcon != null)
        {
            chestpieceImage.sprite = GameData.instance.aldenEquippedChestpiece.itemIcon;
        }
        if (GameData.instance.aldenEquippedLegguards.itemIcon != null)
        {
            legguardsImage.sprite = GameData.instance.aldenEquippedLegguards.itemIcon;
        }
        if (GameData.instance.aldenEquippedBoots.itemIcon != null)
        {
            bootsImage.sprite = GameData.instance.aldenEquippedBoots.itemIcon;
        }
    }

    private void ShowInventory()
    {
        aldenChar.gameObject.SetActive(false);
        equipButtons.gameObject.SetActive(false);
        inventoryPanel.gameObject.SetActive(true);
    }

    private void HideInventory()
    {
        inventoryPanel.gameObject.SetActive(false);
        aldenChar.gameObject.SetActive(true);
        equipButtons.gameObject.SetActive(true);
    }

    public void UnequipItem(Item item)
    {
        if(HeroesMenuManager.instance.activeCharacter == HeroesMenuManager.ActiveCharacter.Alden) // Handle unequipping for Alden
        {
            if(item.itemType == Item.ItemType.Weapon) // If the unequipped item is a weapon
            {
                GameData.instance.inventory.Add(GameData.instance.aldenEquippedWeapon); // Add the unequipped weapon back to inventory
                GameData.instance.aldenEquippedWeapon = new Item(emptyItemSO); // Set alden's equipped item to an empty item.

                // Remove the stats associated with weapon from alden's total stats
                RemoveStats(item);
            }
            // continue this chain to handle other equipment types
        }
        // continue this chain to handle other player characters

        // Change the displayed stats
        HeroesMenuManager.instance.UpdateStatsDisplay();

        // Save changes to GameData - COME BACK TO THIS LATER

        // Hide back the inventory window.
        HideInventory();
    }

    public void EquipItem(Item item)
    {
        // we first remove the currently equipped item's stats from active character before adding stats of new equipped item
        if(item.itemType != Item.ItemType.None)
        {
            if(item.itemType == Item.ItemType.Weapon)
            {
                GameData.instance.inventory.Add(GameData.instance.aldenEquippedWeapon); // Add the unequipped weapon back to inventory
                RemoveStats(GameData.instance.aldenEquippedWeapon);
            }
            // continue this chain for other equipment types
        }

        if (HeroesMenuManager.instance.activeCharacter == HeroesMenuManager.ActiveCharacter.Alden) // Handle equipping for Alden
        {
            if (item.itemType == Item.ItemType.Weapon) // If the equipped item is a weapon
            {
                GameData.instance.aldenEquippedWeapon = item; // Set alden's equipped item to the new item
                GameData.instance.inventory.Remove(item); // Remove the equipped weapon from the inventory - Possible Removal Failure; Note: Seems to be working fine for now.

                // Add the stats associated with weapon to alden's total stats
                AddStats(item);
            }
            // continue this chain for other equipment types
        }

        // Change the displayed stats
        HeroesMenuManager.instance.UpdateStatsDisplay();

        // Save changes to GameData - COME BACK TO THIS LATER

        // Hide back the inventory window.
        HideInventory();
    }

    private void RemoveStats(Item item)
    {
        if(HeroesMenuManager.instance.activeCharacter == HeroesMenuManager.ActiveCharacter.Alden)
        {
            if (item.mainStatType == Item.MainStatType.ATK)  // First removing the main stat values
            {
                GameData.instance.aldenATK -= item.mainStatValue;
            }
            else
            {
                GameData.instance.aldenDEF -= item.mainStatValue;
            }

            if (item.subStatType == Item.SubStatType.ATK) // Then removing the sub stat values
            {
                GameData.instance.aldenATK -= item.subStatValue;
            }
            else if (item.subStatType == Item.SubStatType.CD)
            {
                GameData.instance.aldenCD -= item.subStatValue;
            }
            else if (item.subStatType == Item.SubStatType.CR)
            {
                GameData.instance.aldenCR -= item.subStatValue;
            }
            else if (item.subStatType == Item.SubStatType.DEF)
            {
                GameData.instance.aldenDEF -= item.subStatValue;
            }
            else if (item.subStatType == Item.SubStatType.MP)
            {
                GameData.instance.aldenMP -= item.subStatValue;
            }
        }
        // continue this chain to handle for other player characters
    }

    private void AddStats(Item item)
    {
        if(HeroesMenuManager.instance.activeCharacter == HeroesMenuManager.ActiveCharacter.Alden)
        {
            if (item.mainStatType == Item.MainStatType.ATK)  // First adding the main stat values
            {
                GameData.instance.aldenATK += item.mainStatValue;
            }
            else
            {
                GameData.instance.aldenDEF += item.mainStatValue;
            }

            if (item.subStatType == Item.SubStatType.ATK) // Then adding the sub stat values
            {
                GameData.instance.aldenATK += item.subStatValue;
            }
            else if (item.subStatType == Item.SubStatType.CD)
            {
                GameData.instance.aldenCD += item.subStatValue;
            }
            else if (item.subStatType == Item.SubStatType.CR)
            {
                GameData.instance.aldenCR += item.subStatValue;
            }
            else if (item.subStatType == Item.SubStatType.DEF)
            {
                GameData.instance.aldenDEF += item.subStatValue;
            }
            else if (item.subStatType == Item.SubStatType.MP)
            {
                GameData.instance.aldenMP += item.subStatValue;
            }
        }
        // Continue this chain to handle for other player characters
    }

    // ------------------------ Button Functions -----------------------------
    public void OnWeaponButton()
    {
        // Clear all old instantiated items
        Destroy(equippedItemPosition.GetChild(0).gameObject);

        foreach (Transform child in itemListContainer)
        {
            Destroy(child.gameObject);
        }

        // Fetch the equipped weapon for the active character and display it in the equipped slot
        if(HeroesMenuManager.instance.activeCharacter == HeroesMenuManager.ActiveCharacter.Alden)
        {
            if(GameData.instance.aldenEquippedWeapon.weaponType != Item.WeaponType.None)
            {
                ItemUI equippedItemUIObject = Instantiate(equippedItemUI, equippedItemPosition).GetComponent<ItemUI>();
                equippedItemUIObject.Initialise(GameData.instance.aldenEquippedWeapon);
            }
            else
            {
                Instantiate(emptyWeaponItemUI, equippedItemPosition);
            }
        }
        // Continue this chain for handling other characters

        // fetch the available weapon items from the inventory which are suitable for the active character
        List<Item> weaponItemsList = new List<Item>();

        foreach(Item item in GameData.instance.inventory)
        {
            if(HeroesMenuManager.instance.activeCharacter == HeroesMenuManager.ActiveCharacter.Alden)  // if active character is alden, we select all items that are a weapon of type sword.
            {
                if(item.itemType == Item.ItemType.Weapon && item.weaponType == Item.WeaponType.Sword)
                {
                    weaponItemsList.Add(item);
                }
            }
        }

        // Sort the list to show the weapon with the highest stat at the top
        weaponItemsList.Sort((a, b) => b.mainStatValue.CompareTo(a.mainStatValue));

        // Instantiate all the items in the list and make them children of the scrolling area container
        foreach (Item item in weaponItemsList)
        {
            ItemUI itemUIObject = Instantiate(itemUI, itemListContainer).GetComponent<ItemUI>();
            itemUIObject.Initialise(item);
        }

        ShowInventory();
    }

    public void OnOffhandButton()
    {

    }

    public void OnHeadgearButton()
    {

    }

    public void OnChestPieceButton()
    {

    }

    public void OnLegguardsButton()
    {

    }

    public void OnBootsButton()
    {

    }

    public void OnCloseInventoryButton()
    {
        HideInventory();
    }
}
