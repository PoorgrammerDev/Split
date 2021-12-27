using UnityEngine;

namespace Split.Tiles {
    public class BridgeBrokenTile : TileEntity, IBridgeTile {
        private bool active;

        public BridgeBrokenTile(GameObject gameObject, int gridX, int gridY) : base(gameObject, gridX, gridY) {
            active = false;
        }

        public bool IsActive() {
            return active;
        }

        //TODO: Unimplemented class
    }
}