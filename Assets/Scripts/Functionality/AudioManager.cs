using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    [Header("Musics")]
    [SerializeField] private AudioClip[] backgroundMusics;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] soundEffects;

    [Header("UI Sound Effects")]
    [SerializeField] private AudioClip[] uiSoundEffects;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource = null;
    [SerializeField] private AudioSource[] sfxSources = new AudioSource[3];  // Can be scaled if more AudioSources needed in pool
    [SerializeField] private AudioSource[] uiSources = new AudioSource[3];
    [SerializeField] public AudioSource textSoundSource = null;  // This one is also a part of UI but it fires very rapidly and is thus placed in it's own channel.

    private int currentSfxSource = 0;
    private int currentUiSource = 0;

    // Player volume settings
    public float sfxVolume;
    public float bgmVolume;
    public float uiVolume;

    public bool bgmIsMuted = false;
    public bool sfxIsMuted = false;
    public bool uiIsMuted = false;

    public float bgmMutedSave;
    public float sfxMutedSave;
    public float uiMutedSave;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Trying to create more than one AudioManager!");
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
        // Set volume of audio sources
        sfxVolume = PlayerPrefs.GetFloat("SFX_Volume", 1.0f);
        bgmVolume = PlayerPrefs.GetFloat("BGM_Volume", 1.0f);
        uiVolume = PlayerPrefs.GetFloat("UI_Volume", 1.0f);

        // Retrieve muted save values for audio sources
        bgmMutedSave = PlayerPrefs.GetFloat("bgmMutedSave", 1f);
        sfxMutedSave = PlayerPrefs.GetFloat("sfxMutedSave", 1f);
        uiMutedSave = PlayerPrefs.GetFloat("uiMutedSave", 1f);

        // Load mute states
        bgmIsMuted = PlayerPrefs.GetInt("bgmIsMuted", 0) == 1;
        sfxIsMuted = PlayerPrefs.GetInt("sfxIsMuted", 0) == 1;
        uiIsMuted = PlayerPrefs.GetInt("uiIsMuted", 0) == 1;

        RefreshVolumeLevels();
    }

    public void ChangeVolumeLevels(float bgmVolumeLevel, float sfxVolumeLevel, float uiVolumeLevel)
    {
        bgmVolume = bgmVolumeLevel;
        sfxVolume = sfxVolumeLevel;
        uiVolume = uiVolumeLevel;
        PlayerPrefs.SetFloat("BGM_Volume", bgmVolume);
        PlayerPrefs.SetFloat("SFX_Volume", sfxVolume);
        PlayerPrefs.SetFloat("UI_Volume", uiVolume);
        PlayerPrefs.Save();

        RefreshVolumeLevels();
    }

    public void PlaySound(string audioName)
    {
        AudioClip sound = Array.Find(soundEffects, x => x.name == audioName);

        if (sound == null)
        {
            Debug.LogWarning("Sound not found!");
            return;
        }

        // Alternating between sound channels to prevent interrupting active SFX
        sfxSources[currentSfxSource].PlayOneShot(sound);
        currentSfxSource = (currentSfxSource + 1) % sfxSources.Length;
    }

    public void PlaySoundClip(AudioClip audioClip)
    {
        // Alternating between sound channels to prevent interrupting active SFX
        sfxSources[currentSfxSource].PlayOneShot(audioClip);
        currentSfxSource = (currentSfxSource + 1) % sfxSources.Length;
    }

    public void PlayUISound(string audioName)
    {
        AudioClip sound = Array.Find(uiSoundEffects, x => x.name == audioName);

        if (sound == null)
        {
            Debug.LogWarning("UI Sound not found !");
        }
        else
        {
            // Alternating between sound channels to prevent interrupting active SFX
            uiSources[currentUiSource].PlayOneShot(sound);
            currentUiSource = (currentUiSource + 1) % sfxSources.Length;
        }
    }

    public void PlayMusic(string audioName)
    {
        AudioClip music = Array.Find(backgroundMusics, x => x.name == audioName);

        if(music == null)
        {
            Debug.LogWarning("Music not found !");
        }
        else if(bgmSource.isPlaying == false)
        {
            bgmSource.clip = music;
            bgmSource.Play();
        }
        else
        {
            StartCoroutine(BGMFade(music));
        }
    }

    public void PlayMusicClip(AudioClip audioClip)
    {
        if(bgmSource.isPlaying == false)
        {
            bgmSource.clip = audioClip;
            bgmSource.Play();
        }
        else
        {
            StartCoroutine(BGMFade(audioClip));
        }
    }

    public void StopMusicPlayback()
    {
        bgmSource.Stop();
    }

    public void MuteBGM()
    {
        if (bgmIsMuted == false)
        {
            bgmMutedSave = bgmVolume;
            bgmVolume = 0;
            RefreshVolumeLevels();
            PlayerPrefs.SetFloat("bgmMutedSave", bgmMutedSave);
            bgmIsMuted = true;
        }
        else
        {
            bgmVolume = bgmMutedSave;
            RefreshVolumeLevels();
            bgmIsMuted = false;
        }

        PlayerPrefs.SetInt("bgmIsMuted", bgmIsMuted ? 1 : 0);  // Save the mute state
        PlayerPrefs.Save();
    }

    public void MuteSFX()
    {
        if (sfxIsMuted == false)
        {
            sfxMutedSave = sfxVolume;
            sfxVolume = 0;
            RefreshVolumeLevels();
            PlayerPrefs.SetFloat("sfxMutedSave", sfxMutedSave);
            sfxIsMuted = true;
        }
        else
        {
            sfxVolume = sfxMutedSave;
            RefreshVolumeLevels();
            sfxIsMuted = false;
        }

        PlayerPrefs.SetInt("sfxIsMuted", sfxIsMuted ? 1 : 0);  // Save the mute state
        PlayerPrefs.Save();
    }

    public void MuteUI()
    {
        if (uiIsMuted == false)
        {
            uiMutedSave = uiVolume;
            uiVolume = 0;
            RefreshVolumeLevels();
            PlayerPrefs.SetFloat("uiMutedSave", uiMutedSave);
            uiIsMuted = true;
        }
        else
        {
            uiVolume = uiMutedSave;
            RefreshVolumeLevels();
            uiIsMuted = false;
        }

        PlayerPrefs.SetInt("uiIsMuted", uiIsMuted ? 1 : 0);  // Save the mute state
        PlayerPrefs.Save();
    }

    public void RefreshVolumeLevels(float bgmVolume, float sfxVolume, float uiVolume)
    {
        bgmSource.volume = bgmVolume;

        foreach (AudioSource sfxSource in sfxSources)
        {
            sfxSource.volume = sfxVolume;
        }

        foreach (AudioSource uiSource in uiSources)
        {
            uiSource.volume = uiVolume;
        }

        textSoundSource.volume = uiVolume;

        this.bgmVolume = bgmVolume;
        this.sfxVolume = sfxVolume;
        this.uiVolume = uiVolume;

        SaveVolumeLevels();
    }

    public bool isPlayingBGMTrack(string bgmName)
    {
        if(bgmSource.clip != null)
        {
            if (bgmSource.clip.name == bgmName)
            {
                return true;
            }
        }

        return false;
    }

    private void RefreshVolumeLevels()
    {
        bgmSource.volume = bgmVolume;

        foreach(AudioSource sfxSource in sfxSources)
        {
            sfxSource.volume = sfxVolume;
        }

        foreach(AudioSource uiSource in uiSources)
        {
            uiSource.volume = uiVolume;
        }

        textSoundSource.volume = uiVolume;

        SaveVolumeLevels();
    }

    private void SaveVolumeLevels()
    {
        // Save volume of audio sources
        PlayerPrefs.SetFloat("SFX_Volume", sfxVolume);
        PlayerPrefs.SetFloat("BGM_Volume", bgmVolume);
        PlayerPrefs.SetFloat("UI_Volume", uiVolume);

        // Save muted save values for audio sources
        PlayerPrefs.SetFloat("bgmMutedSave", bgmMutedSave);
        PlayerPrefs.SetFloat("sfxMutedSave", sfxMutedSave);
        PlayerPrefs.SetFloat("uiMutedSave", uiMutedSave);

        // Save mute states
        PlayerPrefs.SetInt("bgmIsMuted", bgmIsMuted ? 1 : 0);
        PlayerPrefs.SetInt("sfxIsMuted", sfxIsMuted ? 1 : 0);
        PlayerPrefs.SetInt("uiIsMuted", uiIsMuted ? 1 : 0);
    }

    private IEnumerator BGMFade(AudioClip musicToFade)
    {
        while (bgmSource.volume > 0)
        {
            bgmSource.volume -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        bgmSource.clip = musicToFade;
        bgmSource.Play();

        while (bgmSource.volume < bgmVolume)
        {
            bgmSource.volume += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
