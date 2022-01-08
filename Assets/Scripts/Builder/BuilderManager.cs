using UnityEngine;
using Split.Builder.CameraStates;

namespace Split.Builder {
    /// <summary>
    /// Manages the Builder Game Scene, aside from the Menus
    /// </summary>
    public class BuilderManager : MonoBehaviour {
        [Header("References")]
        [SerializeField] private CameraController cameraController;
        [SerializeField] private BuilderLevelLoader levelLoader;
        [SerializeField] private BuilderMainMenu mainMenuManager;

        [Header("UI References")]
        [SerializeField] private GameObject builderHUD;

        private BuilderLevelData data;
        
        public void OpenLevel(BuilderLevelData data) {
            this.data = data;

            //Generate the level based on the data
            levelLoader.Generate(data);

            //Close main menu
            mainMenuManager.CloseAllMenus();

            //Open builder HUD
            builderHUD.SetActive(true);

            //Activate camera
            cameraController.SetState(new IsoTilebound(cameraController, levelLoader));
        }

    }
}


