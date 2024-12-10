using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatSceneScript : MonoBehaviour
{
    public static CombatSceneScript instance = null;

    public Transform[] playerPositions = new Transform[4];
    public Transform[] enemyPositions = new Transform[4];

    public GameObject[] playerChars = new GameObject[4];
    public GameObject[] enemiesOnField = new GameObject[4];

    private int currentWave = 1;
    //private int MAX_WAVES = 5;

    public int currentSelectedTarget = 1;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one CombatSceneScript instance!");
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
        PlacePlayerCharacters();
        PlaceEnemyCharacters();
    }

    private void PlacePlayerCharacters()
    {
        for(int i = 0; i < CombatData.playerPartyMembers.Count; i++)
        {
            playerChars[i] = Instantiate(CombatData.playerPartyMembers[i], playerPositions[i].position, Quaternion.identity);
            playerChars[i].gameObject.GetComponent<PlayerChar>().position = i + 1;
        }
    }

    private void PlaceEnemyCharacters()
    {
        switch (currentWave)
        {
            case 1:
                ClearEnemiesOnField();
                for(int i = 0; i < CombatData.enemyWave1.Count; i++)
                {
                    enemiesOnField[i] = Instantiate(CombatData.enemyWave1[i], enemyPositions[i].position, Quaternion.identity);
                    enemiesOnField[i].gameObject.GetComponent<Enemy>().position = i + 1;
                }
                break;
            case 2:
                ClearEnemiesOnField();
                for (int i = 0; i < CombatData.enemyWave2.Count; i++)
                {
                    enemiesOnField[i] = Instantiate(CombatData.enemyWave1[i], enemyPositions[i].position, Quaternion.identity);
                    enemiesOnField[i].gameObject.GetComponent<Enemy>().position = i + 1;
                }
                break;
            case 3:
                ClearEnemiesOnField();
                for (int i = 0; i < CombatData.enemyWave3.Count; i++)
                {
                    enemiesOnField[i] = Instantiate(CombatData.enemyWave1[i], enemyPositions[i].position, Quaternion.identity);
                    enemiesOnField[i].gameObject.GetComponent<Enemy>().position = i + 1;
                }
                break;
            case 4:
                ClearEnemiesOnField();
                for (int i = 0; i < CombatData.enemyWave4.Count; i++)
                {
                    enemiesOnField[i] = Instantiate(CombatData.enemyWave1[i], enemyPositions[i].position, Quaternion.identity);
                    enemiesOnField[i].gameObject.GetComponent<Enemy>().position = i + 1;
                }
                break;
            case 5:
                ClearEnemiesOnField();
                for (int i = 0; i < CombatData.enemyWave5.Count; i++)
                {
                    enemiesOnField[i] = Instantiate(CombatData.enemyWave1[i], enemyPositions[i].position, Quaternion.identity);
                    enemiesOnField[i].gameObject.GetComponent<Enemy>().position = i + 1;
                }
                break;
        }
    }

    private void ClearEnemiesOnField()
    {
        for(int i = 0; i < 4; i++)
        {
            enemiesOnField[i] = null;
        }
    }

    public void SelectInitialTarget()
    {
        enemiesOnField[0].gameObject.GetComponent<Enemy>().selectMarker.SetActive(true);
    }

    public void UpdateSelectedTarget(int direction)
    {
        if(direction == 1)
        {
            if(currentSelectedTarget == 1)
            {
                if(enemiesOnField[3] != null)
                {
                    enemiesOnField[3].gameObject.GetComponent<Enemy>().selectMarker.SetActive(true);
                    enemiesOnField[0].gameObject.GetComponent<Enemy>().selectMarker.SetActive(false);
                    currentSelectedTarget = 4;
                }
                else if(enemiesOnField[2] != null)
                {
                    enemiesOnField[2].gameObject.GetComponent<Enemy>().selectMarker.SetActive(true);
                    enemiesOnField[0].gameObject.GetComponent<Enemy>().selectMarker.SetActive(false);
                    currentSelectedTarget = 3;
                }
            }
            else if(currentSelectedTarget == 2)
            {
                if (enemiesOnField[0] != null)
                {
                    enemiesOnField[0].gameObject.GetComponent<Enemy>().selectMarker.SetActive(true);
                    enemiesOnField[1].gameObject.GetComponent<Enemy>().selectMarker.SetActive(false);
                    currentSelectedTarget = 1;
                }
                else if (enemiesOnField[3] != null)
                {
                    enemiesOnField[3].gameObject.GetComponent<Enemy>().selectMarker.SetActive(true);
                    enemiesOnField[1].gameObject.GetComponent<Enemy>().selectMarker.SetActive(false);
                    currentSelectedTarget = 4;
                }
                else if (enemiesOnField[2] != null)
                {
                    enemiesOnField[2].gameObject.GetComponent<Enemy>().selectMarker.SetActive(true);
                    enemiesOnField[1].gameObject.GetComponent<Enemy>().selectMarker.SetActive(false);
                    currentSelectedTarget = 3;
                }
            }
            else if(currentSelectedTarget == 4) // not needed for 3 since it is at maximum 'up' position
            {
                if(enemiesOnField[2] != null)
                {
                    enemiesOnField[2].gameObject.GetComponent<Enemy>().selectMarker.SetActive(true);
                    enemiesOnField[3].gameObject.GetComponent<Enemy>().selectMarker.SetActive(false);
                    currentSelectedTarget = 3;
                }
            }
        }
        else if(direction == 2)
        {
            if (currentSelectedTarget == 1)
            {
                if (enemiesOnField[1] != null)
                {
                    enemiesOnField[1].gameObject.GetComponent<Enemy>().selectMarker.SetActive(true);
                    enemiesOnField[0].gameObject.GetComponent<Enemy>().selectMarker.SetActive(false);
                    currentSelectedTarget = 2;
                }
            }
            else if (currentSelectedTarget == 3) // not needed for 2 since it is at maximum 'down' position
            {
                if (enemiesOnField[3] != null)
                {
                    enemiesOnField[3].gameObject.GetComponent<Enemy>().selectMarker.SetActive(true);
                    enemiesOnField[2].gameObject.GetComponent<Enemy>().selectMarker.SetActive(false);
                    currentSelectedTarget = 4;
                }
                else if (enemiesOnField[0] != null)
                {
                    enemiesOnField[0].gameObject.GetComponent<Enemy>().selectMarker.SetActive(true);
                    enemiesOnField[2].gameObject.GetComponent<Enemy>().selectMarker.SetActive(false);
                    currentSelectedTarget = 1;
                }
                else if (enemiesOnField[1] != null)
                {
                    enemiesOnField[1].gameObject.GetComponent<Enemy>().selectMarker.SetActive(true);
                    enemiesOnField[2].gameObject.GetComponent<Enemy>().selectMarker.SetActive(false);
                    currentSelectedTarget = 2;
                }
            }
            else if (currentSelectedTarget == 4)
            {
                if (enemiesOnField[0] != null)
                {
                    enemiesOnField[0].gameObject.GetComponent<Enemy>().selectMarker.SetActive(true);
                    enemiesOnField[3].gameObject.GetComponent<Enemy>().selectMarker.SetActive(false);
                    currentSelectedTarget = 1;
                }
                else if(enemiesOnField[1] != null)
                {
                    enemiesOnField[1].gameObject.GetComponent<Enemy>().selectMarker.SetActive(true);
                    enemiesOnField[3].gameObject.GetComponent<Enemy>().selectMarker.SetActive(false);
                    currentSelectedTarget = 2;
                }
            }
        }
    }
}
