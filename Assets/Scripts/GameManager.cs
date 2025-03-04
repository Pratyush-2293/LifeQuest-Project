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
        foreach(ItemSO itemSO in inventoryTestItems)
        {
            GameData.instance.inventory.Add(new Item(itemSO));
        }

        GameData.instance.aldenEquippedWeapon = new Item(aldenWeaponTest);
    }
}
