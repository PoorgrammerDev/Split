using UnityEngine;

namespace Split.Tiles {
    public class BridgeButtonTile : TileEntity, IBridgeTile {
        private bool active;

        public BridgeButtonTile(GameObject gameObject, int gridX, int gridY) : base(gameObject, gridX, gridY) {
            active = false;
        }

        public bool IsActive() {
            return active;
        }


        //TODO: Unimplemented class
    }
}