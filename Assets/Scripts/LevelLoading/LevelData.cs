using UnityEngine;
using Split.Tiles;

namespace Split.LevelLoading {
    
    /*
     * This class holds data for a map or stage in the game
     * to be fed into the MapGenerator
     */
    
    [System.Serializable]
    public class LevelData {
        public string levelName;
        public string levelDescription;

        public Vector2Int startPosition;
        public Vector2Int endPosition;

        public TileGrid gridData;
        public ButtonTileData[] buttonTileData;

        public int maxPlayers;
    }

    [System.Serializable]
    public class ButtonTileData {
        public Vector2Int tilePosition;
        public Vector2Int[] bridgeTiles;
    }

    [System.Serializable]
    public struct TileGrid {
        public int x, y;
        public TileTypeList[] rows;

        public TileGrid(int x, int y) {
            this.x = x;
            this.y = y;

            rows = new TileTypeList[x];

            for (int i = 0; i < x; ++i) {
                rows[i] = new TileTypeList(y);
            }
        }

        public TileType this[int x, int y] {
            get {
                return rows[x].cols[y];
            }
            set {
                rows[x].cols[y] = value;
            }
        }
    }

    [System.Serializable]
    public struct TileTypeList {
        public TileType[] cols;

        public TileTypeList(int y) {
            cols = new TileType[y];
        }

    }
}