using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Split.Tiles;
using Split.LevelLoading;

namespace Split.Builder {
    /// <summary>
    /// Generates the tile objects/meshes for the level in the Builder. Each Row contains a separate mesh (and object) for each Tile Type.
    /// </summary>
    public class BuilderLevelLoader : MonoBehaviour {
        //NOTE: These prefabs are not to be used directly, other than creating instances from them. These instances are stored in the Dictionary typeToTile.
        [Header("Tile Prefabs")]
        [SerializeField] private MeshFilter emptyTile;
        [SerializeField] private MeshFilter basicTile;
        [SerializeField] private MeshFilter startTile;
        [SerializeField] private MeshFilter endTile;
        [SerializeField] private MeshFilter brokenTile;
        [SerializeField] private MeshFilter buttonTile;
        [SerializeField] private MeshFilter bridgeTile;
        [SerializeField] private MeshFilter bridgeBrokenTile;
        [SerializeField] private MeshFilter bridgeButtonTile;

        private Dictionary<TileType, MeshFilter> typeToTile;
        private List<Row> rows;
        private BuilderLevelData levelData;
        private Array allTiles;
        private LevelSerializer serializer;

        public BuilderLevelData LevelData => levelData;

        private void Awake() {
            this.serializer = new LevelSerializer();
            this.rows = new List<Row>();
            this.allTiles = Enum.GetValues(typeof(TileType));

            this.typeToTile = new Dictionary<TileType, MeshFilter>();
            PopulateDictionary();
        }

        /// <summary>
        /// Creates the tile objects/meshes for the specified input level data.
        /// </summary>
        /// <param name="data">Data for the level layout to generate</param>
        public void Generate(BuilderLevelData data) {
            this.levelData = data;

            for (int i = 0; i < data.gridData.Count; ++i) {
                rows.Add(new Row());
                CalculateEntireRow(i);
                RenderRow(rows[i]);
            }
        }

        /// <summary>
        /// Modifies the tile at a specific grid location
        /// </summary>
        /// <param name="pos">Grid coordinates</param>
        /// <param name="type">TileType to change (x,y) to</param>
        public void SetTile(Vector2Int pos, TileType type) {
            TileType old = levelData.gridData[pos.x][pos.y];
            levelData.gridData[pos.x][pos.y] = type;

            //Recalculates mesh data for both types
            CalculateRowByType(pos.x, old);
            CalculateRowByType(pos.x, type);

            //Renders new mesh data for both types
            RenderRowByType(this.rows[pos.x], old);
            RenderRowByType(this.rows[pos.x], type);
        }

        public IEnumerator Fill(Vector2Int pos1, Vector2Int pos2, TileType type) {
            //Calculate all the new data
            Vector2Int min = new Vector2Int(Math.Min(pos1.x, pos2.x), Math.Min(pos1.y, pos2.y));
            Vector2Int max = new Vector2Int(Math.Max(pos1.x, pos2.x), Math.Max(pos1.y, pos2.y));

            //TODO: verify that the coordinates are in bounds

            HashSet<TileType> affectedTypes = new HashSet<TileType>();
            affectedTypes.Add(type);

            //Update all internal type representations
            for (int x = min.x; x <= max.x; ++x) {
                for (int y = min.y; y <= max.y; ++y) {
                    affectedTypes.Add(levelData.gridData[x][y]);
                    levelData.gridData[x][y] = type;
				}
            }

            //Recalculate all mesh data and re-render
            for (int x = min.x; x <= max.x; ++x) {
                foreach (TileType affectedType in affectedTypes) {
                    CalculateRowByType(x, affectedType);
                    RenderRowByType(this.rows[x], affectedType);

                    yield return null;
                }
            }
        }
		
		//TODO: This can be a bit performance intensive when increasing Y. Perhaps make updating rendering a coroutine or something?
		public void AddSize(int x, int y) {
			if (x > 0) {
				List<TileType> list = Enumerable.Repeat(TileType.EMPTY, levelData.gridData[0].Count).ToList();
				
				for (int i = 0; i < x; ++i) {
					//populates internal data
					levelData.gridData.Add(list);
				
					//rendering
					this.rows.Add(new Row());
					CalculateRowByType(levelData.gridData.Count - 1, TileType.EMPTY);
					RenderRowByType(this.rows[levelData.gridData.Count - 1], TileType.EMPTY);
				}
			}

			if (y > 0) {
				for (int i = 0; i < levelData.gridData.Count; ++i) {
					for (int j = 0; j < y; ++j) {
						//internal data changes
						levelData.gridData[i].Add(TileType.EMPTY);
					}
	
					//rendering
					CalculateRowByType(i, TileType.EMPTY);
					RenderRowByType(this.rows[i], TileType.EMPTY);
				}
			}	
		}

        /**************************
        *        RENDERING        *
        **************************/

        /// <summary>
        /// Creates Tile Mesh GameObjects for every TileType in a specified row
        /// </summary>
        private void RenderRow(Row row) {
            foreach (TileType type in allTiles) {
                if (row.data[type].IsEmpty()) continue;
                RenderRowByType(row, type);
            }
        }

