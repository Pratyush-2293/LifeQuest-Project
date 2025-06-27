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
    [SerializeField] private Slider uiVolumeSlider = null;

    [Header("Other Components")]
    [SerializeField] private SceneTransition sceneTransition;

    // Use when you have muted and unmuted images
    /*
    [SerializeField] private Image bgmMuteButtonImage = null;
    [SerializeField] private Image sfxMuteButtonImage = null;
    [SerializeField] private Sprite unmutedIcon = null;
    [SerializeField] private Sprite mutedIcon = null;
    */

    private void Awake()
    {
        if (instance)
        {
            Debug.Log("Trying to create more than one OptionsMenuManager!");
            Destroy(gameObject);
        }
        instance = this;
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
        bgmVolumeSlider.value = AudioManager.instance.bgmVolume;
        sfxVolumeSlider.value = AudioManager.instance.sfxVolume;
        uiVolumeSlider.value = AudioManager.instance.uiVolume;

        // show the sound menu
        soundMenu.gameObject.SetActive(true);
    }

    public void OnBGMMuteButton()
    {
        if (AudioManager.instance.bgmIsMuted == false)
        {
            bgmVolumeSlider.value = 0;
        }
        else
        {
            bgmVolumeSlider.value = AudioManager.instance.bgmMutedSave;
        }

        AudioManager.instance.MuteBGM();
    }

    public void OnSFXMuteButton()
    {
        if (AudioManager.instance.sfxIsMuted == false)
        {
            sfxVolumeSlider.value = 0;
        }
        else
        {
            sfxVolumeSlider.value = AudioManager.instance.sfxMutedSave;
        }

        AudioManager.instance.MuteSFX();
    }

    public void OnUIMuteButton()
    {
        if (AudioManager.instance.uiIsMuted == false)
        {
            uiVolumeSlider.value = 0;
        }
        else
        {
            uiVolumeSlider.value = AudioManager.instance.uiMutedSave;
        }

        AudioManager.instance.MuteUI();
    }

    public void OnApplyButton()
    {
        AudioManager.instance.RefreshVolumeLevels(bgmVolumeSlider.value, sfxVolumeSlider.value, uiVolumeSlider.value);

        // Return to options menu
        soundMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(true);
    }

    public void OnLeaveButton()
    {
        // add functionality here
        sceneTransition.LoadSceneWithTransition("LevelSelectMenu");
    }
}
