using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Split.Builder.CameraStates;
using Split.Tiles;
using Split.Builder.States;

namespace Split.Builder {
    /// <summary>
    /// Manages the Builder Game Scene, aside from the Menus
    /// </summary>
    public class BuilderManager : MonoBehaviour {
        [Header("Options")]
        [SerializeField] private float freeMoveSpeed;
        [SerializeField] private Color inactiveCameraColor;
        [SerializeField] private Color activeCameraColor;
        [SerializeField] private Color fillHighlighterColor;
        [SerializeField] private Color dataHighlighterColor;
        [SerializeField] private Color dataTargetsHLColor;
        [SerializeField] private Color inactiveUIButtonColor;
        [SerializeField] private Color activeUIButtonColor;
        [SerializeField] private int initialTileHighlighters;
        [SerializeField] private TileType[] dataTilesArr;
        [SerializeField] private TileType[] bridgeTilesArr;

        [Header("References")]
        [SerializeField] private CameraController cameraController;
        [SerializeField] private BuilderLevelLoader levelLoader;
        [SerializeField] private BuilderMainMenu mainMenuManager;
        [SerializeField] private GameObject tileHighlighterPrefab;

        [Header("UI")]
        [SerializeField] private GameObject builderHUD;
        [SerializeField] private GameObject dataMode;
        [SerializeField] private GameObject menu;
        
        [Header("UI - Base Topbar")]
        [SerializeField] private GameObject normalTopbar;
        [SerializeField] private Image tileTypeButton;
        [SerializeField] private Image eraserButton;
        [SerializeField] private Image visibilityButton;

        [Header("UI - Tile Type Selector")]
        [SerializeField] private GameObject tileTypeSelTopbar;
        [SerializeField] private Image selectedTileType;

        [Header("UI - Camera Mode Menu")]
        [SerializeField] private GameObject cameraModeMenu;
        [SerializeField] private Image isometricButton;
        [SerializeField] private Image topDownButton;

        private BuilderState state;
        private BuilderLevelData data;
        private bool emptyVisibility;
        private Stack<GameObject> tileHighlighters;
        private bool shiftPressed;
        private HashSet<TileType> dataTiles;
        private HashSet<TileType> bridgeTiles;


        public TileType CurrentType {get; private set;}
        public bool EraseMode {get; private set;}
        public Color FillHighlighterColor => fillHighlighterColor;
        public Color DataHighlighterColor => dataHighlighterColor;
        public Color DataTargetsHLColor => dataTargetsHLColor;
        public bool EmptyVisibility => emptyVisibility;

        private void Awake() {
            this.CurrentType = TileType.BASIC;
            this.EraseMode = false;
            this.shiftPressed = false;
            this.emptyVisibility = true;
            this.tileHighlighters = new Stack<GameObject>();
            this.dataTiles = new HashSet<TileType>(dataTilesArr);
            this.bridgeTiles = new HashSet<TileType>(bridgeTilesArr);
            SetState(new Normal(this, cameraController, levelLoader));

            for (int i = 0; i < initialTileHighlighters; ++i) {
                AddTileHighlighter();
            }
        }

        private void Start() {
            this.tileTypeButton.color = levelLoader.GetTileColor(this.CurrentType);
        }
        
        public void OpenLevel(BuilderLevelData data) {
            this.data = data;

            //Generate the level based on the data
            levelLoader.Generate(data);

            //Close main menu
            mainMenuManager.CloseAllMenus();

            //Open builder HUD
            builderHUD.SetActive(true);

            //Activate camera - set default state: IsoTilebound
            cameraController.SetState(new IsoTilebound(cameraController, levelLoader));
        }

        public BuilderState GetState() {
            return this.state;
        }

        public void SetState(BuilderState state) {
            if (this.state != null) this.state.End();

            this.state = state;
            this.state.Start();
        }

        /*************
        * INPUT KEYS *
        *************/

        public void PlaceTile(InputAction.CallbackContext context) {
            if (!context.performed) return;

            if (shiftPressed) {
                GetState().ShiftPlaceKey();
            }
            else {
                GetState().PlaceKey();
            }
            
        }

        public void ShiftDetector(InputAction.CallbackContext context) {
            if (context.performed) {
                shiftPressed = true;
            }
            else if (context.canceled) {
                shiftPressed = false;
            }
        }


        /*************
        *     UI     *
        *************/

