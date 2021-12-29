using System.Collections.Generic;
using UnityEngine;
using Split.Tiles;
using Split.LevelLoading;

namespace Split.Builder {
    public class BuilderLevelData {
        public string levelName;
        public string levelDescription;
        public string fileName;

        public Vector2Int startPosition;
        public Vector2Int endPosition;

        public List<List<TileType>> gridData;
        public List<ButtonTileData> buttonTileData;
    }
}