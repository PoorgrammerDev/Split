using UnityEngine;
using Split.Tiles.Properties;
using Split.LevelLoading;
using Split.Player;

namespace Split.Tiles {
    /// <summary>
    /// Tile that holds the Bridge Property (Activated/Deactivated by button)
    /// </summary>
    public class BridgeTile : TileEntity, IToggleable {
        private BridgeProperty bridgeProperty;

        public BridgeTile(float deactivatedAlpha, PlayerManager playerManager, LevelData mapData, GameObject gameObject, int gridX, int gridY) : base(gameObject, gridX, gridY) {
            this.bridgeProperty = new BridgeProperty(deactivatedAlpha, playerManager, mapData, GridPosition, gameObject);
        }

        public bool IsActive() {
            return bridgeProperty.Active;
        }

    }
}