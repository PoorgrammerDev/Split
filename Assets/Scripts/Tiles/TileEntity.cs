using UnityEngine;

namespace Split.Tiles {
    public abstract class TileEntity : Tile {
        public Vector2Int GridPosition {get; protected set;}
        public GameObject GameObject {get; protected set;}
        
        public TileEntity(GameObject gameObject, int gridX, int gridY) {
            this.GridPosition = new Vector2Int(gridX, gridY);
            this.GameObject = gameObject;
        }
    }
}