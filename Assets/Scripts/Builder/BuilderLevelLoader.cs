using System;
using System.Collections.Generic;
using UnityEngine;
using Split.Tiles;
using Split.LevelLoading;

namespace Split.Builder {
    /// <summary>
    /// Generates the tile objects/meshes for the level in the Builder
    /// </summary>
    public class BuilderLevelLoader : MonoBehaviour {
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

        // Start is called before the first frame update
        private void Start() {
            this.rows = new List<Row>();
            this.allTiles = Enum.GetValues(typeof(TileType));

            this.typeToTile = new Dictionary<TileType, MeshFilter>();
            PopulateDictionary();
        }

        public void Generate(BuilderLevelData data) {
            this.levelData = data;

            for (int i = 0; i < data.gridData.Count; ++i) {
                Row row = CalculateEntireRow(data, i);            
                
                RenderRow(row);
                rows.Add(row);
            }
        }

        public void SetTile(int x, int y, TileType type) {
            TileType old = levelData.gridData[x][y];
            levelData.gridData[x][y] = type;

            RecalculateRowByType(x, old);
            RecalculateRowByType(x, type);
        }

        /**************************
        *        RENDERING        *
        **************************/

        private void RenderRow(Row row) {
            foreach (TileType type in allTiles) {
                if (row.data[type].IsEmpty()) continue;
                RenderRowByType(row, type);
            }
        }

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

        private void RecalculateRowByType(int row, TileType type) {
            if (this.levelData == null) return;

            this.rows[row].data[type] = CalculateMCD(this.levelData, row, type);
            RenderRowByType(this.rows[row], type);
        }

        private Row CalculateEntireRow(BuilderLevelData levelData, int index) {
            Row row = new Row();

            foreach (TileType type in allTiles) {
                row.data[type] = CalculateMCD(levelData, index, type);
            }

            return row;
        }

        private MeshCombineData CalculateMCD(BuilderLevelData levelData, int row, TileType type) {
            MeshCombineData data = new MeshCombineData();
            
            for (int i = 0; i < levelData.gridData[row].Count; ++i) {
                if (levelData.gridData[row][i] == type) {
                    CombineInstance combine = GetCombineInstance(this.typeToTile[type], row, i);

                    if (!data.Add(combine)) {
                        Debug.LogError($"Row {row} in Level \"{levelData.levelName}\" has reached the maximum vertex count!");
                        return data;
                    }

                }
            }

            return data;
        }

        private CombineInstance GetCombineInstance(MeshFilter tile, int x, int y) {
            CombineInstance combine = new CombineInstance();

            tile.transform.position = GridToWorldPos(x, y);
            combine.mesh = tile.sharedMesh;
            combine.transform = tile.transform.localToWorldMatrix;

            return combine;
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
    }

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