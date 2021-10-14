using System.Collections;
using UnityEngine;

namespace Split.Map.Tiles {

    /*
     * This class represents a special tile that acts as a Hole when deactivated
     * and as a normal tile when activated
     */
     
    public class BridgeTile : Tile {
        private float deactivatedAlpha;
        private MapData mapData;
        public bool Activated {get; protected set;}

        public BridgeTile(Transform tilePrefab, Vector3 tilePosition, MapGenerator parent, MapData mapData, int gridX, int gridY) : base(tilePrefab, tilePosition, parent, mapData, gridX, gridY) {
            this.mapData = mapData;
            this.deactivatedAlpha = tilePrefab.GetComponent<Renderer>().sharedMaterial.color.a;
            this.Activated = false;

            //Subscribe activate and deactivate
            GameEvents.current.onButtonActivate += OnButtonActivate;
            GameEvents.current.onButtonDeactivate += OnButtonDeactivate;
        }
        
        //Step on button - activate tiles
        private void OnButtonActivate(Vector2Int buttonPosition) {
            ButtonTileData buttonTileData = mapData.GetButtonByCoord(buttonPosition);

            if (buttonTileData != null && buttonTileData.activateBridgeTiles && buttonTileData.ContainsBridgeTile(this.GridPosition)) {
                TileActivate(true);
            }
        }

        //Step off button - deactivate tiles
        private void OnButtonDeactivate(Vector2Int buttonPosition) {
            ButtonTileData buttonTileData = mapData.GetButtonByCoord(buttonPosition);

            if (buttonTileData != null && buttonTileData.activateBridgeTiles && buttonTileData.ContainsBridgeTile(this.GridPosition)) {
                TileActivate(false);
            }
        }

        private void TileActivate(bool enabled) {
            Activated = enabled;

            LeanTween.alpha(this.TileObject.gameObject, (enabled ? 1.0f : deactivatedAlpha), 0.25f);
        }

    }
}