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
    public int totalLeversFirstLevel = 0;
    [Space(10)]
    public int totalLeversSecondLevel = 0;
    [Space(10)]
    public int totalLeversThirdLevel = 0;

    [Header("Door In Scene")]
    [Space(5)]
    public DoorController doorControllerFirstLevel;
    public DoorController doorControllerSecondLevel;
    public DoorController doorControllerThirdLevel;

    [Header("Alden Controls")]
    [Space(5)]
    public Button aldenInteractButton;

    [Header("Audio Clips")]
    [Space(5)]
    public AudioClip backgroundMusic;
    public AudioClip[] soundEffects;

    [Header("Scene Components")]
    [Space(5)]
    public AldenTopDown aldenCharacter;
    public GameObject dungeonRootObject;
    public SceneTransition sceneTransition;
    public Animator doorTransitionAnimator;
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource aldenSFXSource;

    [Header("Menu Components")]
    public GameObject pauseMenuPanel;

    // Private Variables
    private int currentLevel = 1;

    private int leversTriggeredFirstLevel = 0;
    private int leversTriggeredSecondLevel = 0;
    private int leversTriggeredThirdLevel = 0;

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
        if(currentLevel == 1)
        {
            leversTriggeredFirstLevel++;
        }
        else if(currentLevel == 2)
        {
            leversTriggeredSecondLevel++;
        }
        else if(currentLevel == 3)
        {
            leversTriggeredThirdLevel++;
        }

        // Check if all levers are triggered to open the door
        CheckDoorOpenCondition();
    }

    public void DoorEntered(int teleportToLevel, Transform teleportPosition)
    {
        currentLevel = teleportToLevel;

        StartCoroutine(TeleportAlden(teleportPosition));
    }

    private IEnumerator TeleportAlden(Transform teleportPosition)
    {
        doorTransitionAnimator.SetTrigger("Blackout");
        yield return new WaitForSeconds(0.5f);
        aldenCharacter.gameObject.transform.position = teleportPosition.position;

        aldenCharacter.OnDownButton();
        yield return new WaitForSeconds(0.1f);
        aldenCharacter.OnReleaseButton();
    }

    private void CheckDoorOpenCondition()
    {
        if(currentLevel == 1)
        {
            if(leversTriggeredFirstLevel == totalLeversFirstLevel)
            {
                doorControllerFirstLevel.OpenDoor();
            }
        }
        else if(currentLevel == 2)
        {
            if (leversTriggeredSecondLevel == totalLeversSecondLevel)
            {
                doorControllerSecondLevel.OpenDoor();
            }
        }
        else if(currentLevel == 3)
        {
            if (leversTriggeredThirdLevel == totalLeversThirdLevel)
            {
                doorControllerThirdLevel.OpenDoor();
            }
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

    public void OnPauseButton()
    {
        pauseMenuPanel.gameObject.SetActive(true);
    }

    public void OnCancelButton()
    {
        pauseMenuPanel.gameObject.SetActive(false);
    }

    public void OnExitButton()
    {
        if(GameData.instance.maxCompletedLevel >= 11)
        {
            sceneTransition.LoadSceneWithTransition("DungeonSelectMenu");
        }
        else
        {   
            sceneTransition.LoadSceneWithTransition("Level_11-2");
        }
    }
}
