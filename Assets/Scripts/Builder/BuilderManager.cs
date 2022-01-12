using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Split.Builder.CameraStates;
using Split.Tiles;

namespace Split.Builder {
    /// <summary>
    /// Manages the Builder Game Scene, aside from the Menus
    /// </summary>
    public class BuilderManager : MonoBehaviour {
        [Header("Options")]
        [SerializeField] private float freeMoveSpeed;
        [SerializeField] private Color inactiveCameraColor;
        [SerializeField] private Color activeCameraColor;
        [SerializeField] private int initialTileHighlighters;

        [Header("References")]
        [SerializeField] private CameraController cameraController;
        [SerializeField] private BuilderLevelLoader levelLoader;
        [SerializeField] private BuilderMainMenu mainMenuManager;
        [SerializeField] private GameObject tileHighlighterPrefab;

        [Header("UI")]
        [SerializeField] private GameObject builderHUD;
        

        [Header("UI - Base Topbar")]
        [SerializeField] private GameObject normalTopbar;
        [SerializeField] private Image tileTypeButton;

        [Header("UI - Tile Type Selector")]
        [SerializeField] private GameObject tileTypeSelTopbar;
        [SerializeField] private Image selectedTileType;

        [Header("UI - Camera Mode Menu")]
        [SerializeField] private GameObject cameraModeMenu;
        [SerializeField] private Image isoTileboundButton;
        [SerializeField] private Image isoFreeMoveButton;
        [SerializeField] private Image tdTileboundButton;
        [SerializeField] private Image tdFreeMoveButton;

        [Header("UI - Data Mode")]
        [SerializeField] private GameObject dataModeTopbar;

        private BuilderLevelData data;
        private TileType currentType;
        private bool eraseMode;
        private bool emptyVisibility;
        private GameObject currentFillPosHL;
        private Vector2Int? fillPos1;
        private Stack<GameObject> tileHighlighters;
        

        private void Awake() {
            this.currentType = TileType.BASIC;
            this.eraseMode = false;
            this.tileHighlighters = new Stack<GameObject>();

            for (int i = 0; i < initialTileHighlighters; ++i) {
                AddTileHighlighter();
            }
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
        * INPUT KEYS *
        *************/

        public void SelectFillPosition(InputAction.CallbackContext context) {
            if (!context.performed) return;

            Vector2Int? vec = cameraController.GetState().GetPosition();
            if (vec.HasValue) {
                fillPos1 = vec.Value;

                GameObject highlighter = (currentFillPosHL != null) ? currentFillPosHL : GetTileHighlighter();
                highlighter.transform.position = levelLoader.GridToWorldPos(vec.Value.x, vec.Value.y);
                highlighter.SetActive(true);

                currentFillPosHL = highlighter;
            }
        }

        public void PlaceTile(InputAction.CallbackContext context) {
            if (!context.performed) return;

            Vector2Int? vec = cameraController.GetState().GetPosition();
            if (vec.HasValue) {
                if (fillPos1.HasValue) {
                    StartCoroutine(levelLoader.Fill(fillPos1.Value, vec.Value, (eraseMode ? TileType.EMPTY : currentType)));

                    currentFillPosHL.SetActive(false);
                    tileHighlighters.Push(currentFillPosHL);
                    currentFillPosHL = null;
                    fillPos1 = null;
                }
                else {
                    levelLoader.SetTile(vec.Value, (eraseMode ? TileType.EMPTY : currentType));
                }
            }
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

        
        public void ToggleCameraModeMenu() {
            bool active = !cameraModeMenu.activeInHierarchy;
            cameraModeMenu.SetActive(active);
            
            if (active) {
                UpdateCameraMenu(cameraController.GetState());
            }
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
            UpdateCameraMenu(state);
        }

        public void ToggleEmptyTileVisibility() {
            emptyVisibility = !emptyVisibility;
            levelLoader.SetTypeActive(TileType.EMPTY, emptyVisibility);
        }

        //TODO: kinda messy
        private void UpdateCameraMenu(CameraState state) {
            isoTileboundButton.color = inactiveCameraColor;
            isoFreeMoveButton.color = inactiveCameraColor;
            tdTileboundButton.color = inactiveCameraColor;
            tdFreeMoveButton.color = inactiveCameraColor;

            if (state is IsoTilebound) {
                isoTileboundButton.color = activeCameraColor;
            }
            else if (state is IsoFreeMove) {
                isoFreeMoveButton.color = activeCameraColor;
            }
            // else if (state is TopDownTilebound) {

            // }
            // else if (state is TopDownFreeMove) {

            // }
        }


        /**************
        *    OTHER    *
        **************/

        private GameObject GetTileHighlighter() {
            if (tileHighlighters.Count > 0) {
                return tileHighlighters.Pop();
            }
            else {
                AddTileHighlighter();
                return GetTileHighlighter();
            }
        }

        private void AddTileHighlighter() {
            GameObject obj = Instantiate(tileHighlighterPrefab, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);

            tileHighlighters.Push(obj);
        }

    }
}


