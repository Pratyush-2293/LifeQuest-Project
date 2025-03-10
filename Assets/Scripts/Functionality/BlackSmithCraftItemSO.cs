using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Craft Item", menuName = "Blacksmith/Craft Item")]
public class BlacksmithCraftItemSO : ScriptableObject
{
    [Header("Item Core Data")]
    [Space(5)]
    public int unlockLevel = 1;
    public string itemName = "";
    public Sprite itemIcon;
    
    [Header("Item Stats Config")]
    [Space(5)]
    public Item.MainStatType mainStatType = Item.MainStatType.ATK;
    public int mainStatValue = 0;
    [Space(5)]
    public Item.SubStatType subStatType = Item.SubStatType.ATK;
    public int subStatValue = 0;

    [Header("Crafting Materials Config")]
    [Space(5)]
    public int totalMaterialTypes = 1;
    [Space(5)]
    public ItemSO craftingMaterial1;
    public int materialQuantity1 = 1;
    [Space(5)]
    public ItemSO craftingMaterial2;
    public int materialQuantity2 = 1;
    [Space(5)]
    public ItemSO craftingMaterial3;
    public int materialQuantity3 = 1;
    [Space(5)]
    public ItemSO craftingMaterial4;
    public int materialQuantity4 = 1;
    [Space(10)]
    public int craftingCost = 1;

    [Header("Item Rewarded By Crafting")]
    public ItemSO craftingReward;
}
