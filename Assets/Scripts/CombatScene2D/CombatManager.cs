using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CombatManager : MonoBehaviour
{
    [Header("Player Characters")]
    public GameObject playerCharacter1 = null;
    public GameObject playerCharacter2 = null;
    public GameObject playerCharacter3 = null;
    public GameObject playerCharacter4 = null;

    [Header("Enemy Characters")]
    public GameObject enemyCharacter1 = null;
    public GameObject enemyCharacter2 = null;
    public GameObject enemyCharacter3 = null;
    public GameObject enemyCharacter4 = null;

    // Storing script files for all characters on field
    private PlayerCombat2D playerCombatController1;
    private PlayerCombat2D playerCombatController2;
    private PlayerCombat2D playerCombatController3;
    private PlayerCombat2D playerCombatController4;

    private EnemyCombat2D enemyCombatController1;
    private EnemyCombat2D enemyCombatController2;
    private EnemyCombat2D enemyCombatController3;
    private EnemyCombat2D enemyCombatController4;

    private void Start()
    {
        playerCombatController1 = playerCharacter1.gameObject.GetComponent<PlayerCombat2D>();
        playerCombatController2 = playerCharacter2.gameObject.GetComponent<PlayerCombat2D>();
        playerCombatController3 = playerCharacter3.gameObject.GetComponent<PlayerCombat2D>();
        playerCombatController4 = playerCharacter4.gameObject.GetComponent<PlayerCombat2D>();

        enemyCombatController1 = enemyCharacter1.gameObject.GetComponent<EnemyCombat2D>();
        enemyCombatController2 = enemyCharacter2.gameObject.GetComponent<EnemyCombat2D>();
        enemyCombatController3 = enemyCharacter3.gameObject.GetComponent<EnemyCombat2D>();
        enemyCombatController4 = enemyCharacter4.gameObject.GetComponent<EnemyCombat2D>();
    }
}
