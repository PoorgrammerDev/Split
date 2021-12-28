using UnityEngine;
using Split.Player;

namespace Split.Tiles {
    /*
     * This class represents a special tile that detects a Player's movement
     */
     
    public class ButtonTile : TileEntity {
        private Properties.ButtonProperty buttonProperty;

        public ButtonTile(GameObject gameObject, PlayerManager playerManager, int gridX, int gridY) : base(gameObject, gridX, gridY) {
            this.buttonProperty = new Properties.ButtonProperty(playerManager, GridPosition);
        }

    }
}