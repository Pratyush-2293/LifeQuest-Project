using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance = null;

    public Transform uiShakeTarget; // Assign this in UI-only scenes

    [Header("Light Shake Settings")]
    public float lightDuration = 0.0f;
    public float lightMagnitude = 0.0f;

    [Header("Medium Shake Settings")]
    public float mediumDuration = 0.0f;
    public float mediumMagnitude = 0.0f;

    [Header("Strong Shake Settings")]
    public float strongDuration = 0.0f;
    public float strongMagnitude = 0.0f;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Trying to create more than one instance of CameraShake!");
            Destroy(gameObject);
        }
        instance = this;
    }

    public void StartShake(int shakeStrengthLevel, bool isUIShake = false)
    {
        float duration = 0f, magnitude = 0f;

        if (shakeStrengthLevel == 0) { duration = lightDuration; magnitude = lightMagnitude; }
        else if (shakeStrengthLevel == 1) { duration = mediumDuration; magnitude = mediumMagnitude; }
        else if (shakeStrengthLevel == 2) { duration = strongDuration; magnitude = strongMagnitude; }

        if (isUIShake && uiShakeTarget != null)
            StartCoroutine(Shake(uiShakeTarget, duration, magnitude));
        else
            StartCoroutine(Shake(transform, duration, magnitude));
    }

    private IEnumerator Shake(Transform target, float duration, float magnitude)
    {
        Vector3 originalPos = target.localPosition;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            target.localPosition = new Vector3(x, y, originalPos.z);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        target.localPosition = originalPos;
    }
}
