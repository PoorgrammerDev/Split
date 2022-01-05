using UnityEngine;
using System.Collections.Generic;
using Split.Player;
using Split.LevelLoading;

namespace Split.Tiles.Properties {
    /// <summary>
    /// Property allows this tile to be deactivated and reactivated by a different "Button" tile
    /// </summary>
    public class BridgeProperty : ITileProperty {
        private Vector2Int position;
        private float deactivatedAlpha;
        private LevelData mapData;
        private PlayerManager playerManager;
        private GameObject gameObject;
        private List<ButtonTileData> activators;

        public bool Active {get; private set;}

        public BridgeProperty(float deactivatedAlpha, PlayerManager playerManager, LevelData mapData, Vector2Int position, GameObject gameObject) {
            this.gameObject = gameObject;
            this.mapData = mapData;
            this.position = position;
            this.deactivatedAlpha = deactivatedAlpha;
            this.playerManager = playerManager;
            this.Active = false;

            this.activators = new List<ButtonTileData>();
            PopulateActivators();

            //Subscribe activate and deactivate
            GameEvents.current.onButtonActivate += OnButtonActivate;
            GameEvents.current.onButtonDeactivate += OnButtonDeactivate;
        }


        private void PopulateActivators() {
            foreach (ButtonTileData button in mapData.buttonTileData) {
                if (button == null) continue;
                
                foreach (Vector2Int target in button.bridgeTiles) {
                    if (target == this.position) {
                        this.activators.Add(button);
                        break;
                    }
                }
            }
        }

        private void SetActive(bool enabled) {
            this.Active = enabled;

            LeanTween.alpha(this.gameObject, (enabled ? 1.0f : deactivatedAlpha), 0.25f);
        }
        
        private void OnButtonActivate(Vector2Int buttonPosition) {
            ButtonTileData buttonTileData = GetButton(buttonPosition);

            if (buttonTileData != null) {
                SetActive(true);
                GameEvents.current.BridgeActivate(this.position); //fire bridge activation event
            }
        }

        private void OnButtonDeactivate(Vector2Int buttonPosition) {
            ButtonTileData buttonTileData = GetButton(buttonPosition);

            //check if other buttons are still activating this tile
            if (buttonTileData != null && !OtherButtonsPopulated(buttonTileData)) {
                SetActive(false);
                GameEvents.current.BridgeDeactivate(this.position); //fire bridge deactivate event
            }
        }


        private ButtonTileData GetButton(Vector2Int position) {
            foreach (ButtonTileData button in this.activators) {
                if (button.tilePosition == position) {
                    return button;
                }
            }

            return null;
        }

        private bool OtherButtonsPopulated(ButtonTileData tileData) {
            foreach (ButtonTileData button in this.activators) {
                if (button != tileData) {
                    if (playerManager.GetPlayerAtPosition(button.tilePosition) != null) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}