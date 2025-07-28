using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    // Item Data
    public ItemType itemType;
    public WeaponType weaponType;
    public OffhandType offhandType;
    public ArmorType armorType;

    public bool isStackable = false;
    public int itemQuantity = 1;

    public Sprite itemIcon = null;
    public string itemIconName = null;
    public string itemName = null;

    public ItemRarity itemRarity;

    public int enhancementLevel = 0;

    public MainStatType mainStatType;
    public int mainStatValue = 0;

    public SubStatType subStatType;
    public int subStatValue = 0;

    public int unlockLevel = 1;
    public int sellValue = 0;

    public RequiredUpgradeMaterials[] upgradeMaterialsList = new RequiredUpgradeMaterials[10];

    // ------------------------- Enum Definitions -------------------------------
    // Item Type Config
    public enum ItemType { None, Weapon, Offhand, Material, QuestItem, HeavyArmor, MediumArmor, LightArmor};
    public enum WeaponType { None, Sword, Staff};
    public enum OffhandType { None, Relic, Shield, Tome, Sword};
    public enum ArmorType { None, Headgear, Chestpiece, Legguards, Boots};

    // Item Stats Data
    public enum ItemRarity { Common, Uncommon, Rare, Mystic, Epic, Legendary };
    public enum MainStatType { ATK, DEF};
    public enum SubStatType { None, CR, CD, ATK, DEF, MP};
    // ------------------------- Enum Definitions -------------------------------


    public Item(ItemSO itemData)
    {
        itemType = (ItemType)itemData.itemType;
        weaponType = (WeaponType)itemData.weaponType;
        offhandType = (OffhandType)itemData.offhandType;
        armorType = (ArmorType)itemData.armorType;

        isStackable = itemData.isStackable;
        itemQuantity = itemData.itemQuantity;

        itemIcon = itemData.itemIcon;
        itemIconName = itemData.itemIconName;
        itemName = itemData.itemName;

        itemRarity = (ItemRarity)itemData.itemRarity;
        enhancementLevel = itemData.enhancementLevel;
        mainStatType = (MainStatType)itemData.mainStatType;
        mainStatValue = itemData.mainStatValue;
        subStatType = (SubStatType)itemData.subStatType;
        subStatValue = itemData.subStatValue;

        unlockLevel = itemData.unlockLevel;
        sellValue = itemData.sellValue;

        upgradeMaterialsList = itemData.upgradeMaterialsList;
    }
}
