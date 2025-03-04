using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public Image weaponImage = null;
    public Image offhandImage = null;
    public Image headgearImage = null;
    public Image chestpieceImage = null;
    public Image legguardsImage = null;
    public Image bootsImage = null;

    public Animator aldenCharAnimator = null;
    public Animator inventoryPanelAnimator = null;
    public GameObject itemUI = null;
    public GameObject equippedItemUI = null;
    public Transform itemListContainer = null;
    public Transform equippedItemPosition = null;

    private enum ActiveCharacter { Alden, Valric, Osmir, Assassin};
    private ActiveCharacter activeCharacter = ActiveCharacter.Alden;

    /*private List<Item> weaponItems = new List<Item>();
    private List<Item> offhandItems = new List<Item>();
    private List<Item> headgearItems = new List<Item>();
    private List<Item> chestpieceItems = new List<Item>();
    private List<Item> legguardsItems = new List<Item>();
    private List<Item> bootsItems = new List<Item>(); */

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
        aldenCharAnimator.SetTrigger("FadeOut");
        inventoryPanelAnimator.SetTrigger("SlideIn");
    }

    private void HideInventory()
    {
        inventoryPanelAnimator.SetTrigger("SlideOut");
        aldenCharAnimator.SetTrigger("FadeIn");
    }


    // ------------------------ Button Functions -----------------------------
    public void OnWeaponButton()
    {
        // Fetch the equipped weapon for the active character and display it in the equipped slot
        if(activeCharacter == ActiveCharacter.Alden)
        {
            ItemUI equippedItemUIObject = Instantiate(equippedItemUI, equippedItemPosition).GetComponent<ItemUI>();
            equippedItemUIObject.Initialise(GameData.instance.aldenEquippedWeapon);
        }

        // fetch the available weapon items from the inventory which are suitable for the active character
        List<Item> weaponItemsList = new List<Item>();

        foreach(Item item in GameData.instance.inventory)
        {
            if(activeCharacter == ActiveCharacter.Alden)  // if active character is alden, we select all items that are a weapon of type sword.
            {
                if(item.itemType == Item.ItemType.Weapon && item.weaponType == Item.WeaponType.Sword)
                {
                    weaponItemsList.Add(item);
                }
            }
        }

        // Instantiate all the items in the list and make them children of the scrolling area container
        foreach(Item item in weaponItemsList)
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
