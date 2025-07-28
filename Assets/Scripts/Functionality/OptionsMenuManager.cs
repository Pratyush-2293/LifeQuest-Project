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

    [Header("Mute Button Icons")]
    [Space(5)]
    [SerializeField] private Image bgmMuteButtonImage = null;
    [SerializeField] private Image sfxMuteButtonImage = null;
    [SerializeField] private Image uiMuteButtonImage = null;
    [Space(10)]
    [SerializeField] private Sprite unmutedIcon = null;
    [SerializeField] private Sprite mutedIcon = null;

    [Header("Other Components")]
    [SerializeField] private SceneTransition sceneTransition;

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
        AudioManager.instance.PlayUISound("uiClick2");

        // activate touch blocker to prevent interference into the game
        touchBlockerPanel.gameObject.SetActive(true);

        // display the options menu
        optionsMenu.gameObject.SetActive(true);

        // stop time to pause the game
        Time.timeScale = 0;
    }

    public void OnResumeGameButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        // resume time to unpause the game
        Time.timeScale = 1;

        // remove touch blocker panel
        touchBlockerPanel.gameObject.SetActive(false);

        // hide the options menu
        optionsMenu.gameObject.SetActive(false);
    }

    public void OnSoundOptionsButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        // hide options menu
        optionsMenu.gameObject.SetActive(false);

        // Load sound data into the sliders
        bgmVolumeSlider.value = AudioManager.instance.bgmVolume;
        sfxVolumeSlider.value = AudioManager.instance.sfxVolume;
        uiVolumeSlider.value = AudioManager.instance.uiVolume;

        // Load mute icon data into mute buttons
        UpdateMuteButtonIcons();

        // show the sound menu
        soundMenu.gameObject.SetActive(true);
    }

    public void OnBGMMuteButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        if (AudioManager.instance.bgmIsMuted == false)
        {
            bgmVolumeSlider.value = 0;
        }
        else
        {
            bgmVolumeSlider.value = AudioManager.instance.bgmMutedSave;
        }

        AudioManager.instance.MuteBGM();

        UpdateMuteButtonIcons();
    }

    public void OnSFXMuteButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        if (AudioManager.instance.sfxIsMuted == false)
        {
            sfxVolumeSlider.value = 0;
        }
        else
        {
            sfxVolumeSlider.value = AudioManager.instance.sfxMutedSave;
        }

        AudioManager.instance.MuteSFX();

        UpdateMuteButtonIcons();
    }

    public void OnUIMuteButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        if (AudioManager.instance.uiIsMuted == false)
        {
            uiVolumeSlider.value = 0;
        }
        else
        {
            uiVolumeSlider.value = AudioManager.instance.uiMutedSave;
        }

        AudioManager.instance.MuteUI();

        UpdateMuteButtonIcons();
    }

    public void OnApplyButton()
    {
        AudioManager.instance.RefreshVolumeLevels(bgmVolumeSlider.value, sfxVolumeSlider.value, uiVolumeSlider.value);

        AudioManager.instance.PlayUISound("uiClick2");

        // Return to options menu
        soundMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(true);
    }

    public void OnLeaveButton()
    {
        Time.timeScale = 1;
        AudioManager.instance.PlayUISound("uiClick2");
        AudioManager.instance.PlayMusic("hometownVillage");

        // add functionality here
        sceneTransition.LoadSceneWithTransition("LevelSelectMenu");
    }

    private void UpdateMuteButtonIcons()
    {
        if (AudioManager.instance.bgmIsMuted == true)
        {
            bgmMuteButtonImage.sprite = mutedIcon;
        }
        else
        {
            bgmMuteButtonImage.sprite = unmutedIcon;
        }

        if (AudioManager.instance.sfxIsMuted == true)
        {
            sfxMuteButtonImage.sprite = mutedIcon;
        }
        else
        {
            sfxMuteButtonImage.sprite = unmutedIcon;
        }

        if (AudioManager.instance.uiIsMuted == true)
        {
            uiMuteButtonImage.sprite = mutedIcon;
        }
        else
        {
            uiMuteButtonImage.sprite = unmutedIcon;
        }
    }
}
