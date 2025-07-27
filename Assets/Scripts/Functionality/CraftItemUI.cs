using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftItemUI : MonoBehaviour
{
    public BlacksmithCraftItemSO item;

    public Image itemIcon = null;
    public TMP_Text itemName = null;
    public TMP_Text mainStat = null;
    public TMP_Text subStat = null;

    public Color greaterValueColor;
    public Color lowerValueColor;

    public void Initialise(BlacksmithCraftItemSO itemData)
    {
        item = itemData;

        // Load the item's icon onto the UI
        if(item.itemIcon != null)
        {
            itemIcon.sprite = item.itemIcon;
        }

        // Load the item's name, enhancement level and rarity on the UI
        itemName.text = item.itemName;

        // Load the item's main stat on the UI
        LoadMainStatText();

        // Load the item's sub stat on the UI
        LoadSubStatText();
    }

    public void OnCraftButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        BlacksmithManager.instance.OpenCraftingDetails(item);
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
