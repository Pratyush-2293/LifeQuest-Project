using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatData
{
    public static List<GameObject> playerPartyMembers = new List<GameObject>();
    public static List<GameObject> enemyWave1 = new List<GameObject>();
    public static List<GameObject> enemyWave2 = new List<GameObject>();
    public static List<GameObject> enemyWave3 = new List<GameObject>();
    public static List<GameObject> enemyWave4 = new List<GameObject>();
    public static List<GameObject> enemyWave5 = new List<GameObject>();

    public static void ClearCombatData()
    {
        playerPartyMembers.Clear();
        enemyWave1.Clear();
        enemyWave2.Clear();
        enemyWave3.Clear();
        enemyWave4.Clear();
        enemyWave5.Clear();
    }

    public static void LoadPlayerParty(List<GameObject> playerParty)
    {
        playerPartyMembers = playerParty;
    }

    public static void LoadWaves(List<GameObject> wave1, List<GameObject> wave2, List<GameObject> wave3, List<GameObject> wave4, List<GameObject> wave5)
    {
        enemyWave1 = wave1;
        enemyWave2 = wave2;
        enemyWave3 = wave3;
        enemyWave4 = wave4;
        enemyWave5 = wave5;
    }
}