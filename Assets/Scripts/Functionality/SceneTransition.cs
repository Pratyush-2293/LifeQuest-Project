using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator sceneTransitionAnimator;
    public bool dontShowOnWake = false;

    private void Start()
    {
        if(dontShowOnWake == true)
        {
            sceneTransitionAnimator.SetTrigger("Idle");
        }

        dontShowOnWake = false;
    }

    public void LoadSceneWithTransition(string sceneName)
    {
        StartCoroutine(LoadSceneAfterTransition(sceneName, false));
    }

    public void LoadSceneWithTransitionAdditive(string sceneName, GameObject rootObject)
    {
        StartCoroutine(LoadSceneAfterTransition(sceneName, true, rootObject));
    }

    private IEnumerator LoadSceneAfterTransition(string sceneName, bool isAdditive, GameObject rootObject = null)
    {
        sceneTransitionAnimator.SetTrigger("SlideIn");
        yield return new WaitForSeconds(0.8f);
        if(isAdditive == true)
        {
            rootObject.gameObject.SetActive(false);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
