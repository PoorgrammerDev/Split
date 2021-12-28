using UnityEngine;
using Split.Tiles.Properties;
using Split.LevelLoading;
using Split.Player;

namespace Split.Tiles {

    /*
     * This class represents a special tile that acts as a Hole when deactivated
     * and as a normal tile when activated
     */
     
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