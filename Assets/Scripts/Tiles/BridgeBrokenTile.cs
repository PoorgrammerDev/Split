using UnityEngine;
using Split.Tiles.Properties;

namespace Split.Tiles {
    public class BridgeBrokenTile : TileEntity, IToggleable {
        private BridgeProperty bridgeProperty;
        private BrokenProperty brokenProperty;

        public BridgeBrokenTile(float deactivatedAlpha, Player.PlayerManager playerManager, LevelLoading.LevelData mapData, GameObject gameObject, int gridX, int gridY) : base(gameObject, gridX, gridY) {
            this.bridgeProperty = new BridgeProperty(deactivatedAlpha, playerManager, mapData, GridPosition, gameObject);
            this.brokenProperty = new BrokenProperty(GridPosition, gameObject);
        }

        public bool IsActive() {
            return (brokenProperty.Active && bridgeProperty.Active);
        }
    }
}