using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCombatStarter : MonoBehaviour
{
    public string loadCombatSceneName;
    public Animator combatStartIndicatorAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Alden"))
        {
            combatStartIndicatorAnimator.SetTrigger("Attack");
            DungeonManager.instance.PlaySoundEffect("attackIndicator");
            StartCoroutine(TriggerCombatStart());
        }
    }

    private IEnumerator TriggerCombatStart()
    {
        yield return new WaitForSeconds(0.2f);
        DungeonManager.instance.LoadDungeonCombatScene(loadCombatSceneName);
    }
}
