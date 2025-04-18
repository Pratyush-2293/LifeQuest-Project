using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    [HideInInspector] public static DungeonManager instance = null;

    [Header("Levers In Scene")]
    [Space(5)]
    public int totalLevers = 0;
    public LeverController[] leverControllers;

    [Header("Door In Scene")]
    [Space(5)]
    public DoorController doorController;

    [Header("Alden Controls")]
    [Space(5)]
    public Button aldenInteractButton;

    [Header("Audio Clips")]
    [Space(5)]
    public AudioClip backgroundMusic;
    public AudioClip[] soundEffects;

    [Header("Scene Components")]
    [Space(5)]
    public GameObject dungeonRootObject;
    public SceneTransition sceneTransition;
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource aldenSFXSource;

    // Private Variables
    private int leversTriggered = 0;

    private float sfxVolume;
    private float bgmVolume;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Trying to create more than once instance of DungeonManager!");
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.dungeonRootObject = dungeonRootObject;

        // Set volume of audio sources
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume_Combat", 1.0f);
        bgmVolume = PlayerPrefs.GetFloat("bgmVolume_Combat", 1.0f);
        RefreshVolumeLevels();

        // Start playing background music
        bgmSource.clip = backgroundMusic;
        bgmSource.Play();
    }

    public void LeverTriggered()
    {
        // Increment the levers triggered
        leversTriggered++;

        // Check if all levers are triggered to open the door
        CheckDoorOpenCondition();
    }

    private void CheckDoorOpenCondition()
    {
        if(leversTriggered == totalLevers)
        {
            doorController.OpenDoor();
        }
    }

    public void LoadDungeonCombatScene(string sceneName)
    {
        // Play the screen transition
        sceneTransition.LoadSceneWithTransitionAdditive(sceneName, dungeonRootObject);
    }

    private void RefreshVolumeLevels()
    {
        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
        aldenSFXSource.volume = sfxVolume;
    }

    public void PlaySoundEffect(string sfxName)
    {
        AudioClip sound = Array.Find(soundEffects, x => x.name == sfxName);

        if (sound == null)
        {
            Debug.LogWarning("Sound not found !");
        }
        else
        {
            sfxSource.PlayOneShot(sound);
        }
    }
}
