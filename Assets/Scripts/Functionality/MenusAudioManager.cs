using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenusAudioManager : MonoBehaviour
{
    public static MenusAudioManager instance = null;
    [Header("Musics")]
    [Space(5)]
    public AudioClip initalBackgroundMusic;
    public AudioClip[] backgroundMusics;

    [Header("Sound Effects")]
    [Space(5)]
    [SerializeField] private AudioClip[] soundEffects;

    [Header("Audio Sources")]
    [Space(5)]
    [SerializeField] private AudioSource backgroundMusicSource = null;
    [SerializeField] private AudioSource sfxSource1 = null;
    [SerializeField] private AudioSource sfxSource2 = null;
    [SerializeField] private AudioSource sfxSource3 = null;
    private int currentAudioSource = 1;

    // Player volume settings
    private float sfxVolume;
    private float bgmVolume;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Trying to create more than one MenusAudioManager!");
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

        // Start playing background music
        if (initalBackgroundMusic != null)
        {
            backgroundMusicSource.clip = initalBackgroundMusic;
            backgroundMusicSource.Play();
        }
    }

    public void ChangeVolumeLevels(float bgmVolumeLevel, float sfxVolumeLevel)
    {
        bgmVolume = bgmVolumeLevel;
        sfxVolume = sfxVolumeLevel;

        RefreshVolumeLevels();
    }

    public void ChangeBackgroundMusic(string audioName)
    {
        AudioClip music = Array.Find(backgroundMusics, x => x.name == audioName);

        if (music == null)
        {
            Debug.LogWarning("Sound not found !");
        }
        else
        {
            StartCoroutine(BGMFade(music));
        }
    }

    public void PlaySound(string audioName)
    {
        AudioClip sound = Array.Find(soundEffects, x => x.name == audioName);

        if (sound == null)
        {
            Debug.LogWarning("Sound not found !");
        }
        else
        {
            // Alternating between sound channels to prevent interrupting active SFX
            if (currentAudioSource == 1)
            {
                sfxSource1.PlayOneShot(sound);
                currentAudioSource++;
            }
            else if (currentAudioSource == 2)
            {
                sfxSource2.PlayOneShot(sound);
                currentAudioSource++;
            }
            else
            {
                sfxSource3.PlayOneShot(sound);
                currentAudioSource = 1;
            }
        }
    }

    private void RefreshVolumeLevels()
    {
        backgroundMusicSource.volume = bgmVolume;
        sfxSource1.volume = sfxVolume;
        sfxSource2.volume = sfxVolume;
        sfxSource3.volume = sfxVolume;
    }

    private IEnumerator BGMFade(AudioClip musicToFade)
    {
        while (backgroundMusicSource.volume > 0)
        {
            backgroundMusicSource.volume -= 0.05f;
            yield return new WaitForSeconds(0.1f);
        }

        backgroundMusicSource.clip = musicToFade;
        backgroundMusicSource.Play();

        while (backgroundMusicSource.volume < bgmVolume)
        {
            backgroundMusicSource.volume += 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
