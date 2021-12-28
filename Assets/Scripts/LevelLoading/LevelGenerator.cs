using System.Collections.Generic;
using UnityEngine;
using Split.Tiles;
using System.Linq;
using Split.Player;

namespace Split.LevelLoading
{
    public class LevelGenerator : MonoBehaviour
    {
        private const int MAX_MESH_VERTICIES = 65535;

        [Header("References")]
        [SerializeField] private PlayerManager playerManager;

        [Header("Tile Prefabs")]
        [SerializeField] private GameObject basicTile;
        [SerializeField] private GameObject startTile;
        [SerializeField] private GameObject endTile;
        [SerializeField] private GameObject brokenTile;
        [SerializeField] private GameObject buttonTile;
        [SerializeField] private GameObject bridgeTile;
        [SerializeField] private GameObject bridgeBrokenTile;
        [SerializeField] private GameObject bridgeButtonTile;

        public LevelData LevelData {get; set;}
        public Tile[,] Grid {get; private set;}
        private List<MeshCombineData> basicMesh;
        private MeshFilter basicMeshFilter;

        //TODO: testing, remove
        void Start() {
            LevelData levelData = new LevelData();

            const int a = 100;

            levelData.gridData = new TileType[a, a];
            levelData.startPosition = Vector2Int.zero;
            for (int x = 0; x < a; x++) {
                for (int y = 0; y < a; y++) {
                    levelData.gridData[x, y] = TileType.BASIC;//(TileType) Random.Range(0, 8);
                }
            }

            levelData.gridData[1,1] = TileType.BUTTON;

            levelData.gridData[3,1] = TileType.BRIDGE;
            levelData.gridData[3,2] = TileType.BRIDGE;
            levelData.gridData[3,3] = TileType.BRIDGE;

            levelData.buttonTileData = new ButtonTileData[1];

            ButtonTileData abc = new ButtonTileData();
            abc.tilePosition = new Vector2Int(1, 1);
            abc.bridgeTiles = new Vector2Int[3];
            abc.bridgeTiles[0] = new Vector2Int(3, 1);
            abc.bridgeTiles[1] = new Vector2Int(3, 2);
            abc.bridgeTiles[2] = new Vector2Int(3, 3);

            levelData.buttonTileData[0] = abc;
            levelData.startPosition = Vector2Int.right;

            this.LevelData = levelData;
            Generate();
        }

        public void Generate() {
            if (this.LevelData == null) return;

            Grid = new Tile[this.LevelData.gridData.GetLength(0), this.LevelData.gridData.GetLength(1)];
            basicMesh = new List<MeshCombineData>();
            basicMesh.Add(new MeshCombineData());

            basicMeshFilter = basicTile.GetComponent<MeshFilter>();

            //Save original position of basic prefab
            Vector3 origBasicPos = basicTile.transform.position;

            //Create tiles based on data in 2D array
            for (int x = 0; x < Grid.GetLength(0); ++x) {
                for (int y = 0; y < Grid.GetLength(1); ++y) {
                    if (this.LevelData.gridData[x, y] == TileType.EMPTY) continue;

                    Grid[x, y] = CreateTile(this.LevelData.gridData[x, y], x, y);
                }
            }

            RenderBasicMesh();

            //Restore original position
            basicTile.transform.position = origBasicPos;
        }

        private Tile CreateTile(TileType type, int x, int y) {
            Vector3 worldPos = GridToWorldPos(x, y);
            GameObject tileGO;
            Tile newTile;

            switch (type) {
                case TileType.BASIC:
                    newTile = new Tile();
                    AddToBasicMesh(worldPos);
                    break;
                case TileType.START:
                    tileGO = GameObject.Instantiate(startTile, worldPos, Quaternion.identity);
                    newTile = new StartTile(tileGO, x, y);
                    break;

                case TileType.END:
                    tileGO = GameObject.Instantiate(endTile, worldPos, Quaternion.identity);
                    newTile = new EndTile(tileGO, x, y);
                    break;

                case TileType.BROKEN:
                    tileGO = GameObject.Instantiate(brokenTile, worldPos, Quaternion.identity);
                    newTile = new BrokenTile(tileGO, x, y);
                    break;

                case TileType.BUTTON:
                    tileGO = GameObject.Instantiate(buttonTile, worldPos, Quaternion.identity);
                    newTile = new ButtonTile(tileGO, playerManager, x, y);
                    break;

                case TileType.BRIDGE:
                    tileGO = GameObject.Instantiate(bridgeTile, worldPos, Quaternion.identity);
                    newTile = new BridgeTile(0.25f, this.LevelData, tileGO, x, y);
                    break;

                case TileType.BRIDGE_BROKEN:
                    tileGO = GameObject.Instantiate(bridgeBrokenTile, worldPos, Quaternion.identity);
                    newTile = new BridgeBrokenTile(tileGO, x, y);
                    break;

                case TileType.BRIDGE_BUTTON:
                    tileGO = GameObject.Instantiate(bridgeButtonTile, worldPos, Quaternion.identity);
                    newTile = new BridgeButtonTile(tileGO, x, y);
                    break;

                default:
                    return null;
            }
            
            return newTile;
        }

        private void AddToBasicMesh(Vector3 worldPos) {
            //Set basic tile position to desired pos
            basicTile.transform.position = worldPos;

            //Create a combine instance and set its mesh and transform to proper values
            CombineInstance combine = new CombineInstance();
            combine.mesh = basicMeshFilter.sharedMesh;
            combine.transform = basicTile.transform.localToWorldMatrix;

            //If verticies are over max, make new mesh
            if (this.basicMesh.Last().vertexCount + combine.mesh.vertexCount > MAX_MESH_VERTICIES) {
                this.basicMesh.Add(new MeshCombineData());
            }
            
            //Add combine instance to the list
            this.basicMesh.Last().combineInstances.Add(combine);
            this.basicMesh.Last().vertexCount += combine.mesh.vertexCount;
        }

        private void RenderBasicMesh() {
            Renderer prefabRenderer = basicTile.GetComponent<Renderer>();

            foreach (MeshCombineData data in this.basicMesh) {
                GameObject obj = new GameObject("Basic Tile Mesh");

                MeshFilter filter = obj.AddComponent<MeshFilter>();
                MeshRenderer rend = obj.AddComponent<MeshRenderer>();

                rend.material = prefabRenderer.sharedMaterial;
                filter.mesh.CombineMeshes(data.combineInstances.ToArray());
                filter.mesh.RecalculateNormals();
            }
        }

        public Vector3? GetSpawnWorldPos() {
            if (LevelData == null) return null;
            return GridToWorldPos(LevelData.startPosition.x, LevelData.startPosition.y);
        }

        public Vector3 GridToWorldPos(int x, int y) {
            return new Vector3(-Grid.GetLength(0) / 2 + 0.5f + y, -basicTile.transform.localScale.y / 2, -Grid.GetLength(1) / 2 + 0.5f + x);
        }
    
    }

    public class MeshCombineData {
        public List<CombineInstance> combineInstances;
        public int vertexCount;

        public MeshCombineData() {
            combineInstances = new List<CombineInstance>();
            vertexCount = 0;
        }
    }
}

