using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnhanceItemUI : MonoBehaviour
{
    [HideInInspector] public Item item;

    public Image itemIcon = null;
    public Image equippedCharIcon = null;
    public TMP_Text itemName = null;
    public TMP_Text mainStat = null;
    public TMP_Text subStat = null;

    public Sprite aldenEquippedSprite;
    public Sprite valricEquippedSprite;
    public Sprite osmirEquippedSprite;
    public Sprite assassinEquippedSprite;

    public Color greaterValueColor;
    public Color lowerValueColor;

    public Button enhanceButton;
    public TMP_Text enhanceButtonText;

    public enum EquippedCharName { None, Alden, Valric, Osmir, Assassin};
    public EquippedCharName currentEquippedCharName = EquippedCharName.None;

    private bool isCurrentlyEquipped = false;

    public void Initialise(Item itemData, bool isEquipped, EquippedCharName equippedCharName)
    {
        item = itemData;
        isCurrentlyEquipped = isEquipped;
        currentEquippedCharName = equippedCharName;

        // Load the item's icon onto the UI
        if (item.itemIcon != null)
        {
            itemIcon.sprite = item.itemIcon;
        }

        // Load the character's equip icon if it is an equipped item, else disable the icon
        if(isEquipped == true)
        {
            if(equippedCharName == EquippedCharName.Alden)
            {
                if(aldenEquippedSprite != null)
                {
                    equippedCharIcon.sprite = aldenEquippedSprite;
                }
            }
            else if(equippedCharName == EquippedCharName.Valric)
            {
                if(valricEquippedSprite != null)
                {
                    equippedCharIcon.sprite = valricEquippedSprite;
                }
            }
            else if (equippedCharName == EquippedCharName.Osmir)
            {
                if (osmirEquippedSprite != null)
                {
                    equippedCharIcon.sprite = osmirEquippedSprite;
                }
            }
            else
            {
                if (assassinEquippedSprite != null)
                {
                    equippedCharIcon.sprite = assassinEquippedSprite;
                }
            }
        }
        else
        {
            equippedCharIcon.gameObject.SetActive(false);
        }

        // Load the item's name, enhancement level and rarity on the UI
        itemName.text = item.itemName;

        // Add the item's enhancement level to the name
        if (item.enhancementLevel > 0)
        {
            itemName.text += " +" + item.enhancementLevel.ToString();
        }

        // Add the item's rarity label to the name
        AddItemRarityLabel();

        // Load the item's main stat on the UI
        LoadMainStatText();

        // Load the item's sub stat on the UI
        LoadSubStatText();

        // Disable the enhance button and change it's text to 'max' if item is already at +10
        UpdateEnhanceButtonActive();
    }

    public void UpdateEnhanceButtonActive()
    {
        if (item.enhancementLevel == 10)
        {
            enhanceButton.interactable = false;
            enhanceButtonText.text = "MAX";
        }
    }

    public void UpdateUI()
    {
        itemName.text = item.itemName + " +" + item.enhancementLevel.ToString();
        AddItemRarityLabel();
        LoadMainStatText();
        LoadSubStatText();
    }

    public void OnEnhanceButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        AlchemistManager.instance.OpenEnhancingDetails(item, isCurrentlyEquipped, currentEquippedCharName, this);
    }

    private void AddItemRarityLabel()
    {
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
        else
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
        else if (item.subStatType == Item.SubStatType.CR)
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
        if (item.subStatType == Item.SubStatType.CR || item.subStatType == Item.SubStatType.CD)
        {
            subStat.text += "%";
        }
    }
}
