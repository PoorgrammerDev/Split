using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    [SerializeField] private int sceneBuildIndex;
    [SerializeField] private SceneSwitcherGUI switchAnimator;
    [SerializeField] private AnimationClip animationClip;
    private bool readyToSwitchScenes = false;

    public void BeginAsyncProcess(bool disableButtons = true) {
        //disables all buttons
        if (disableButtons) {
            foreach (Button button in FindObjectsOfType<Button>()) {
                button.interactable = false;
            }
        }

        //animation for transition
        if (switchAnimator != null && animationClip != null) {
            switchAnimator.PlayAnimation(this, animationClip.name);
        }
        
        //fade out music and begin loading
        // StartCoroutine(musicManager.FadeOutAndStop(1f));
        StartCoroutine(LoadScene());
    }

    public void ActivateScene() {
        readyToSwitchScenes = true;
    }

    private IEnumerator LoadScene() {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Single);
        asyncOp.allowSceneActivation = false;

        while (!asyncOp.allowSceneActivation) {
            if (readyToSwitchScenes && asyncOp.progress >= 0.9f) {
                asyncOp.allowSceneActivation = true;
                break;
            }
            yield return null;
        }
    }

}
