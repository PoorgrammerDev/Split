using UnityEngine;
using Split.Map.Tiles;

namespace Split.Map {

    /*
     * This class builds a stage of the game out of tiles, 
     * given the MapData serializable object
     */

    public class MapGenerator : MonoBehaviour {
        [SerializeField] private Transform tilePrefab;
        [SerializeField] private MapData mapData; //TODO: Maybe replace this with a manager class?

        public Tile[,] Grid {get; private set;}

        void Awake() {
            Grid = new Tile[mapData.FieldSize.x, mapData.FieldSize.y];

            GenerateMap();
        }

        //TODO: Replace all of this with a more efficient mesh generator 
        public void GenerateMap() {
            ClearMap();

            for (int x = 0; x < mapData.FieldSize.x; ++x) {
                for (int y = 0; y < mapData.FieldSize.y; ++y) {
                    if (mapData.holeGrid.GetCell(x, y)) continue;

                    //Calculates the position of the tile to be created, and then instantiates it
                    Vector3 tilePosition = new Vector3(-mapData.FieldSize.y / 2 + 0.5f + y, -tilePrefab.localScale.y / 2, -mapData.FieldSize.x / 2 + 0.5f + x);
                    Tile newTile = new Tile(tilePrefab, tilePosition, this, mapData);

                    Grid[x, y] = newTile;
                }
            }
        }

        public void ClearMap() {
            //Iterates through grid and destroys each tile
            for (int x = 0; x < Grid.GetLength(0); ++x) {
                for (int y = 0; y < Grid.GetLength(1); ++y) {
                    if (Grid[x, y] != null) {
                        Destroy(Grid[x, y].TileObject.gameObject);
                        Grid[x, y] = null;
                    }
                }
            }
        }



    }
}