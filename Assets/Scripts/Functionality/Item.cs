using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public ScriptableObject itemData = null;
    public enum ItemType { Weapon, Offhand, Material, QuestItem, HeavyArmor, MediumArmor, LightArmor};
    public enum WeaponType { None, Sword, Staff};
    public enum OffhandType { None, Relic, Shield, Tome, Sword};
    public enum ArmorType { Headgear, Chestpiece, Legguards, Boots}

    public Sprite itemIcon = null;
    public string itemName = null;

}
