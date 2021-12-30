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

        public int maxPlayers;

        public BuilderLevelData() {}

        public BuilderLevelData(LevelData levelData, string fileName) {
            this.levelName = levelData.levelName;
            this.levelDescription = levelData.levelDescription;
            this.fileName = fileName;

            this.startPosition = levelData.startPosition;
            this.endPosition = levelData.endPosition;

            this.gridData = new List<List<TileType>>(levelData.gridData.x);
            for (int x = 0; x < levelData.gridData.x; ++x) {
                this.gridData.Add(new List<TileType>(levelData.gridData.y));

                for (int y = 0; y < levelData.gridData.y; ++y) {
                    this.gridData[x].Add(levelData.gridData[x, y]);
                }
            }

            this.buttonTileData = new List<ButtonTileData>(levelData.buttonTileData);
            this.maxPlayers = levelData.maxPlayers;
        }
    }
}