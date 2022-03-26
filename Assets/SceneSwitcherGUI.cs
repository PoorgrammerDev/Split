using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SceneSwitcherGUI : MonoBehaviour
{
    private AsyncSceneLoader loader;
    private Animator animator;

    private void Awake() {
        this.animator = this.GetComponent<Animator>();
    }
 
    public void PlayAnimation(AsyncSceneLoader loader, string clipName) {
        this.loader = loader;
        animator.Play(clipName);
    }

    public void Activate() {
        if (loader != null) loader.ActivateScene();
    }
}
