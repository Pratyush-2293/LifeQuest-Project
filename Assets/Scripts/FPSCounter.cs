using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private int frameCount = 0;
    private float deltaTime = 0.0f;
    private float logInterval = 1.0f; // Log every 1 second

    void Update()
    {
        frameCount++;
        deltaTime += Time.unscaledDeltaTime;

        if (deltaTime >= logInterval)
        {
            float fps = frameCount / deltaTime;
            Debug.Log("FPS: " + Mathf.RoundToInt(fps));

            // Reset counters
            frameCount = 0;
            deltaTime = 0.0f;
        }
    }
}
