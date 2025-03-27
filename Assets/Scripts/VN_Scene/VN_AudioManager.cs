using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VN_AudioManager : MonoBehaviour
{
    public static VN_AudioManager instance = null;

    [Header("Background Music Source")]
    [Space(5)]
    public AudioSource backgroundMusicSource;
    public AudioSource sfxSource;
    public AudioSource textSoundSource;

    // Player volume settings
    private float sfxVolume;
    private float bgmVolume;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Trying to create more than one AudioManager!");
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
        // Set volume of audio sources
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume_VN", 1.0f);
        bgmVolume = PlayerPrefs.GetFloat("bgmVolume_VN", 1.0f);
        RefreshVolumeLevels();
    }

    public void ChangeVolumeLevels(float bgmVolumeLevel, float sfxVolumeLevel)
    {
        bgmVolume = bgmVolumeLevel;
        sfxVolume = sfxVolumeLevel;
        PlayerPrefs.SetFloat("bgmVolume_VN", bgmVolume);
        PlayerPrefs.SetFloat("sfxVolume_VN", sfxVolume);
        PlayerPrefs.Save();

        RefreshVolumeLevels();
    }

    private void RefreshVolumeLevels()
    {
        backgroundMusicSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
        textSoundSource.volume = sfxVolume;
    }
}
