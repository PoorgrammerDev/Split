using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitcherGUI : MonoBehaviour
{
    [SerializeField] private AsyncSceneLoader sceneLoader;
 
    public void Activate() {
        sceneLoader.ActivateScene();
    }
}
