using UnityEngine;
using Split.Tiles;

namespace Split.Builder.States {
    public class Normal : BuilderState {
        private Vector2Int? fillPos;
        private GameObject currentFillPosHighlighter;
        
        public Normal(BuilderManager manager, CameraController camera, BuilderLevelLoader loader) : base(manager, camera, loader) {
        }

        /// <summary>
        /// Places a Tile at the selected spot, or completes fill operation 
        /// </summary>
        public override bool PlaceKey() {
            Vector2Int? vec = camera.GetState().GetPosition();
            if (vec.HasValue) {
                //If fill POS1 is already set - start filling
                if (fillPos.HasValue) {
                    manager.StartCoroutine(loader.Fill(fillPos.Value, vec.Value, (manager.EraseMode ? TileType.EMPTY : manager.CurrentType)));

                    manager.RemoveTileHighlighter(ref currentFillPosHighlighter);
                    fillPos = null;
                }

                //Fill POS1 not set -> Set tile individually
                else {
                    loader.SetTile(vec.Value, (manager.EraseMode ? TileType.EMPTY : manager.CurrentType));
                }
                
                return true;
            }

            return false;
        }

        
        public override bool ShiftPlaceKey() {
            Vector2Int? vec = camera.GetState().GetPosition();
            if (vec.HasValue) {
                fillPos = vec.Value;

                GameObject highlighter = (currentFillPosHighlighter != null) ? currentFillPosHighlighter : manager.GetTileHighlighter(manager.FillHighlighterColor);
                highlighter.transform.position = loader.GridToWorldPos(vec.Value.x, vec.Value.y);

                currentFillPosHighlighter = highlighter;
                return true;
            }

            return false;
        }

    }
}