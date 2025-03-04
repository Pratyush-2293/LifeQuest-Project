using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData : MonoBehaviour
{
    public static GameData instance = null;

    [Header("Currencies")]
    public int expPoints = 0;
    public int goldCoins = 0;
    public int levelKeys = 1;
    public int dungeonKeys = 0;

    [Header("Level Completion Data")]
    public int maxCompletedLevel = 1;
    public int maxUnlockedLevel = 1;
    public int maxAvailableLevel = 2;

    [Header("Inventory")]
    public List<Item> inventory = new List<Item>();

    [Header("Alden's Data")]
    public int aldenLevel = 1;
    public int aldenEXP = 0;
    public int aldenEXPForNextLevel = 100;
    public int aldenSTR = 10;
    public int aldenDEF = 5;
    public int aldenHP = 100;
    public int aldenMP = 100;
    public int aldenCR = 10;
    public int aldenCD = 100;
    // Update this in the loader function
    public Item aldenEquippedWeapon;
    public Item aldenEquippedOffHand;
    public Item aldenEquippedHeadgear;
    public Item aldenEquippedChestpiece;
    public Item aldenEquippedLegguards;
    public Item aldenEquippedBoots;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Trying to create more than one GameData!");
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SaveManager.instance.LoadGameData();
    }

    [ContextMenu("Save Test Data")]
    private void SaveDataForTesting()
    {
        SaveManager.instance.SaveGameData();
    }

    // Converts GameData to GameDataSave for serialization
    public GameDataSave ToSaveData()
    {
        return new GameDataSave
        {
            expPoints = expPoints,
            goldCoins = goldCoins,
            levelKeys = levelKeys,
            dungeonKeys = dungeonKeys,
            maxCompletedLevel = maxCompletedLevel,
            maxUnlockedLevel = maxUnlockedLevel,
            maxAvailableLevel = maxAvailableLevel,
            inventory = inventory,

            aldenLevel = aldenLevel,
            aldenEXP = aldenEXP,
            aldenEXPForNextLevel = aldenEXPForNextLevel,
            aldenSTR = aldenSTR,
            aldenDEF = aldenDEF,
            aldenHP = aldenHP,
            aldenMP = aldenMP,
            aldenCR = aldenCR,
            aldenCD = aldenCD
        };
    }

    // Loads data from GameDataSave into GameData
    public void LoadFromSaveData(GameDataSave data)
    {
        expPoints = data.expPoints;
        goldCoins = data.goldCoins;
        levelKeys = data.levelKeys;
        dungeonKeys = data.dungeonKeys;
        maxCompletedLevel = data.maxCompletedLevel;
        maxUnlockedLevel = data.maxUnlockedLevel;
        maxAvailableLevel = data.maxAvailableLevel;
        inventory = data.inventory;

        aldenLevel = data.aldenLevel;
        aldenEXP = data.aldenEXP;
        aldenEXPForNextLevel = data.aldenEXPForNextLevel;
        aldenSTR = data.aldenSTR;
        aldenDEF = data.aldenDEF;
        aldenHP = data.aldenHP;
        aldenMP = data.aldenMP;
        aldenCR = data.aldenCR;
        aldenCD = data.aldenCD;
    }
}

[System.Serializable]
public class GameDataSave
{
    // Core Game Data
    public int expPoints;
    public int goldCoins;
    public int levelKeys;
    public int dungeonKeys;
    public int maxCompletedLevel;
    public int maxUnlockedLevel;
    public int maxAvailableLevel;
    public List<Item> inventory;

    // Alden's Data
    public int aldenLevel;
    public int aldenEXP;
    public int aldenEXPForNextLevel;
    public int aldenSTR;
    public int aldenDEF;
    public int aldenHP;
    public int aldenMP;
    public int aldenCR;
    public int aldenCD;
}

