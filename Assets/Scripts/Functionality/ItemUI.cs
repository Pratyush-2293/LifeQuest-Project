using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    public Item item;

    public Image itemIcon = null;
    public TMP_Text itemName = null;
    public TMP_Text mainStat = null;
    public TMP_Text subStat = null;

    public Color greaterValueColor;
    public Color lowerValueColor;

    public void Initialise(Item itemData)
    {
        item = itemData;

        // Load the item's icon onto the UI
        if(item.itemIcon != null)
        {
            itemIcon.sprite = item.itemIcon;
        }

        // Load the item's name, enhancement level and rarity on the UI
        LoadItemNameText();

        // Load the item's main stat on the UI
        LoadMainStatText();

        // Load the item's sub stat on the UI
        LoadSubStatText();

        // Add the value color to the text
        ColorText();
    }

    public void OnUnequipButton()
    {
        InventoryManager.instance.UnequipItem(item);
    }

    public void OnEquipButton()
    {
        InventoryManager.instance.EquipItem(item);
    }

    private void ColorText()
    {
        // Check if the mainstat & substat value of the given item is lower or greater than the equipped item.
        // If lower, then color red, if greater then color green.

        if(HeroesMenuManager.instance.activeCharacter == HeroesMenuManager.ActiveCharacter.Alden)
        {
            if (item.itemType == Item.ItemType.Weapon)
            {
                if(item.mainStatValue > GameData.instance.aldenEquippedWeapon.mainStatValue)
                {
                    mainStat.color = greaterValueColor;
                }
                else if(item.mainStatValue == GameData.instance.aldenEquippedWeapon.mainStatValue)
                {
                    mainStat.color = Color.white;
                }
                else
                {
                    mainStat.color = lowerValueColor;
                }

                if (item.subStatValue > GameData.instance.aldenEquippedWeapon.subStatValue)
                {
                    subStat.color = greaterValueColor;
                }
                else if (item.subStatValue == GameData.instance.aldenEquippedWeapon.subStatValue)
                {
                    subStat.color = Color.white;
                }
                else
                {
                    subStat.color = lowerValueColor;
                }
            }
        }
    }

    private void LoadItemNameText()
    {
        // Load the item's name & enhancement level on the UI
        if (item.enhancementLevel == 0)
        {
            itemName.text = item.itemName;
        }
        else
        {
            itemName.text = item.itemName + " +" + item.enhancementLevel.ToString();
        }

        // Add the item's rarity label on the item name
        if (item.itemRarity == Item.ItemRarity.Common)
        {
            itemName.text += " (Common)";
        }
        else if (item.itemRarity == Item.ItemRarity.Uncommon)
        {
            itemName.text += " (Uncommon)";
        }
        else if (item.itemRarity == Item.ItemRarity.Rare)
        {
            itemName.text += " (Rare)";
        }
        else if (item.itemRarity == Item.ItemRarity.Mystic)
        {
            itemName.text += " (Mystic)";
        }
        else if (item.itemRarity == Item.ItemRarity.Epic)
        {
            itemName.text += " (Epic)";
        }
        else if (item.itemRarity == Item.ItemRarity.Legendary)
        {
            itemName.text += " (Legendary)";
        }
    }

    private void LoadMainStatText()
    {
        
        if (item.mainStatType == Item.MainStatType.ATK)
        {
            mainStat.text = "ATK ";
        }
        else
        {
            mainStat.text = "DEF ";
        }

        // Add the main stat value to the main stat text
        mainStat.text += "+" + item.mainStatValue.ToString();
    }

    private void LoadSubStatText()
    {

        if (item.subStatType == Item.SubStatType.None)
        {
            subStat.text = "-";
        }
        else if(item.subStatType == Item.SubStatType.CR)
        {
            subStat.text = "CR ";
        }
        else if (item.subStatType == Item.SubStatType.CD)
        {
            subStat.text = "CD ";
        }
        else if (item.subStatType == Item.SubStatType.ATK)
        {
            subStat.text = "ATK ";
        }
        else if (item.subStatType == Item.SubStatType.DEF)
        {
            subStat.text = "DEF ";
        }
        else if (item.subStatType == Item.SubStatType.MP)
        {
            subStat.text = "MP ";
        }

        // Add the sub stat value to the sub stat text
        subStat.text += "+" + item.subStatValue.ToString();

        // Add a % symbol if the substat is CR or CD
        if(item.subStatType == Item.SubStatType.CR || item.subStatType == Item.SubStatType.CD)
        {
            subStat.text += "%";
        }
    }
}
