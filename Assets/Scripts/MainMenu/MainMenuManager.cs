using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
   
	[SerializeField] private AsyncSceneLoader gameSceneLoader;
	[SerializeField] private AsyncSceneLoader builderSceneLoader;

	public void PlayClick() {
		
	}

	public void BuilderClick() {
		builderSceneLoader.BeginAsyncProcess();
	}

	public void SettingsClick() {
		
	}
	
	public void TutorialClick() {
		
	}
 
	public void AboutClick() {
		
	}
	
	public void ExitClick() {
		Application.Quit();
	}
 
}
