using UnityEngine;

namespace Split.Tiles {
    /*
     * This class represents a special tile that detects a Player's movement
     */
     
    public class ButtonTile : TileEntity {

        public ButtonTile(GameObject gameObject, int gridX, int gridY) : base(gameObject, gridX, gridY) {
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