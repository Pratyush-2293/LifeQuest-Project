using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Item")]
public class ItemSO : ScriptableObject
{
    // Input Item Values
    [Header("Item Type Config")]
    [Space(5)]
    public ItemType itemType;
    public WeaponType weaponType;
    public OffhandType offhandType;
    public ArmorType armorType;

    [Header("Item Stacking Config")]
    [Space(5)]
    public bool isStackable = false;
    public int itemQuantity = 1;

    [Header("Item Identity Config")]
    [Space(5)]
    public Sprite itemIcon = null;
    public string itemName = null;

    [Header("Item Stats Config")]
    [Space(5)]
    public ItemRarity itemRarity;
    [Space(10)]
    public int enhancementLevel = 0;
    [Space(10)]
    public MainStatType mainStatType;
    public int mainStatValue = 0;
    [Space(10)]
    public SubStatType subStatType;
    public int subStatValue = 0;

    // Item Type Config
    public enum ItemType { Weapon, Offhand, Material, QuestItem, HeavyArmor, MediumArmor, LightArmor };
    public enum WeaponType { None, Sword, Staff };
    public enum OffhandType { None, Relic, Shield, Tome, Sword };
    public enum ArmorType { None, Headgear, Chestpiece, Legguards, Boots };

    // Item Stats Data
    public enum ItemRarity { Common, Uncommon, Rare, Mystic, Epic, Legendary };
    public enum MainStatType { ATK, DEF };
    public enum SubStatType { None, CR, CD, ATK, DEF, MP };
}
