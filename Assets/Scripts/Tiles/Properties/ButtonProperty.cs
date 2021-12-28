using UnityEngine;

namespace Split.Tiles.Properties {
    public class ButtonProperty : ITileProperty {
        private Vector2Int position;
        private Player.PlayerManager playerManager;
        
        public ButtonProperty(Player.PlayerManager playerManager, Vector2Int position) {
            this.playerManager = playerManager;
            this.position = position;

            GameEvents.current.playerMoveToTile += OnPlayerPressButton;
        }

        private void OnPlayerPressButton(Vector2Int from, Vector2Int to) {
            //Player entering button - enabling it
            if (to.Equals(this.position)) {
                GameEvents.current.ButtonActivate(this.position);
            }

            //Player leaving button - disabling it
            else if (from.Equals(this.position) && !(to.Equals(this.position))) {
                //Check if there's a player left on there
                if (playerManager.GetPlayerAtPosition(this.position) == null) {
                    GameEvents.current.ButtonDeactivate(this.position);
                }
            }
        }
    }
}