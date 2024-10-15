using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatStartTrigger : MonoBehaviour
{
    public List<GameObject> playerPartyMembers = new List<GameObject>();
    public List<GameObject> enemyWave1 = new List<GameObject>();
    public List<GameObject> enemyWave2 = new List<GameObject>();
    public List<GameObject> enemyWave3 = new List<GameObject>();
    public List<GameObject> enemyWave4 = new List<GameObject>();
    public List<GameObject> enemyWave5 = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        CombatData.ClearCombatData();
        CombatData.LoadPlayerParty(playerPartyMembers);
        CombatData.LoadWaves(enemyWave1, enemyWave2, enemyWave3, enemyWave4, enemyWave5);

        SceneManager.LoadScene("CombatScene");
    }
}
