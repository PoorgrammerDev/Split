using UnityEngine;
using Split.Tiles;

namespace Split.LevelLoading {
    
    /*
     * This class holds data for a map or stage in the game
     * to be fed into the MapGenerator
     */
    
    public class LevelData {
        public TileType[,] gridData;
        public ButtonTileData[] buttonTileData;
    }

    [System.Serializable]
    public class ButtonTileData {
        public Vector2Int tilePosition;
        public Vector2Int[] bridgeTiles;
    }
}