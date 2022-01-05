using UnityEngine;
using Split.Player;

namespace Split.Tiles {
    /// <summary>
    /// Tile that holds the button property (activates Bridge tiles once stepped on)
    /// </summary>
    public class ButtonTile : TileEntity {
        private Properties.ButtonProperty buttonProperty;

        public ButtonTile(GameObject gameObject, PlayerManager playerManager, int gridX, int gridY) : base(gameObject, gridX, gridY) {
            this.buttonProperty = new Properties.ButtonProperty(playerManager, GridPosition);
        }

    }
}