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
            AldenTopDown aldenController = collision.GetComponent<AldenTopDown>();
            aldenController.FreezePlayer();
            combatStartIndicatorAnimator.SetTrigger("Attack");
            AudioManager.instance.PlaySound("attackIndicator");
            StartCoroutine(TriggerCombatStart(aldenController));
        }
    }

    private IEnumerator TriggerCombatStart(AldenTopDown aldenController)
    {
        yield return new WaitForSeconds(0.8f);
        aldenController.UnfreezePlayer();
        DungeonManager.instance.LoadDungeonCombatScene(loadCombatSceneName);

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
