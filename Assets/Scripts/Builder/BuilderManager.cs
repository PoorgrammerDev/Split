using UnityEngine;
using UnityEngine.UI;
using Split.Builder.CameraStates;
using Split.Tiles;

namespace Split.Builder {
    /// <summary>
    /// Manages the Builder Game Scene, aside from the Menus
    /// </summary>
    public class BuilderManager : MonoBehaviour {
        [Header("Options")]
        [SerializeField] private float freeMoveSpeed;

        [Header("References")]
        [SerializeField] private CameraController cameraController;
        [SerializeField] private BuilderLevelLoader levelLoader;
        [SerializeField] private BuilderMainMenu mainMenuManager;

        [Header("UI")]
        [SerializeField] private GameObject builderHUD;
        [SerializeField] private GameObject cameraModeMenu;

        [Header("UI - Base Topbar")]
        [SerializeField] private GameObject normalTopbar;
        [SerializeField] private Image tileTypeButton;

        [Header("UI - Tile Type Selector")]
        [SerializeField] private GameObject tileTypeSelTopbar;
        [SerializeField] private Image selectedTileType;

        [Header("UI - Data Mode")]
        [SerializeField] private GameObject dataModeTopbar;

        private BuilderLevelData data;
        private TileType currentType;
        private bool eraseMode;

        private void Awake() {
            this.currentType = TileType.BASIC;
            this.eraseMode = false;
        }

        private void Start() {
            this.tileTypeButton.color = levelLoader.GetTileColor(this.currentType);
        }
        
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
            // cameraController.SetState(new IsoFreeMove(cameraController, 10f));
        }

        /*************
        *     UI     *
        *************/

        public void ToggleTileTypeSelector(bool active) {
            normalTopbar.SetActive(!active);
            tileTypeSelTopbar.SetActive(active);

            selectedTileType.color = levelLoader.GetTileColor(currentType);
            tileTypeButton.color = levelLoader.GetTileColor(currentType);
        }

        public void ToggleCameraModeMenu(bool active) {
            cameraModeMenu.SetActive(active);
        }

        public void ToggleEraseMode() {
            eraseMode = !eraseMode;
        }

        //NOTE: This is an integer because the Unity Inspector does not allow enums in events
        public void SetTileType(int type) {
            this.currentType = (TileType) type;
            ToggleTileTypeSelector(false);
        }

        public void SetCameraMode(string mode) {
            CameraState state;
            switch (mode) {
                case "ISO_TILE_BOUND":
                    state = new IsoTilebound(cameraController, levelLoader);
                    break;
                case "ISO_FREE_MOVE":
                    state = new IsoFreeMove(cameraController, freeMoveSpeed);
                    break;
                // case "TOP_DOWN_TILE_BOUND":
                //     state = new TopDownTilebound();
                //     break;
                // case "TOP_DOWN_FREE_MOVE":
                //     state = new TopDownFreeMove();
                //     break;
                default:
                    return;
            }

            cameraController.SetState(state);
            ToggleCameraModeMenu(false);
        }

    }
}


