using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    public static OptionsMenuManager instance = null;

    [Header("Menu GameObjects")]
    [SerializeField] private GameObject optionsMenu = null;
    [SerializeField] private GameObject soundMenu = null;
    [SerializeField] private GameObject touchBlockerPanel = null;

    [Header("Sound Menu Components")]
    [SerializeField] private Slider bgmVolumeSlider = null;
    [SerializeField] private Slider sfxVolumeSlider = null;

    // Use when you have muted and unmuted images
    /*
    [SerializeField] private Image bgmMuteButtonImage = null;
    [SerializeField] private Image sfxMuteButtonImage = null;
    [SerializeField] private Sprite unmutedIcon = null;
    [SerializeField] private Sprite mutedIcon = null;
    */

    private float bgmVolume;
    private float sfxVolume;

    private bool bgmIsMuted = false;
    private bool sfxIsMuted = false;

    private float bgmMutedSave;
    private float sfxMutedSave;

    private void Awake()
    {
        if (instance)
        {
            Debug.Log("Trying to create more than one OptionsMenuManager!");
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
        bgmVolume = PlayerPrefs.GetFloat("bgmVolume_Combat", 1.0f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume_Combat", 1.0f);

        bgmMutedSave = PlayerPrefs.GetFloat("bgmMutedSave_Combat", 1f);
        sfxMutedSave = PlayerPrefs.GetFloat("sfxMutedSave_Combat", 1f);

        // Load mute states
        bgmIsMuted = PlayerPrefs.GetInt("bgmIsMuted_Combat", 0) == 1;
        sfxIsMuted = PlayerPrefs.GetInt("sfxIsMuted_Combat", 0) == 1;
    }

    public void OnOptionsButton()
    {
        // activate touch blocker to prevent interference into the game
        touchBlockerPanel.gameObject.SetActive(true);

        // display the options menu
        optionsMenu.gameObject.SetActive(true);

        // stop time to pause the game
        Time.timeScale = 0;
    }

    public void OnResumeGameButton()
    {
        // resume time to unpause the game
        Time.timeScale = 1;

        // remove touch blocker panel
        touchBlockerPanel.gameObject.SetActive(false);

        // hide the options menu
        optionsMenu.gameObject.SetActive(false);
    }

    public void OnSoundOptionsButton()
    {
        // hide options menu
        optionsMenu.gameObject.SetActive(false);

        // Load sound data into the sliders
        bgmVolumeSlider.value = bgmVolume;
        sfxVolumeSlider.value = sfxVolume;

        // show the sound menu
        soundMenu.gameObject.SetActive(true);
    }

    public void OnBGMMuteButton()
    {
        if(bgmIsMuted == false)
        {
            AudioManager.instance.ChangeVolumeLevels(0, sfxVolume);
            bgmVolumeSlider.value = 0;
            bgmMutedSave = bgmVolume;
            PlayerPrefs.SetFloat("bgmMutedSave_Combat", bgmMutedSave);
            bgmIsMuted = true;
        }
        else
        {
            AudioManager.instance.ChangeVolumeLevels(bgmMutedSave, sfxVolume);
            bgmVolumeSlider.value = bgmMutedSave;
            bgmIsMuted = false;
        }

        PlayerPrefs.SetInt("bgmIsMuted_Combat", bgmIsMuted ? 1 : 0);  // Save the mute state
        PlayerPrefs.Save();
    }

    public void OnSFXMuteButton()
    {
        if (sfxIsMuted == false)
        {
            AudioManager.instance.ChangeVolumeLevels(bgmVolume, 0);
            sfxVolumeSlider.value = 0;
            sfxMutedSave = sfxVolume;
            PlayerPrefs.SetFloat("sfxMutedSave_Combat", sfxMutedSave);
            sfxIsMuted = true;
        }
        else
        {
            AudioManager.instance.ChangeVolumeLevels(bgmVolume, sfxMutedSave);
            sfxVolumeSlider.value = sfxMutedSave;
            sfxIsMuted = false;
        }

        PlayerPrefs.SetInt("sfxIsMuted_Combat", sfxIsMuted ? 1 : 0);  // Save the mute state
        PlayerPrefs.Save();
    }

    public void OnApplyButton()
    {
        // Update internal variables with new values
        bgmVolume = bgmVolumeSlider.value;
        sfxVolume = sfxVolumeSlider.value;

        // Notify Audiomanager about volume change
        AudioManager.instance.ChangeVolumeLevels(bgmVolume, sfxVolume);

        // Save new volume values to PlayerPrefs
        PlayerPrefs.SetFloat("bgmVolume_Combat", bgmVolume);
        PlayerPrefs.SetFloat("sfxVolume_Combat", sfxVolume);

        // Return to options menu
        soundMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(true);
    }

    public void OnLeaveButton()
    {
        // add functionality here
    }
}
