using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop instance = null;

    [Header("Hitstop Config 0")]
    public float duration0 = 0.0f;
    public float timeScale0 = 0.0f;

    private bool waiting = false;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Trying to create more than one HitStop!");
            Destroy(gameObject);
        }
        instance = this;
    }

    public void StartHitStop(int configIndex)
    {
        if(configIndex == 0)
        {
            if (waiting)
            {
                return;
            }

            Time.timeScale = timeScale0;
            StartCoroutine(Wait(duration0));
        }
    }

    private IEnumerator Wait(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        waiting = false;
    }
}
