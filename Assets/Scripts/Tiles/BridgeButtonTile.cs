using UnityEngine;
using Split.Tiles.Properties;

namespace Split.Tiles {
    /// <summary>
    /// Tile that holds both Bridge and Button properties
    /// </summary>
    public class BridgeButtonTile : TileEntity, IToggleable {
        private BridgeProperty bridgeProperty;
        private ButtonProperty buttonProperty;

        public BridgeButtonTile(float deactivatedAlpha, Player.PlayerManager playerManager, LevelLoading.LevelData mapData, GameObject gameObject, int gridX, int gridY) : base(gameObject, gridX, gridY) {
            this.bridgeProperty = new BridgeProperty(deactivatedAlpha, playerManager, mapData, GridPosition, gameObject);
            this.buttonProperty = new ButtonProperty(playerManager, GridPosition);
        }

        public bool IsActive() {
            return bridgeProperty.Active;
        }
    }
}