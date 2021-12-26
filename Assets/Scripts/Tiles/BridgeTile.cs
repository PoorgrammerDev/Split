using System.Collections;
using UnityEngine;
using Split.LevelLoading;

namespace Split.Tiles {

    /*
     * This class represents a special tile that acts as a Hole when deactivated
     * and as a normal tile when activated
     */
     
    public class BridgeTile : TileEntity {
        private float deactivatedAlpha;
        private LevelData mapData;
        public bool Activated {get; protected set;}

        public BridgeTile(float deactivatedAlpha, LevelData mapData, GameObject gameObject, int gridX, int gridY) : base(gameObject, gridX, gridY) {
            this.mapData = mapData;
            this.deactivatedAlpha = deactivatedAlpha;
            this.Activated = false;

            //Subscribe activate and deactivate
            GameEvents.current.onButtonActivate += OnButtonActivate;
            GameEvents.current.onButtonDeactivate += OnButtonDeactivate;
        }
        
        //Step on button - activate tiles
        private void OnButtonActivate(Vector2Int buttonPosition) {
            // ButtonTileData buttonTileData = mapData.GetButtonByCoord(buttonPosition);

            // if (buttonTileData != null && buttonTileData.activateBridgeTiles && buttonTileData.ContainsBridgeTile(this.GridPosition)) {
            //     TileActivate(true);
            // }
        }

        //Step off button - deactivate tiles
        private void OnButtonDeactivate(Vector2Int buttonPosition) {
            // ButtonTileData buttonTileData = mapData.GetButtonByCoord(buttonPosition);

            // if (buttonTileData != null && buttonTileData.activateBridgeTiles && buttonTileData.ContainsBridgeTile(this.GridPosition)) {
            //     TileActivate(false);
            // }
        }

        private void TileActivate(bool enabled) {
            // Activated = enabled;

            // LeanTween.alpha(this.TileObject.gameObject, (enabled ? 1.0f : deactivatedAlpha), 0.25f);
        }

    }
}