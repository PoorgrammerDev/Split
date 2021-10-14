using UnityEngine;
using Array2DEditor;

namespace Split.Map {
    
    /*
     * This class holds data for a map or stage in the game
     * to be fed into the MapGenerator
     */
    
    [CreateAssetMenu(fileName = "New Map Data", menuName = "Maps/Map Data")]
    public class MapData : ScriptableObject {
        /**********************
         * INSPECTOR VIEWABLE *
         **********************/
        [Range(0, 1)]
        public float tileOutlinePercent;

        public Array2DBool holeGrid;
        public Vector2Int SpawnPosition;
        public ButtonTileData[] buttonTileData;


        /*********************
         *       OTHER       *
         *********************/
        public Vector2Int FieldSize {
            get {
                return holeGrid.GridSize;
            }
        }

        //TODO: Sequential search - perhaps could be improved?
        public ButtonTileData GetButtonByCoord(Vector2Int coord) {
            foreach (ButtonTileData tile in this.buttonTileData) {
                if (tile.buttonPosition.Equals(coord)) {
                    return tile;
                }
            }
            return null;
        }
    }

    [System.Serializable]
    public class ButtonTileData {
        public Vector2Int buttonPosition;

        public bool activateBridgeTiles;
        public Vector2Int[] bridgeTiles;

        //TODO: Sequential search - perhaps could be improved?
        public bool ContainsBridgeTile(Vector2Int target) {
            foreach (Vector2Int tile in this.bridgeTiles) {
                if (tile.Equals(target)) {
                    return true;
                }
            }
            return false;
        }
    }
}