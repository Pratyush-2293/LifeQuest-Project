using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class VN_Manager : MonoBehaviour
{
    [Header("Level ID")]
    [Space(5)]
    public float levelID = 0f;
    public int skipToDialogueFromLast = 1;

    [Header("Character Prefabs")]
    [Space(5)]
    public List<Character> characters = new List<Character>();

    [Header("Background Sprites")]
    [Space(5)]
    public List<Sprite> backgrounds = new List<Sprite>();

    [Header("SFX Audioclips")]
    [Space(5)]
    public string buttonClickSound = "uiClick";
    public List<AudioClip> soundEffects = new List<AudioClip>();

    [Header("Background Musics")]
    [Space(5)]
    public AudioClip initialBackgroundMusic = null;
    public List<AudioClip> backgroundMusics = new List<AudioClip>();

    [Header("Create Dialogues:")]
    [Space(5)]
    public List<DialogueEntry> dialogues = new List<DialogueEntry>();

    [Header("Text Display Speed")]
    [Space(5)]
    public float textSpeed = 0.01f;

    [Header("Typing Sound Settings")]
    [Space(5)]
    public AudioClip[] textSounds = new AudioClip[2];
    [Space(5)]
    public float soundPitchMin = 0.9f;
    public float soundPitchMax = 1.1f;
    public int soundInterval = 1;

    [Header("Plug in these components:")]
    [Space(5)]
    public Image background = null;
    public GameObject backgroundTransition = null;
    public GameObject nextButton = null;
    public GameObject nameTab = null;
    public TMP_Text nameTabText;
    public TMP_Text dialogueBoxText = null;
    public GameObject dialogueOptionsPanel;
    public GameObject[] optionButtons = new GameObject[4];
    public TMP_Text[] optionButtonTexts = new TMP_Text[4];
    public LevelCompleteManager levelCompleteManager;
    public SceneTransition sceneTransition;

    // Internal Variables
    private DialogueEntry currentDialogue = null;
    private Animator nameTabAnimator;
    private float nextDialogue = 0;
    private int activeCharacter = -1;
    private bool nameTabAtLeft = true;
    private bool tabMoving = false;
    [HideInInspector] public int[] dialogueOptionPointers = new int[4];
    private bool dialoguesFinished = false;
    private string combatSceneName = "";
    private bool typingDialogue = false;
    private Coroutine typingCoroutine;
    public enum Expression { Neutral, Happy, Laughing, Serious, Surprised, Angry, Sad };

    public void Start()
    {
        // Triggering the next button to set up scene with first dialogue
        OnNextButton();
        characters[activeCharacter].gameObject.GetComponent<Animator>().SetTrigger("SlideInstant");  // using a faster version of animation to spawn character sooner

        // Playing the initial background music
        AudioManager.instance.PlayMusicClip(initialBackgroundMusic);

        nameTabAnimator = nameTab.gameObject.GetComponent<Animator>();
    }

    public IEnumerator UpdateSceneData(bool calledFromOptions = false)
    {
        // Check if dialogues are finished & load appropriate scene
        if (dialoguesFinished == true)
        {
            if(combatSceneName != "")
            {
                if(levelID > GameData.instance.levelViewedID)
                {
                    GameData.instance.levelViewedID = levelID;
                }
                SaveManager.instance.SaveGameData();
                sceneTransition.LoadSceneWithTransition(combatSceneName);
            }
            else
            {
                levelCompleteManager.LoadLevelCompletePanel(levelID > GameData.instance.levelViewedID ? true : false);
                if (levelID > GameData.instance.levelViewedID)
                {
                    GameData.instance.levelViewedID = levelID;
                }
                SaveManager.instance.SaveGameData();
            }
            yield break;
        }

        // Play button clicked SFX
        AudioManager.instance.PlayUISound(buttonClickSound);

        // Fetching the appropriate dialogue data for next step
        if (calledFromOptions == false)
        {
            for (int i = 0; i < dialogues.Count; i++)
            {
                if (dialogues[i].dialogueID == nextDialogue)
                {
                    currentDialogue = dialogues[i];
                    break;
                }
            }
        }

        // Check if this is an options dialogue & load appropriate options
        if (currentDialogue.showOptions == true)
        {
            dialogueOptionsPanel.gameObject.SetActive(true);
            dialogueOptionPointers = currentDialogue.optionPointers;

            for(int i = 0; i < currentDialogue.numberOfOptions; i++)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtonTexts[i].text = currentDialogue.optionTexts[i];
            }

            dialogueBoxText.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);

            yield break;
        }

        // Checking if BG transition is needed; -1 = Not Needed.
        if (currentDialogue != null && currentDialogue.background != -1)
        {
            StartCoroutine(SwapBackground(currentDialogue.background));
            if(characters[activeCharacter] != characters[currentDialogue.character])
            {
                characters[activeCharacter].CharacterSlideOut();
            }
            nextButton.gameObject.SetActive(false);                                 // to prevent user from skipping dialogue mid transition
            yield return new WaitForSeconds(1.5f);
        }

        // Making the first dialogue character slide into frame
        if (currentDialogue != null && currentDialogue.character != -1)
        {
            if(currentDialogue.dialogueID == 0)
            {
                characters[currentDialogue.character].CharacterSlideInstant();
            }
        }

        // Checking if the next dialogue is made by a different character.

        // First initialising the active character
        if (activeCharacter == -1)
        {
            activeCharacter = currentDialogue.character;
        }
        else // If already initialised, checking to see if active character needs to be changed.
        {
            if (currentDialogue.character != activeCharacter) // character needs to be changed
            {
                if(currentDialogue.background == -1)
                {
                    characters[activeCharacter].CharacterSlideOut();  // Removing the old active character
                }

                characters[currentDialogue.character].CharacterSlideIn();  //spawning in the new character after short delay
                activeCharacter = currentDialogue.character;  // Refreshing the current active character
            }
        }

        // Updating the character's expression
        characters[activeCharacter].UpdateExpression(currentDialogue.characterExpression);
        

        // Moving the name tab to right if a right character has become active
        if(characters[activeCharacter].isLeftCharacter == false)
        {
            if(nameTabAtLeft == true)
            {
                nameTabAnimator.SetTrigger("MoveRight");
                nameTabAtLeft = false;

                tabMoving = true;
            }
        }
        // Moving the name tab to left if a left character has become active
        if (characters[activeCharacter].isLeftCharacter == true)
        {
            if(nameTabAtLeft == false)
            {
                nameTabAnimator.SetTrigger("MoveLeft");
                nameTabAtLeft = true;

                tabMoving = true;
            }
        }

        // Inserting the character name in the name tab
        if (tabMoving == true)
        {
            StartCoroutine(InsertCharacterName(currentDialogue.characterName));   // with delay when name tab moves
            tabMoving = false;
        }
        else
        {
            nameTabText.text = currentDialogue.characterName;  // instantly when static
        }

        // Inserting character dialogue
        typingCoroutine = StartCoroutine(AnimateText(currentDialogue.dialogueText));

        // Playing dialogue SFX if it exists
        if (currentDialogue.SFX != -1)
        {
            AudioManager.instance.PlaySoundClip(soundEffects[currentDialogue.SFX]);
        }

        // Starting Screenshake if needed
        if (currentDialogue.screenShake == true)
        {
            CameraShake.instance.StartShake(2, true);
        }

        // Changing BGM if needed
        if (currentDialogue.BGM != -1)
        {
            AudioManager.instance.PlayMusicClip(backgroundMusics[currentDialogue.BGM]);
        }

        // Moving to next dialogue
        nextDialogue = currentDialogue.nextDialogue;

        // Check if this was the last scene
        if(currentDialogue.lastDialogue == true)
        {
            dialoguesFinished = true;
            combatSceneName = currentDialogue.loadCombat;
        }
    }

    public IEnumerator AnimateText(string characterDialogue)
    {
        typingDialogue = true;

        for (int i=0; i < characterDialogue.Length + 1; i++)
        {
            dialogueBoxText.text = characterDialogue.Substring(0, i);

            // Play typing sound
            if (i > 0 && (i % soundInterval == 0))
            {
                if (AudioManager.instance.textSoundSource != null && textSounds != null && textSounds.Length > 0)
                {
                    int soundIndex = UnityEngine.Random.Range(0, textSounds.Length);
                    AudioManager.instance.textSoundSource.clip = textSounds[soundIndex];
                    AudioManager.instance.textSoundSource.pitch = UnityEngine.Random.Range(soundPitchMin, soundPitchMax);
                    AudioManager.instance.textSoundSource.Play();

                    yield return null; //enable or disable this incase tweaking text sound frequency, could probably save some performance
                }
            }

            yield return new WaitForSeconds(textSpeed);
        }

        typingDialogue = false;
    }

    public IEnumerator InsertCharacterName(string characterName)
    {
        yield return new WaitForSeconds(0.25f);
        nameTabText.text = characterName;
    }

    public IEnumerator SwapBackground(int nextBackground)
    {
        backgroundTransition.gameObject.GetComponent<Animator>().SetTrigger("StartBlackout");
        Debug.Log("Trigger Started.");
        yield return new WaitForSeconds(1.1f);
        background.sprite = backgrounds[nextBackground];
        yield return new WaitForSeconds(0.4f);
        NextButtonActivator();
    }

    public void NextButtonActivator()
    {
        nextButton.gameObject.SetActive(true);
    }

    public IEnumerator SlideAfterTransition(DialogueEntry currentDialogue)
    {
        yield return new WaitForSeconds(1.5f);
        characters[currentDialogue.character].gameObject.SetActive(true);
    }

    private void HandleOptionButtonAction(int buttonID)
    {
        // Turn on the dialogue box
        dialogueBoxText.gameObject.SetActive(true);

        // Turn on the next button
        nextButton.SetActive(true);

        // Change the current dialogue according to the assigned pointer of the button
        currentDialogue = dialogues[dialogueOptionPointers[buttonID]];

        // Update the scene with the new dialogue
        StartCoroutine(UpdateSceneData(true));

        // Turn off all the option buttons
        foreach (GameObject optionButton in optionButtons)
        {
            optionButton.gameObject.SetActive(false);
        }

        // Close the options panel
        dialogueOptionsPanel.gameObject.SetActive(false);
    }

    public void SkipToDialogue()
    {
        nextDialogue = dialogues.Count - skipToDialogueFromLast;
        StartCoroutine(UpdateSceneData());
    }

    [ContextMenu("Auto Index")]
    public void AutoIndexDialogues()
    {
        for(int i = 0; i < dialogues.Count; i++)
        {
            dialogues[i].dialogueID = i;
            dialogues[i].nextDialogue = i + 1;
        }
    }

    // -------------------------- BUTTON FUNCTIONS --------------------------

    public void OnNextButton() // Loads the scene with the next dialogue's data.
    {
        // If still typing, skip animation and show full text instantly
        if (typingDialogue)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            dialogueBoxText.text = currentDialogue.dialogueText; // Show full text immediately
            typingDialogue = false;
        }
        else
        {
            StartCoroutine(UpdateSceneData());
        }
    }

    public void OnOption1Button()
    {
        HandleOptionButtonAction(0);
    }

    public void OnOption2Button()
    {
        HandleOptionButtonAction(1);
    }

    public void OnOption3Button()
    {
        HandleOptionButtonAction(2);
    }

    public void OnOption4Button()
    {
        HandleOptionButtonAction(3);
    }
}

[Serializable] 
public class DialogueEntry
{
    public float dialogueID = -1;
    public float nextDialogue = -1;
    public bool showOptions = false;
    [Range(0,4)]
    public int numberOfOptions = 0;
    public string[] optionTexts = new string[4];
    public int[] optionPointers = new int[4];
    [TextArea(1, 5)]
    public string characterName = null;
    [TextArea(5, 10)]
    public string dialogueText = null;
    public int character = -1;
    public VN_Manager.Expression characterExpression;
    public int background = -1;
    public int SFX = -1;
    public int BGM = -1;
    public bool screenShake = false;
    public bool lastDialogue = false;
    [TextArea(1, 5)]
    public String loadCombat = null;
}
   