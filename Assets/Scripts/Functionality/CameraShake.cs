using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance = null;

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

    public void StartShake(int shakeStrengthLevel)
    {
        if(shakeStrengthLevel == 0)
        {
            StartCoroutine(Shake(lightDuration, lightMagnitude));
        }
        else if(shakeStrengthLevel == 1)
        {
            StartCoroutine(Shake(mediumDuration, mediumMagnitude));
        }
        else if(shakeStrengthLevel == 2)
        {
            StartCoroutine(Shake(strongDuration, strongMagnitude));
        }
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsedTime = 0.0f;

        while(elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
