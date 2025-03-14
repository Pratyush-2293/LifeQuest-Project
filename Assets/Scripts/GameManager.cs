using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Test Items")]
    public ItemSO aldenWeaponTest;
    public ItemSO aldenOffhandTest;
    public ItemSO aldenHeadgearTest;
    public ItemSO aldenChestpieceTest;
    public ItemSO aldenLegguardsTest;
    public ItemSO aldenBootsTest;
    [Space(5)]
    public List<ItemSO> inventoryTestItems = new List<ItemSO>();

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one instance of GameManager!");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameManager Created.");
    }

    [ContextMenu("Add Test Items")]
    public void AddTestItems()
    {
        GameData.instance.inventory.Clear();

        foreach(ItemSO itemSO in inventoryTestItems)
        {
            GameData.instance.inventory.Add(new Item(itemSO));
        }

        GameData.instance.aldenEquippedWeapon = new Item(aldenWeaponTest);
        GameData.instance.aldenEquippedOffHand = new Item(aldenOffhandTest);
        GameData.instance.aldenEquippedHeadgear = new Item(aldenHeadgearTest);
        GameData.instance.aldenEquippedChestpiece = new Item(aldenChestpieceTest);
        GameData.instance.aldenEquippedLegguards = new Item(aldenLegguardsTest);
    }

    [ContextMenu("Reset Alden Data")]
    public void ResetAldenData()
    {
        // Only call this when alden has nothing equipped, otherwise the stats will break upon unequipping and will have to be manually set again.
        GameData.instance.aldenLevel = 1;
        GameData.instance.aldenEXP = 0;
        GameData.instance.aldenEXPForNextLevel = 75;
        GameData.instance.aldenATK = 8;
        GameData.instance.aldenDEF = 6;
        GameData.instance.aldenHP = 10;
    }

    [ContextMenu("Give Me EXP")]
    public void GiveMeEXP()
    {
        GameData.instance.expPoints += 4000;
    }

    [ContextMenu("Remove Empty Items")]
    public void RemoveEmptyObjectsInInventory()
    {
        GameData.instance.inventory.RemoveAll(item => item.itemType == Item.ItemType.None);
    }

    // Save Handling for mobile devices:
    /*
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) // App goes to background
        {
            SaveManager.instance.SaveGameData();
        }
    }

    void OnApplicationQuit()
    {
        SaveManager.instance.SaveGameData();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) // App loses focus (e.g., notification popup, switching apps)
        {
            SaveManager.instance.SaveGameData();
        }
    }
    */
}
