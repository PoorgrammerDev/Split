using System.Collections.Generic;
using UnityEngine;
using Split.Map.Tiles;

namespace Split.Map {

    /*
     * This class builds a stage of the game out of tiles, 
     * given the MapData serializable object
     */

    public class MapGenerator : MonoBehaviour {
        [Header("References")]
        [SerializeField] private MapData mapData; //TODO: Maybe replace this with a manager class?
        [SerializeField] private Transform regularTile;
        [SerializeField] private Transform buttonTile;
        [SerializeField] private Transform bridgeTile;
        [SerializeField] private Transform brokenTile;

        public Tile[,] Grid {get; private set;}

        void Awake() {
            Grid = new Tile[mapData.FieldSize.x, mapData.FieldSize.y];
    
            GenerateMap();
        }

        public void GenerateMap() {
            HashSet<Vector2Int> specialTiles = new HashSet<Vector2Int>();
            ClearMap();

            //------------------------------------
            // Create Data-holding Tiles first
            //------------------------------------

            //Button and Bridge Tiles
            foreach(ButtonTileData button in mapData.buttonTileData) {
                //Creates the Button tile
                Vector2Int position = button.buttonPosition;
                Grid[position.x, position.y] = CreateTile(TileType.BUTTON, position.x, position.y);
                specialTiles.Add(button.buttonPosition);

                //Creates every bridge tile associated with it
                foreach (Vector2Int bridgeCoord in button.bridgeTiles) {
                    if (specialTiles.Contains(bridgeCoord)) continue;

                    Grid[bridgeCoord.x, bridgeCoord.y] = CreateTile(TileType.BRIDGE, bridgeCoord.x, bridgeCoord.y);
                    specialTiles.Add(bridgeCoord);
                }
            }

            //------------------------------------
            // Create Other Special Tiles and Normal Tiles
            //------------------------------------
            for (int x = 0; x < mapData.FieldSize.x; ++x) {
                for (int y = 0; y < mapData.FieldSize.y; ++y) {
                    if (mapData.gridData.GetCell(x, y) == (int) TileType.HOLE) continue;
                    if (specialTiles.Contains(new Vector2Int(x, y))) continue;

                    Grid[x, y] = CreateTile((TileType) mapData.gridData.GetCell(x, y), x, y);
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

        //Helper method to create a tile of specified type at position
        private Tile CreateTile(TileType tileType, int x, int y) {
            Vector3 tilePosition = new Vector3(-mapData.FieldSize.y / 2 + 0.5f + y, -regularTile.localScale.y / 2, -mapData.FieldSize.x / 2 + 0.5f + x);
            Tile newTile = null;

            switch (tileType) {
                case TileType.BUTTON:
                    newTile = new ButtonTile(buttonTile, tilePosition, this, mapData, x, y);
                    break;
                case TileType.BRIDGE:
                    newTile = new BridgeTile(bridgeTile, tilePosition, this, mapData, x, y);
                    break;
                case TileType.BROKEN:
                    newTile = new BrokenTile(brokenTile, tilePosition, this, mapData, x, y);
                    break;
                default:
                    newTile = new Tile(regularTile, tilePosition, this, mapData, x, y);
                    break;
            }

            return newTile;
        }

        public void DeleteTile(int x, int y) {
            Destroy(Grid[x, y].TileObject.gameObject);
            Grid[x, y] = null;
        }

    }
}