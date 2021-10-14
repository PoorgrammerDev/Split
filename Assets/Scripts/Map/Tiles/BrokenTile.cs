using UnityEngine;

namespace Split.Map.Tiles {
    /*
     * This class represents a tile that becomes a hole once stepped off
     */
     
    public class BrokenTile : Tile {
        private MapGenerator mapGenerator;

        public BrokenTile(Transform tilePrefab, Vector3 tilePosition, MapGenerator parent, MapData mapData, int gridX, int gridY) : base(tilePrefab, tilePosition, parent, mapData, gridX, gridY) {
            this.mapGenerator = parent;

            GameEvents.current.playerMoveToTile += OnLeaveTile;
        }

        private void OnLeaveTile(Vector2Int from, Vector2Int to) {
            if (from.Equals(this.GridPosition)) {
                //TODO: play breaking animation

                mapGenerator.DeleteTile(this.GridPosition.x, this.GridPosition.y);
            }
        }

     
    }
}