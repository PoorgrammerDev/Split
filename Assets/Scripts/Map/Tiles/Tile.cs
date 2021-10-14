using UnityEngine;

namespace Split.Map.Tiles {
    public class Tile {
        public Transform TileObject {get; protected set;}
        
        public Tile(Transform tilePrefab, Vector3 tilePosition, MapGenerator parent, MapData mapData) {
            this.TileObject = GameObject.Instantiate(tilePrefab, tilePosition, Quaternion.identity, parent.transform).transform;

            //Sets the width/outline accordingly
            Vector3 scale = this.TileObject.localScale;
            scale.x = 1 - mapData.tileOutlinePercent;
            scale.z = scale.x;
            this.TileObject.localScale = scale;
        }

        
    }
}