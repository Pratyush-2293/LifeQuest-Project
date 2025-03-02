using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance = null;

    [Header("Core Game Data")]
    public int expPoints = 0;
    public int goldCoins = 0;
    public int levelKeys = 1;
    public int dungeonKeys = 0;
    public int maxCompletedLevel = 1;
    public int maxUnlockedLevel = 1;
    public int maxAvailableLevel = 2;

    [Header("Alden's Data")]
    public int aldenLevel = 1;
    public int aldenEXP = 0;
    public int aldenEXPForNextLevel = 100;
    public int aldenSTR = 10;
    public int aldenDEF = 5;
    public int aldenVIT = 5;
    public int aldenHP = 100;
    public int aldenMP = 100;
    public int aldenCRIT = 10;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Trying to create more than one GameData!");
            Destroy(gameObject);
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SaveManager.instance.LoadGameData();
    }
}
