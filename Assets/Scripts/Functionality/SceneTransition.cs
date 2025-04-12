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
    }

    public void LoadSceneWithTransition(string sceneName)
    {
        StartCoroutine(LoadSceneAfterTransition(sceneName));
    }

    private IEnumerator LoadSceneAfterTransition(string sceneName)
    {
        sceneTransitionAnimator.SetTrigger("SlideIn");
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(sceneName);
    }
}
