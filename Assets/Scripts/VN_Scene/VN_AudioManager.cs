using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VN_AudioManager : MonoBehaviour
{
    public static VN_AudioManager instance = null;

    [Header("Background Music Source")]
    [Space(5)]
    public AudioSource textSoundSource;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Trying to create more than one AudioManager!");
            Destroy(gameObject);
        }
        instance = this;
    }
}
