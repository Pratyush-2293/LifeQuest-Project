using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneScript : MonoBehaviour
{
    public Transform[] playerPositions = new Transform[4];
    public Transform[] enemyPositions = new Transform[5];

    private int currentWave = 1;
    //private int MAX_WAVES = 5;

    private void Start()
    {
        PlacePlayerCharacters();
        PlaceEnemyCharacters();
    }

    private void PlacePlayerCharacters()
    {
        for(int i = 0; i < CombatData.playerPartyMembers.Count; i++)
        {
            Instantiate(CombatData.playerPartyMembers[i], playerPositions[i].position, Quaternion.identity);
        }
    }

    private void PlaceEnemyCharacters()
    {
        switch (currentWave)
        {
            case 1:
                for(int i = 0; i < CombatData.enemyWave1.Count; i++)
                {
                    Instantiate(CombatData.enemyWave1[i], enemyPositions[i].position, Quaternion.identity);
                }
                break;
            case 2:
                for (int i = 0; i < CombatData.enemyWave2.Count; i++)
                {
                    Instantiate(CombatData.enemyWave2[i], enemyPositions[i].position, Quaternion.identity);
                }
                break;
            case 3:
                for (int i = 0; i < CombatData.enemyWave3.Count; i++)
                {
                    Instantiate(CombatData.enemyWave3[i], enemyPositions[i].position, Quaternion.identity);
                }
                break;
            case 4:
                for (int i = 0; i < CombatData.enemyWave4.Count; i++)
                {
                    Instantiate(CombatData.enemyWave4[i], enemyPositions[i].position, Quaternion.identity);
                }
                break;
            case 5:
                for (int i = 0; i < CombatData.enemyWave5.Count; i++)
                {
                    Instantiate(CombatData.enemyWave5[i], enemyPositions[i].position, Quaternion.identity);
                }
                break;
        }
    }
}
