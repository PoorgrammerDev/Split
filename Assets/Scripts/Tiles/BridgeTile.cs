using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Split.LevelLoading;

namespace Split.Tiles {

    /*
     * This class represents a special tile that acts as a Hole when deactivated
     * and as a normal tile when activated
     */
     
    public class BridgeTile : TileEntity, IBridgeTile {
        private float deactivatedAlpha;
        private LevelData mapData;
        private bool active;
        private List<ButtonTileData> activators;

        public BridgeTile(float deactivatedAlpha, LevelData mapData, GameObject gameObject, int gridX, int gridY) : base(gameObject, gridX, gridY) {
            this.mapData = mapData;
            this.deactivatedAlpha = deactivatedAlpha;
            this.active = false;

            this.activators = new List<ButtonTileData>();
            PopulateActivators();

            //Subscribe activate and deactivate
            GameEvents.current.onButtonActivate += OnButtonActivate;
            GameEvents.current.onButtonDeactivate += OnButtonDeactivate;
        }

        public bool IsActive() {
            return this.active;
        }

        private void SetActive(bool enabled) {
            this.active = enabled;

            LeanTween.alpha(this.GameObject.gameObject, (enabled ? 1.0f : deactivatedAlpha), 0.25f);
        }
        
        private void OnButtonActivate(Vector2Int buttonPosition) {
            ButtonTileData buttonTileData = GetButtonByCoord(buttonPosition);

            if (buttonTileData != null) {
                SetActive(true);
            }
        }

        private void OnButtonDeactivate(Vector2Int buttonPosition) {
            ButtonTileData buttonTileData = GetButtonByCoord(buttonPosition);

            if (buttonTileData != null) {
                SetActive(false);
                GameEvents.current.BridgeDeactivate(GridPosition); //fire bridge deactivate event
            }
        }

        private void PopulateActivators() {
            foreach (ButtonTileData button in mapData.buttonTileData) {
                if (button == null) continue;
                
                foreach (Vector2Int target in button.bridgeTiles) {
                    if (target == GridPosition) {
                        this.activators.Add(button);
                        break; //TODO: not sure if this works
                    }
                }
            }
        }

        private ButtonTileData GetButtonByCoord(Vector2Int position) {
            foreach (ButtonTileData button in this.activators) {
                if (button.tilePosition == position) {
                    return button;
                }
            }

            return null;
        }

    }
}