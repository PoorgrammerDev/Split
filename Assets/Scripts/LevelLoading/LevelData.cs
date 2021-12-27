using UnityEngine;
using Split.Tiles;

namespace Split.LevelLoading {
    
    /*
     * This class holds data for a map or stage in the game
     * to be fed into the MapGenerator
     */
    
    public class LevelData {
        public Vector2Int startPosition;
        public Vector2Int endPosition;
        public TileType[,] gridData;
        public ButtonTileData[] buttonTileData;
    }

    [System.Serializable]
    public class ButtonTileData {
        public Vector2Int tilePosition;
        public Vector2Int[] bridgeTiles;
    }
}