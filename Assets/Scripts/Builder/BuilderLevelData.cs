using System.Collections.Generic;
using UnityEngine;
using Split.Tiles;
using Split.LevelLoading;

namespace Split.Builder {
    /// <summary>
    /// Representation of LevelData for the Builder.
    /// Notable differences from LevelData include the usage of List<> instead of array[].
    /// This is due to the fact that the level can expand in the builder.
    /// </summary>
    public class BuilderLevelData {
        public string levelName;
        public string levelDescription;
        public string fileName;

        public Vector2Int startPosition;
        public Vector2Int endPosition;

        public List<List<TileType>> gridData;
        public List<ButtonTileData> buttonTileData;

        public int maxPlayers;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public BuilderLevelData() {
            this.gridData = new List<List<TileType>>();
            this.buttonTileData = new List<ButtonTileData>();
        }

        /// <summary>
        /// Constructor that converts regular LevelData to BuilderLevelData
        /// </summary>
        /// <param name="levelData">Regular Level Data</param>
        /// <param name="fileName">File Name: additional data not included</param>
        public BuilderLevelData(LevelData levelData, string fileName) {
            //All the data that is directly transferrable is assigned
            this.levelName = levelData.levelName;
            this.levelDescription = levelData.levelDescription;
            this.fileName = fileName;
            this.startPosition = levelData.startPosition;
            this.endPosition = levelData.endPosition;
            this.maxPlayers = levelData.maxPlayers;

            //Uses List<> ctor to convert 1D arr[] to 1D List<>
            this.buttonTileData = new List<ButtonTileData>(levelData.buttonTileData);

            //Converts TileGrid (2D arr[]) to 2D List<>
            this.gridData = new List<List<TileType>>(levelData.gridData.x);
            for (int x = 0; x < levelData.gridData.x; ++x) {
                this.gridData.Add(new List<TileType>(levelData.gridData.y));

                for (int y = 0; y < levelData.gridData.y; ++y) {
                    this.gridData[x].Add(levelData.gridData[x, y]);
                }
            }
            
        }

        public LevelData ToLevelData() {
            LevelData output = new LevelData();

            //All the data that is directly transferrable is assigned
            output.levelName = this.levelName;
            output.levelDescription = this.levelDescription;
            output.startPosition = this.startPosition;
            output.endPosition = this.endPosition;
            output.maxPlayers = this.maxPlayers;

            output.buttonTileData = this.buttonTileData.ToArray();

            //Converts 2D List<> to 2D array[]
            output.gridData = new TileGrid(this.gridData.Count, this.gridData[0].Count);
            for (int x = 0; x < this.gridData.Count; ++x) {
                for (int y = 0; y < this.gridData[0].Count; ++y) {
                    output.gridData[x, y] = this.gridData[x][y];
                }
            }
            
            return output;
        }
    }


}