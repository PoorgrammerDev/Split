using UnityEngine;
using Split.LevelLoading;

namespace Split.Tiles {
    /*
     * This class represents a tile that becomes a hole once stepped off
     */
     
    public class BrokenTile : TileEntity {
        private LevelGenerator generator;

        public BrokenTile(GameObject gameObject, int gridX, int gridY) : base(gameObject, gridX, gridY) {
            GameEvents.current.playerMoveToTile += OnLeaveTile;
        }

        private void OnLeaveTile(Vector2Int from, Vector2Int to) {
            if (from.Equals(this.GridPosition)) {
                //TODO: play breaking animation

                //mapGenerator.DeleteTile(this.GridPosition.x, this.GridPosition.y);
                //TODO: add tile break event
            }
        }

     
    }
}