        /// <summary>
        /// Creates Tile Mesh GameObjects for a specified TileType in a specified Row
        /// </summary>
        private void RenderRowByType(Row row, TileType type) {
            GameObject obj = (row.objects.ContainsKey(type) ? row.objects[type] : new GameObject($"Row | {type}"));
            
            MeshFilter filter = obj.GetComponent<MeshFilter>();
            MeshRenderer rend = obj.GetComponent<MeshRenderer>();

            if (filter == null) filter = obj.AddComponent<MeshFilter>();
            if (rend == null) rend = obj.AddComponent<MeshRenderer>();

            filter.mesh.Clear();
            rend.material = this.typeToTile[type].GetComponent<Renderer>().sharedMaterial;
            filter.mesh.CombineMeshes(row.data[type].ToArray());
            filter.mesh.RecalculateNormals();

            row.objects[type] = obj;
        }

        /**************************
        *       CALCULATING       *
        **************************/

        /// <summary>
        /// Calculates the Mesh Data for every type in a specified row. Requires that the row is already created.
        /// Directly edits the existing row with the new information.
        /// </summary>
        /// <param name="row">Index of row to access</param>
        private void CalculateEntireRow(int row) {
            if (this.levelData == null) return;

            foreach (TileType type in allTiles) {
                CalculateRowByType(row, type);
            }
        }

        /// <summary>
        /// Calculates the Mesh Data for a specified row and type. Requires that the row is already created.
        /// Directly edits the existing row with the new information.
        /// </summary>
        /// <param name="row">Index of row to access</param>
        /// <param name="type">Type of tile to calculate</param>
        private void CalculateRowByType(int row, TileType type) {
            if (this.levelData == null) return;
            MeshCombineData data = new MeshCombineData();
            
            for (int i = 0; i < levelData.gridData[row].Count; ++i) {
                if (levelData.gridData[row][i] == type) {
                    CombineInstance combine = GetCombineInstance(this.typeToTile[type], row, i);

                    if (!data.Add(combine)) {
                        Debug.LogError($"Row {row} in Level \"{levelData.levelName}\" has reached the maximum vertex count!");
                        break;
                    }

                }
            }

            this.rows[row].data[type] = data;
        }

        /// <summary>
        /// Creates a CombineInstance from a specified tile at position (x,y)
        /// </summary>
        /// <param name="tile">Tile Object to use</param>
        /// <param name="x">Grid Position X</param>
        /// <param name="y">Grid Position Y</param>
        /// <returns></returns>
        private CombineInstance GetCombineInstance(MeshFilter tile, int x, int y) {
            CombineInstance combine = new CombineInstance();

            tile.transform.position = GridToWorldPos(x, y);
            combine.mesh = tile.sharedMesh;
            combine.transform = tile.transform.localToWorldMatrix;

            return combine;
        }

        /**************************
        *         SAVING          *
        **************************/
        public void Save() {
            serializer.Save(levelData.ToLevelData(), levelData.fileName, true);
        }

        /**************************
        *          OTHER          *
        **************************/

        //The formula used here is different from the one in LevelGenerator
        public Vector3 GridToWorldPos(int x, int y) {
            return new Vector3(0.5f + y, 0, 0.5f + x);
        }

        private void PopulateDictionary() {
            foreach (TileType type in allTiles) {
                this.typeToTile[type] = Instantiate(GetObjectByType(type).gameObject, Vector3.zero, Quaternion.identity).GetComponent<MeshFilter>();
                this.typeToTile[type].gameObject.SetActive(false);
            }
        }

        private MeshFilter GetObjectByType(TileType type) {
            switch (type) {
                case TileType.EMPTY:
                    return emptyTile;
                case TileType.BASIC:
                    return basicTile;
                case TileType.START:
                    return startTile;
                case TileType.END:
                    return endTile;
                case TileType.BROKEN:
                    return brokenTile;
                case TileType.BUTTON:
                    return buttonTile;
                case TileType.BRIDGE:
                    return bridgeTile;
                case TileType.BRIDGE_BROKEN:
                    return bridgeBrokenTile;
                case TileType.BRIDGE_BUTTON:
                    return bridgeButtonTile;
                default:
                    return null;
            }
        }

        public Color GetTileColor(TileType type) {
            return this.typeToTile[type].GetComponent<Renderer>().sharedMaterial.color;
        }

        public void SetTypeActive(TileType type, bool active) {
            GameObject obj;

            foreach (Row row in rows) {
                if (row.objects.TryGetValue(type, out obj)) {
                    obj.SetActive(active);
                }
            }
        }
    }

    /// <summary>
    /// Represents a row of tiles in the level editor/builder.
    /// </summary>
    public class Row {
        public Dictionary<TileType, MeshCombineData> data;
        public Dictionary<TileType, GameObject> objects;

        public Row() {
            data = new Dictionary<TileType, MeshCombineData>();
            objects = new Dictionary<TileType, GameObject>();

            foreach (TileType type in Enum.GetValues(typeof(TileType))) {
                data[type] = new MeshCombineData();
            }
        }
    }
}
