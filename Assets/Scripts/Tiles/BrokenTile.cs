using UnityEngine;
using Split.LevelLoading;

namespace Split.Tiles {
    /*
     * This class represents a tile that becomes a hole once stepped off
     */
     
    public class BrokenTile : TileEntity, IBridgeTile {
        private LevelGenerator generator;
        private bool active;

        public BrokenTile(GameObject gameObject, int gridX, int gridY) : base(gameObject, gridX, gridY) {
            this.active = true;

            GameEvents.current.playerMoveToTile += OnLeaveTile;
        }

        public bool IsActive() {
            return this.active;
        }

        private void OnLeaveTile(Vector2Int from, Vector2Int to) {
            if (from.Equals(this.GridPosition)) {
                //TODO: play breaking animation

                this.active = false;
                this.GameObject.SetActive(false);
                //TODO: add tile break event
            }
        }

     
    }
}