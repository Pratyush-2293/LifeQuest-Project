using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : Menu
{
    public static SettingsMenu instance = null;

    [Header("Menu Panels")]
    [Space(5)]
    public GameObject mainMenuPanel;
    public GameObject audioSettingsPanel;
    public GameObject notificationSettingsPanel;
    public GameObject creditsPanel;

    [Header("Audio Settings Components")]
    [Space(5)]
    public Slider generalMusicVolumeSlider;
    public Slider generalSFXVolumeSlider;
    public Slider combatMusicVolumeSlider;
    public Slider combatSFXVolumeSlider;

    // Private Variables
    private float generalMusicVolume;
    private float generalSFXVolume;
    private float combatMusicVolume;
    private float combatSFXVolume;

    private bool generalMusicIsMuted = false;
    private bool generalSFXIsMuted = false;
    private bool combatMusicIsMuted = false;
    private bool combatSFXIsMuted = false;

    private float generalMusicMutedSave;
    private float generalSFXMutedSave;
    private float combatMusicMutedSave;
    private float combatSFXMutedSave;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one MainMenu!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        generalMusicVolume = PlayerPrefs.GetFloat("bgmVolume_VN", 1.0f);
        generalSFXVolume = PlayerPrefs.GetFloat("sfxVolume_VN", 1.0f);
        combatMusicVolume = PlayerPrefs.GetFloat("bgmVolume_Combat", 1.0f);
        combatSFXVolume = PlayerPrefs.GetFloat("sfxVolume_Combat", 1.0f);

        generalMusicVolumeSlider.value = generalMusicVolume;
        generalSFXVolumeSlider.value = generalSFXVolume;
        combatMusicVolumeSlider.value = combatMusicVolume;
        combatSFXVolumeSlider.value = combatSFXVolume;

        generalMusicMutedSave = PlayerPrefs.GetFloat("bgmMutedSave_VN", 1f);
        generalSFXMutedSave = PlayerPrefs.GetFloat("sfxMutedSave_VN", 1f);
        combatMusicMutedSave = PlayerPrefs.GetFloat("bgmMutedSave_Combat", 1f);
        combatSFXMutedSave = PlayerPrefs.GetFloat("sfxMutedSave_Combat", 1f);

        // Load mute states
        generalMusicIsMuted = PlayerPrefs.GetInt("bgmIsMuted_VN", 0) == 1;
        generalSFXIsMuted = PlayerPrefs.GetInt("sfxIsMuted_VN", 0) == 1;
        combatMusicIsMuted = PlayerPrefs.GetInt("bgmIsMuted_Combat", 0) == 1;
        combatSFXIsMuted = PlayerPrefs.GetInt("sfxIsMuted_Combat", 0) == 1;
    }


    // ----------------------- BUTTON FUNCTIONS -----------------------

    public void OnBackButton()
    {
        TurnOff(true);
    }

    public void OnAudioSettingsButton()
    {
        mainMenuPanel.gameObject.SetActive(false);
        audioSettingsPanel.gameObject.SetActive(true);
    }

    public void OnGeneralMusicMuteButton()
    {
        if (generalMusicIsMuted == false)
        {
            MenusAudioManager.instance.ChangeVolumeLevels(0, generalSFXVolume);
            generalMusicVolumeSlider.value = 0;
            generalMusicMutedSave = generalMusicVolume;
            PlayerPrefs.SetFloat("bgmMutedSave_VN", generalMusicMutedSave);
            generalMusicIsMuted = true;
        }
        else
        {
            MenusAudioManager.instance.ChangeVolumeLevels(generalMusicMutedSave, generalSFXVolume);
            generalMusicVolumeSlider.value = generalMusicMutedSave;
            generalMusicIsMuted = false;
        }

        PlayerPrefs.SetInt("bgmIsMuted_VN", generalMusicIsMuted ? 1 : 0);  // Save the mute state
        PlayerPrefs.Save();
    }

    public void OnGeneralSFXMuteButton()
    {
        if (generalSFXIsMuted == false)
        {
            MenusAudioManager.instance.ChangeVolumeLevels(generalMusicVolume, 0);
            generalSFXVolumeSlider.value = 0;
            generalSFXMutedSave = generalSFXVolume;
            PlayerPrefs.SetFloat("sfxMutedSave_VN", generalSFXMutedSave);
            generalSFXIsMuted = true;
        }
        else
        {
            MenusAudioManager.instance.ChangeVolumeLevels(generalMusicVolume, generalSFXMutedSave);
            generalSFXVolumeSlider.value = generalSFXMutedSave;
            generalSFXIsMuted = false;
        }

        PlayerPrefs.SetInt("sfxIsMuted_VN", generalSFXIsMuted ? 1 : 0);  // Save the mute state
        PlayerPrefs.Save();
    }

    public void OnCombatMusicMuteButton()
    {
        if (combatMusicIsMuted == false)
        {
            MenusAudioManager.instance.ChangeVolumeLevels(0, combatSFXVolume);
            combatMusicVolumeSlider.value = 0;
            combatMusicMutedSave = combatMusicVolume;
            PlayerPrefs.SetFloat("bgmMutedSave_Combat", combatMusicMutedSave);
            combatMusicIsMuted = true;
        }
        else
        {
            MenusAudioManager.instance.ChangeVolumeLevels(combatMusicMutedSave, combatSFXVolume);
            combatMusicVolumeSlider.value = combatMusicMutedSave;
            combatMusicIsMuted = false;
        }

        PlayerPrefs.SetInt("bgmIsMuted_Combat", combatMusicIsMuted ? 1 : 0);  // Save the mute state
        PlayerPrefs.Save();
    }

    public void OnCombatSFXMuteButton()
    {
        if (combatSFXIsMuted == false)
        {
            MenusAudioManager.instance.ChangeVolumeLevels(combatMusicVolume, 0);
            combatSFXVolumeSlider.value = 0;
            combatSFXMutedSave = combatSFXVolume;
            PlayerPrefs.SetFloat("sfxMutedSave_Combat", combatSFXMutedSave);
            combatSFXIsMuted = true;
        }
        else
        {
            MenusAudioManager.instance.ChangeVolumeLevels(combatMusicVolume, combatSFXMutedSave);
            combatSFXVolumeSlider.value = combatSFXMutedSave;
            combatSFXIsMuted = false;
        }

        PlayerPrefs.SetInt("sfxIsMuted_Combat", combatSFXIsMuted ? 1 : 0);  // Save the mute state
        PlayerPrefs.Save();
    }

    public void OnApplyButton()
    {
        // Update internal variables with new values
        generalMusicVolume = generalMusicVolumeSlider.value;
        generalSFXVolume = generalSFXVolumeSlider.value;
        combatMusicVolume = combatMusicVolumeSlider.value;
        combatSFXVolume = combatSFXVolumeSlider.value;

        // Save new volume values to PlayerPrefs
        PlayerPrefs.SetFloat("bgmVolume_VN", generalMusicVolume);
        PlayerPrefs.SetFloat("sfxVolume_VN", generalSFXVolume);
        PlayerPrefs.SetFloat("bgmVolume_Combat", combatMusicVolume);
        PlayerPrefs.SetFloat("sfxVolume_Combat", combatSFXVolume);
        PlayerPrefs.Save();

        // Return to main menu
        audioSettingsPanel.gameObject.SetActive(false);
        mainMenuPanel.gameObject.SetActive(true);
    }
}
