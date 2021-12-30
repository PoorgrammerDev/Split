using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Split.Tiles;
using Split.LevelLoading;

namespace Split.Builder {
    public class BuilderLevelLoader : MonoBehaviour {
        [Header("Tile Prefabs")]
        [SerializeField] private MeshFilter basicTile;
        [SerializeField] private MeshFilter startTile;
        [SerializeField] private MeshFilter endTile;
        [SerializeField] private MeshFilter brokenTile;
        [SerializeField] private MeshFilter buttonTile;
        [SerializeField] private MeshFilter bridgeTile;
        [SerializeField] private MeshFilter bridgeBrokenTile;
        [SerializeField] private MeshFilter bridgeButtonTile;

        private List<Row> rows;
        private BuilderLevelData levelData;

        // Start is called before the first frame update
        private void Start() {
            this.rows = new List<Row>();

            this.basicTile = Instantiate(basicTile.gameObject, Vector3.zero, Quaternion.identity).GetComponent<MeshFilter>();
            this.startTile = Instantiate(startTile.gameObject, Vector3.zero, Quaternion.identity).GetComponent<MeshFilter>();
            this.endTile = Instantiate(endTile.gameObject, Vector3.zero, Quaternion.identity).GetComponent<MeshFilter>();
            this.brokenTile = Instantiate(brokenTile.gameObject, Vector3.zero, Quaternion.identity).GetComponent<MeshFilter>();
            this.buttonTile = Instantiate(buttonTile.gameObject, Vector3.zero, Quaternion.identity).GetComponent<MeshFilter>();
            this.bridgeTile = Instantiate(bridgeTile.gameObject, Vector3.zero, Quaternion.identity).GetComponent<MeshFilter>();
            this.bridgeBrokenTile = Instantiate(bridgeBrokenTile.gameObject, Vector3.zero, Quaternion.identity).GetComponent<MeshFilter>();
            this.bridgeButtonTile = Instantiate(bridgeButtonTile.gameObject, Vector3.zero, Quaternion.identity).GetComponent<MeshFilter>();

            this.basicTile.gameObject.SetActive(false);
            this.startTile.gameObject.SetActive(false);
            this.endTile.gameObject.SetActive(false);
            this.brokenTile.gameObject.SetActive(false);
            this.buttonTile.gameObject.SetActive(false);
            this.bridgeTile.gameObject.SetActive(false);
            this.bridgeBrokenTile.gameObject.SetActive(false);
            this.bridgeButtonTile.gameObject.SetActive(false);
        }

        public IEnumerator test() {
            WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.25f);

            while (true) {
                int x = UnityEngine.Random.Range(0, levelData.gridData.Count);
                int y = UnityEngine.Random.Range(0, levelData.gridData[0].Count);

                TileType oldType = levelData.gridData[x][y];
                levelData.gridData[x][y] = TileType.EMPTY;

                RecalculateRowByType(x, oldType);

                yield return wait;
            }
        }

        public void Generate(BuilderLevelData data) {
            this.levelData = data;

            for (int i = 0; i < data.gridData.Count; ++i) {
                Row row = CalculateEntireRow(data, i);            
                
                RenderRow(row);
                rows.Add(row);
            }

             StartCoroutine(test());
        }

        public void RecalculateRowByType(int row, TileType type) {
            if (this.levelData == null || type == TileType.EMPTY) return;

            this.rows[row].data[type] = CalculateMCD(this.levelData, row, type);
            RenderRowByType(this.rows[row], type);
        }

        private void RenderRow(Row row) {
            foreach (TileType type in Enum.GetValues(typeof(TileType))) {
                if (type == TileType.EMPTY || row.data[type].IsEmpty()) continue;
                RenderRowByType(row, type);
            }
        }

        private void RenderRowByType(Row row, TileType type) {
            if (type == TileType.EMPTY) return;

            GameObject obj = (row.objects.ContainsKey(type) ? row.objects[type] : new GameObject($"Row | {type}"));
            
            MeshFilter filter = obj.GetComponent<MeshFilter>();
            MeshRenderer rend = obj.GetComponent<MeshRenderer>();

            if (filter == null) filter = obj.AddComponent<MeshFilter>();
            if (rend == null) rend = obj.AddComponent<MeshRenderer>();

            filter.mesh.Clear();
            rend.material = GetObjectByType(type).GetComponent<Renderer>().sharedMaterial;
            filter.mesh.CombineMeshes(row.data[type].ToArray());
            filter.mesh.RecalculateNormals();

            row.objects[type] = obj;
        }


        private Row CalculateEntireRow(BuilderLevelData levelData, int index) {
            Row row = new Row();

            foreach (TileType type in Enum.GetValues(typeof(TileType))) {
                if (type == TileType.EMPTY) continue;
                
                row.data[type] = CalculateMCD(levelData, index, type);
            }

            return row;
        }

        private MeshCombineData CalculateMCD(BuilderLevelData levelData, int row, TileType type) {
            if (type == TileType.EMPTY) return null;
            MeshCombineData data = new MeshCombineData();
            
            for (int i = 0; i < levelData.gridData[row].Count; ++i) {
                if (levelData.gridData[row][i] == type) {
                    CombineInstance combine = GetCombineInstance(GetObjectByType(type), row, i);

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

        //The formula used here is different from the one in LevelGenerator
        private Vector3 GridToWorldPos(int x, int y) {
            return new Vector3(0.5f + y, 0, 0.5f + x);
        }

        private MeshFilter GetObjectByType(TileType type) {
            switch (type) {
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
                //Skip empty
                if (type == TileType.EMPTY) continue;

                data[type] = new MeshCombineData();
            }
        }
    }
}