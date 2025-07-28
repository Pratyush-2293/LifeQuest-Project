using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Dungeon Store Data")]
    public GameObject dungeonRootObject;
    public bool dungeonLoadedFromLevel = false;
    public string activeCombatSceneName;

    [Header("Test Items")]
    public ItemSO aldenWeaponTest;
    public ItemSO aldenOffhandTest;
    public ItemSO aldenHeadgearTest;
    public ItemSO aldenChestpieceTest;
    public ItemSO aldenLegguardsTest;
    public ItemSO aldenBootsTest;
    [Space(5)]
    public ItemSO addTestItem;
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

        // Disabling Vsync
        QualitySettings.vSyncCount = 0;

        // Capping framerate to 90 
        Application.targetFrameRate = 90;
    }


    public void ResumeDungeon()
    {
        StartCoroutine(HandleResumeDungeon());
    }

    public IEnumerator HandleResumeDungeon()
    {
        // Begin unloading the scene
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(activeCombatSceneName);

        // Waiting until the scene is fully unloaded
        while (!unloadOperation.isDone)
        {
            yield return null;
        }

        // After unloading, activate the dungeon scene root
        if (dungeonRootObject != null)
        {
            AudioManager.instance.PlayMusic("devilsCastle");  // !!! THIS IS A TEMPORARY FIX, PATCH THIS LATER WITH PROPER SOLUTION !!! 
            dungeonRootObject.SetActive(true);
        }
    }

    [ContextMenu("Add Test Items")]
    public void AldenItemsInitialise()
    {
        GameData.instance.inventory.Clear();

        GameData.instance.aldenEquippedWeapon = new Item(aldenWeaponTest);
        GameData.instance.aldenEquippedOffHand = new Item(aldenOffhandTest);
        GameData.instance.aldenEquippedHeadgear = new Item(aldenHeadgearTest);
        GameData.instance.aldenEquippedChestpiece = new Item(aldenChestpieceTest);
        GameData.instance.aldenEquippedLegguards = new Item(aldenLegguardsTest);

        GameData.instance.firstTimeEquipmentLoaded = true;
        SaveManager.instance.SaveGameData();
    }

    [ContextMenu("Add Test Item")]
    public void AddTestItem()
    {
        GameData.instance.inventory.Add(new Item(addTestItem));
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

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) // App goes to background
        {
            SaveManager.instance.SaveGameData();
        }
    }

    void OnApplicationQuit()
    {
        if(SaveManager.instance != null)
        {
            SaveManager.instance.SaveGameData();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) // App loses focus (e.g., notification popup, switching apps)
        {
            SaveManager.instance.SaveGameData();
        }
    }
}
