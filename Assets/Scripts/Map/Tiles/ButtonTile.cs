using UnityEngine;

namespace Split.Map.Tiles {
    /*
     * This class represents a special tile that detects a Player's movement
     */
     
    public class ButtonTile : Tile {

        public ButtonTile(Transform tilePrefab, Vector3 tilePosition, MapGenerator parent, MapData mapData, int gridX, int gridY) : base(tilePrefab, tilePosition, parent, mapData, gridX, gridY) {
            GameEvents.current.playerMoveToTile += OnPlayerPressButton;
        }

        private void OnPlayerPressButton(Vector2Int from, Vector2Int to) {
            if (to.Equals(this.GridPosition)) {
                GameEvents.current.ButtonActivate(this.GridPosition);
            }
            else if (from.Equals(this.GridPosition) && !(to.Equals(this.GridPosition))) {
                GameEvents.current.ButtonDeactivate(this.GridPosition);
            }
        }

    }
}