using UnityEngine;
using Split.Player;

namespace Split.Tiles {
    /*
     * This class represents a special tile that detects a Player's movement
     */
     
    public class ButtonTile : TileEntity {
        PlayerManager playerManager;

        public ButtonTile(GameObject gameObject, PlayerManager playerManager, int gridX, int gridY) : base(gameObject, gridX, gridY) {
            this.playerManager = playerManager;

            GameEvents.current.playerMoveToTile += OnPlayerPressButton;
        }

        private void OnPlayerPressButton(Vector2Int from, Vector2Int to) {
            //Player entering button - enabling it
            if (to.Equals(this.GridPosition)) {
                GameEvents.current.ButtonActivate(this.GridPosition);
            }

            //Player leaving button - disabling it
            else if (from.Equals(this.GridPosition) && !(to.Equals(this.GridPosition))) {
                //Check if there's a player left on there
                if (playerManager.GetPlayerAtPosition(this.GridPosition) == null) {
                    GameEvents.current.ButtonDeactivate(this.GridPosition);
                }
            }
        }

    }
}