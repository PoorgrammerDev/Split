using UnityEngine;

namespace Split.Map.Tiles {
    
    /*
     * This class represents a basic tile in the map where the player can move
     */

    public class Tile {
        public Vector2Int GridPosition {get; protected set;}
        public Transform TileObject {get; protected set;}
        
        public Tile(Transform tilePrefab, Vector3 tilePosition, MapGenerator parent, MapData mapData, int gridX, int gridY) {
            //Instantiates Tile GameObject
            this.TileObject = GameObject.Instantiate(tilePrefab, tilePosition, Quaternion.identity, parent.transform).transform;

            //Sets the width/outline accordingly
            Vector3 scale = this.TileObject.localScale;
            scale.x = 1 - mapData.tileOutlinePercent;
            scale.z = scale.x;
            this.TileObject.localScale = scale;

            //Sets the appropriate grid position
            GridPosition = new Vector2Int(gridX, gridY);
        }
        
    }
}