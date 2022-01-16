using System.Collections.Generic;
using UnityEngine;

namespace Split.Builder.States {
    public class DataModeButton : BuilderState {
        private Vector2Int dataPanel;
        private GameObject dataTileHighlighter;
        private Dictionary<Vector2Int, GameObject> targetHiglighters;
        private BuilderButtonTileData tileData;

        public DataModeButton(BuilderManager manager, CameraController camera, BuilderLevelLoader loader) : base(manager, camera, loader) {
            this.targetHiglighters = new Dictionary<Vector2Int, GameObject>();
        }

        public override void Start() {
            dataTileHighlighter = manager.GetTileHighlighter(manager.DataHighlighterColor);
        }

        public override void End() {
            manager.RemoveTileHighlighter(ref dataTileHighlighter);

            foreach (GameObject obj in targetHiglighters.Values) {
                GameObject refObj = obj;
                manager.RemoveTileHighlighter(ref refObj);
            }
        }

        public override void SetPosition(Vector2Int pos) {
            //Ensures that selected tile holds data
            if (manager.IsDataTile(loader.LevelData.gridData[pos.x][pos.y])) {
                //Set position and move highlighter
                dataPanel = pos;
                dataTileHighlighter.transform.position = loader.GridToWorldPos(pos.x, pos.y);

                //Get Button Tile Data
                tileData = GetButtonTileData();
                CreateMarkers();
            }
        }

        /// <summary>
        /// Toggles a bridge to be part of the selected button tile's targets
        /// OR toggles an entire selection of them
        /// </summary>
        public override bool PlaceKey() {
            Vector2Int? pos = camera.GetState().GetPosition();
            GameObject highlighter;
            
            //GUARD CLAUSES --------
            //Position is valid
            if (!pos.HasValue) return false;
            
            //Not the original data tile (button tile cannot activate/deactivate itself)
            if (pos.Value == dataPanel) return false;

            //Tile is a bridge tile
            if (!manager.IsBridgeTile(loader.LevelData.gridData[pos.Value.x][pos.Value.y])) return false;
            //---------

            //Already added -> remove
            if (tileData.bridgeTiles.Contains(pos.Value)) {
                //Remove from data
                tileData.bridgeTiles.Remove(pos.Value);
                //Remove tile highlighter
                highlighter = targetHiglighters[pos.Value];
                manager.RemoveTileHighlighter(ref highlighter);
                targetHiglighters.Remove(pos.Value);
            }
            
            //Add new tile
            else {
                tileData.bridgeTiles.Add(pos.Value);
                
                highlighter = manager.GetTileHighlighter(manager.DataTargetsHLColor);
                highlighter.transform.position = loader.GridToWorldPos(pos.Value.x, pos.Value.y);
                targetHiglighters[pos.Value] = highlighter;
            }

            return true;
        }

        /// <summary>
        /// Sets first position for the selection fill
        /// </summary>
        public override bool ShiftPlaceKey() {
            return false;
        }

        private BuilderButtonTileData GetButtonTileData() {
            //Searches level data for existing data
            foreach (BuilderButtonTileData existingData in loader.LevelData.buttonTileData) {
                if (existingData.tilePosition == dataPanel) {
                    return existingData;
                }
            }

            //Makes new data and adds it to the list
            BuilderButtonTileData newData = new BuilderButtonTileData();
            newData.tilePosition = dataPanel;
            loader.LevelData.buttonTileData.Add(newData);
            return newData;
        }

        private void CreateMarkers() {
            foreach (Vector2Int pos in tileData.bridgeTiles) {
                targetHiglighters[pos] = manager.GetTileHighlighter(manager.DataTargetsHLColor);
                targetHiglighters[pos].transform.position = loader.GridToWorldPos(pos.x, pos.y);
            }
        }

    }
}