        public void ToggleTileTypeSelector(bool active) {
            normalTopbar.SetActive(!active);
            tileTypeSelTopbar.SetActive(active);

            selectedTileType.color = levelLoader.GetTileColor(CurrentType);
            tileTypeButton.color = levelLoader.GetTileColor(EraseMode ? TileType.EMPTY : this.CurrentType);
        }

        
        public void ToggleCameraModeMenu() {
            bool active = !cameraModeMenu.activeInHierarchy;
            cameraModeMenu.SetActive(active);
            
            if (active) {
                UpdateCameraMenu(cameraController.GetState());
            }
        }

        public void ToggleMenu() {
            menu.SetActive(!menu.activeInHierarchy);
        }

        public void ToggleEraseMode() {
            EraseMode = !EraseMode;
            eraserButton.color = (EraseMode ? dataHighlighterColor : activeUIButtonColor);
            tileTypeButton.color = levelLoader.GetTileColor(EraseMode ? TileType.EMPTY : this.CurrentType);
        }

        //NOTE: This is an integer because the Unity Inspector does not allow enums in events
        public void SetTileType(int type) {
            this.CurrentType = (TileType) type;
            ToggleTileTypeSelector(false);
        }

        public void SetCameraMode(string mode) {
            CameraState state;
            switch (mode) {
                case "ISO_TILE_BOUND":
                    state = new IsoTilebound(cameraController, levelLoader);
                    break;
                case "TOP_DOWN_TILE_BOUND":
                    state = new TopDownTilebound(cameraController, levelLoader);
                    break;
                default:
                    return;
            }

            cameraController.SetState(state);
            UpdateCameraMenu(state);
            ToggleCameraModeMenu();
        }

        public void ToggleEmptyTileVisibility() {
            emptyVisibility = !emptyVisibility;
            levelLoader.SetTypeActive(TileType.EMPTY, emptyVisibility);
            visibilityButton.color = (emptyVisibility ? activeUIButtonColor : inactiveUIButtonColor);
        }

        private void UpdateCameraMenu(CameraState state) {
            isometricButton.color = inactiveCameraColor;
            topDownButton.color = inactiveCameraColor;

            if (state is IsoTilebound) {
                isometricButton.color = activeCameraColor;
            }
            else if (state is TopDownTilebound) {
                topDownButton.color = activeCameraColor;
            }
        }

        public void ExitBuilder() {
            //TODO: check for unsaved changes

            //TODO: delete all panel objects

            //set camera inactive
            cameraController.SetState(new Inactive(cameraController));

            //change UI
            builderHUD.SetActive(false);
            menu.SetActive(false);
            mainMenuManager.CloseCreateMenu();

            levelLoader.Clear();
        }

        /**************
        *  DATA MODE  *
        **************/

        public bool IsDataTile(TileType type) {
            return dataTiles.Contains(type);
        }

        public bool IsBridgeTile(TileType type) {
            return bridgeTiles.Contains(type);
        }

        public void EnterDataMode() {
            //Ensures that there is a selected tile
            Vector2Int? pos = cameraController.GetState().GetPosition();
            if (!pos.HasValue) return;

            //Ensure that the tile is a data tile
            if (IsDataTile(levelLoader.LevelData.gridData[pos.Value.x][pos.Value.y])) {
                //Set UI accordingly
                normalTopbar.SetActive(false);
                dataMode.SetActive(true);

                //Change state
                SetState(new DataModeButton(this, cameraController, levelLoader));
                GetState().SetPosition(pos.Value);
            }
        }

        public void ExitDataMode() {
            //Set UI accordingly
            normalTopbar.SetActive(true);
            dataMode.SetActive(false);

            SetState(new Normal(this, cameraController, levelLoader));
        }

        /**************
        *    OTHER    *
        **************/

        public GameObject GetTileHighlighter(Color color) {
            if (tileHighlighters.Count <= 0) {
                AddTileHighlighter();
            }

            GameObject obj = tileHighlighters.Pop();
            obj.GetComponent<Renderer>().material.color = color;
            obj.SetActive(true);
            return obj;
        }

        public void RemoveTileHighlighter(ref GameObject obj) {
            obj.SetActive(false);
            tileHighlighters.Push(obj);
            obj = null;
        }

        private void AddTileHighlighter() {
            GameObject obj = Instantiate(tileHighlighterPrefab, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);

            tileHighlighters.Push(obj);
        }

    }
}


