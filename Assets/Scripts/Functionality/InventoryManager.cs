using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        aldenCharAnimator.SetTrigger("Appear");
    }


    // ------------------------ Button Functions -----------------------------
    public void OnMainWeaponButton()
    {

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
}
