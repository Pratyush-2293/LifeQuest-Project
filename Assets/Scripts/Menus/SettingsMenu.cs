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
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider uiVolumeSlider;

    [Header("Mute Button Icons")]
    [Space(5)]
    [SerializeField] private Image bgmMuteButtonImage = null;
    [SerializeField] private Image sfxMuteButtonImage = null;
    [SerializeField] private Image uiMuteButtonImage = null;
    [Space(10)]
    [SerializeField] private Sprite unmutedIcon = null;
    [SerializeField] private Sprite mutedIcon = null;

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


    // ----------------------- BUTTON FUNCTIONS -----------------------

    public void OnBackButton()
    {
        AudioManager.instance.PlayUISound("uiClick2");

        MainMenu.instance.sceneTransition.gameObject.SetActive(false);
        TurnOff(true);
    }

    public void OnAudioSettingsButton()
    {
        AudioManager.instance.PlayUISound("uiClick");

        // hide options menu
        mainMenuPanel.gameObject.SetActive(false);

        // Load sound data into the sliders
        bgmVolumeSlider.value = AudioManager.instance.bgmVolume;
        sfxVolumeSlider.value = AudioManager.instance.sfxVolume;
        uiVolumeSlider.value = AudioManager.instance.uiVolume;

        // Load mute icon data into mute buttons
        UpdateMuteButtonIcons();

        //show the audio menu
        audioSettingsPanel.gameObject.SetActive(true);
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
        AudioManager.instance.PlayUISound("uiClick2");

        AudioManager.instance.RefreshVolumeLevels(bgmVolumeSlider.value, sfxVolumeSlider.value, uiVolumeSlider.value);

        // Return to options menu
        audioSettingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
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
