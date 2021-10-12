using UnityEngine;

namespace Split.Map {

    /*
     * This class builds a stage of the game out of tiles, 
     * given the MapData serializable object
     */

    public class MapGenerator : MonoBehaviour {
        [SerializeField] private Transform tilePrefab;
        [SerializeField] private MapData mapData;
        private Transform[,] grid;

        // Start is called before the first frame update
        void Start() {
            grid = new Transform[mapData.FieldSize.x, mapData.FieldSize.y];

            GenerateMap();
        }

        //TODO: Replace all of this with a more efficient mesh generator 
        [ContextMenu("Generate Map")]
        public void GenerateMap() {
            ClearMap();

            for (int x = 0; x < mapData.FieldSize.x; ++x) {
                for (int y = 0; y < mapData.FieldSize.y; ++y) {
                    if (mapData.holeGrid.GetCell(x, y)) continue;

                    //Calculates the position of the tile to be created, and then instantiates it
                    Vector3 tilePosition = new Vector3(-mapData.FieldSize.y / 2 + 0.5f + y, -tilePrefab.localScale.y / 2, -mapData.FieldSize.x / 2 + 0.5f + x);
                    Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity, this.transform).transform;

                    //Sets the width/outline accordingly
                    Vector3 scale = newTile.localScale;
                    scale.x = 1 - mapData.tileOutlinePercent;
                    scale.z = scale.x;
                    newTile.localScale = scale;

                    grid[x, y] = newTile;
                }
            }
        }

        [ContextMenu("Clear Map")]
        private void ClearMap() {
            //Iterates through grid and destroys each tile
            for (int x = 0; x < grid.GetLength(0); ++x) {
                for (int y = 0; y < grid.GetLength(1); ++y) {
                    if (grid[x, y] != null) {
                        Destroy(grid[x, y].gameObject);
                        grid[x, y] = null;
                    }
                }
            }
        }
    }
}