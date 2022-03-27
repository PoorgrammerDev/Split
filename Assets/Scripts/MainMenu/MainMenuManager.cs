using UnityEngine;
using Split;
using Split.LevelLoading;

public class MainMenuManager : MonoBehaviour {
   
	[SerializeField] private AsyncSceneLoader gameSceneLoader;
	[SerializeField] private AsyncSceneLoader builderSceneLoader;
	
	[SerializeField] private CanvasGroup[] UIWindows;
	
	private void Start() {
		MenuEvents.current.onLevelEntryPress += StartGame;
	}

	public void BuilderClick() {
		builderSceneLoader.BeginAsyncProcess();
	}
	
	public void ExitClick() {
		Application.Quit();
	}

	// ============
	//  UI Buttons
	// ============

	//Toggles a window / canvas group
	public void ToggleWindow(int num) {
		UIWindows[num].gameObject.SetActive(!UIWindows[num].gameObject.activeInHierarchy);
	}

	public void StartGame(LevelEntry levelEntry) {
		GameObject obj = new GameObject("LevelDataHolder");
		LevelDataHolder dataHolder = obj.AddComponent<LevelDataHolder>();
		dataHolder.Data = levelEntry.Data.ToLevelData();
		
		DontDestroyOnLoad(obj);
		gameSceneLoader.BeginAsyncProcess();
	}

 
}
