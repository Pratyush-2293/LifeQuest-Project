using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialiser : MonoBehaviour
{
    public enum GameMode { INVALID, Menus, Gameplay};

    public GameMode gameMode;

    public GameObject gameManagerPrefab = null;
    void Start()
    {
        if(GameManager.instance == null)
        {
            if (gameManagerPrefab)
            {
                Instantiate(gameManagerPrefab);
            }
            else
            {
                Debug.LogError("GameManager prefab is not set!");
            }
        }
    }
}
