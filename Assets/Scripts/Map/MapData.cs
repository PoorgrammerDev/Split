using UnityEngine;
using Array2DEditor;

namespace Split.Map {
    
    /*
     * This class holds data for a map or stage in the game
     * to be fed into the MapGenerator
     */
    
    [CreateAssetMenu(fileName = "New Map Data", menuName = "Maps/Map Data")]
    public class MapData : ScriptableObject {
        [Range(0, 1)]
        public float tileOutlinePercent;

        public Array2DBool holeGrid;
        public Vector2Int FieldSize {
            get {
                return holeGrid.GridSize;
            }
        }

        public Vector2Int SpawnPosition;
    }
}