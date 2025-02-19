using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEditor;
using UnityEditorInternal;

public class VN_Manager : MonoBehaviour
{
    public List<GameObject> characters = new List<GameObject>();
    public Image background = null;
    public GameObject backgroundTransition = null;
    public GameObject nextButton = null;
    public List<Sprite> backgrounds = new List<Sprite>();
    public List<AudioClip> soundEffects = new List<AudioClip>();
    public List<AudioClip> backgroundMusic = new List<AudioClip>();
    public AudioSource backgroundMusicSource = null;
    public AudioSource SFX_Source = null;
    public TMP_Text leftName = null;
    public TMP_Text rightName = null;
    public TMP_Text dialogueBox = null;
    public List<DialogueEntry> dialogues = new List<DialogueEntry>();

    private float nextDialogue = 0;

    public void Start()
    {

    }

    public void OnNextButton() // Loads the scene with the next dialogue's data.
    {

        // Fetching the appropriate dialogue data for next step
        DialogueEntry currentDialogue = null;
        for (int i = 0; i <= dialogues.Count; i++)
        {
            if(dialogues[i].dialogueID == nextDialogue)
            {
                currentDialogue = dialogues[i];
                break;
            }
        }

        // Checking if BG transition is needed; -1 = Not Needed.
        if(currentDialogue!=null && currentDialogue.background != -1)
        {
            StartCoroutine(SwapBackground(currentDialogue.background));
            nextButton.gameObject.SetActive(false);                                 // to prevent user from skipping dialogue mid transition
        }

        // Making the active character slide into frame
        if(currentDialogue!=null && currentDialogue.character != -1)
        {
            if(currentDialogue.background != -1)
            {
                StartCoroutine(SlideAfterTransition(currentDialogue));
            }
            else
            {
                characters[currentDialogue.character].gameObject.SetActive(true);
            }
        }


    }

    public IEnumerator SwapBackground(int nextBackground)
    {
        backgroundTransition.gameObject.GetComponent<Animator>().SetTrigger("StartBlackout");
        Debug.Log("Trigger Started.");
        yield return new WaitForSeconds(1.1f);
        background.sprite = backgrounds[nextBackground];
        yield return new WaitForSeconds(0.4f);
        nextButton.gameObject.SetActive(true);
    }

    public IEnumerator SlideAfterTransition(DialogueEntry currentDialogue)
    {
        yield return new WaitForSeconds(1.5f);
        characters[currentDialogue.character].gameObject.SetActive(true);
    }
}

[Serializable] 
public class DialogueEntry
{
    public float dialogueID = -1;
    public float nextDialogue = -1;
    [TextArea(1, 5)]
    public string characterName = null;
    [TextArea(5, 10)]
    public string dialogueText = null;
    public int character = -1;
    public int background = -1;
    public int SFX = -1;
    public int BGM = -1;
    public bool lastDialogue = false;
    [TextArea(1, 5)]
    public String loadCombat = null;
}
