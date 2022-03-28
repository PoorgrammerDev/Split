using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Split.Tiles;
using System.Linq;
using Split.Player;

namespace Split.LevelLoading
{
    /// <summary>
    /// Generates tiles/level from LevelData.
    /// </summary>
    public class LevelGenerator : MonoBehaviour {
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
        private float bridgeTileAlpha;
        private float bridgeBrokenTileAlpha;
        private float bridgeButtonTileAlpha;

        private void Awake() {
            PopulateLevelData();

            this.basicTile = Instantiate(basicTile, Vector3.zero, Quaternion.identity);
            this.basicMeshFilter = basicTile.GetComponent<MeshFilter>();
            this.basicTile.SetActive(false);
            
            this.bridgeTileAlpha = bridgeTile.GetComponent<Renderer>().sharedMaterial.color.a;
            this.bridgeBrokenTileAlpha = bridgeBrokenTile.GetComponent<Renderer>().sharedMaterial.color.a;
            this.bridgeButtonTileAlpha = bridgeButtonTile.GetComponent<Renderer>().sharedMaterial.color.a;

            Generate();
        }

        private void PopulateLevelData() {
            LevelDataHolder dataHolder = FindObjectOfType<LevelDataHolder>();

            //No valid level data - return to main menu
            if (dataHolder == null) {
                Debug.LogError("Loaded Game Scene with no level data; returning to Main Menu");
                SceneManager.LoadScene(TagHolder.MAIN_MENU_SCENE, LoadSceneMode.Single);
                return;
            }

            this.LevelData = dataHolder.Data;
            // Destroy(dataHolder.gameObject);
        }

        /// <summary>
        /// Main mechanism of the class. Reads the level data and generates the tiles.
        /// </summary>
        public void Generate() {
            if (this.LevelData == null) return;

            Grid = new Tile[this.LevelData.gridData.x, this.LevelData.gridData.y];
            basicMesh = new List<MeshCombineData>();
            basicMesh.Add(new MeshCombineData());

            //Create tiles based on data in 2D array
            for (int x = 0; x < Grid.GetLength(0); ++x) {
                for (int y = 0; y < Grid.GetLength(1); ++y) {
                    if (this.LevelData.gridData[x, y] == TileType.EMPTY) continue;

                    Grid[x, y] = CreateTile(this.LevelData.gridData[x, y], x, y);
                }
            }

            RenderBasicMesh();
        }

        /// <summary>
        /// Creates a new Tile Object that holds a newly instaniated GameObject.
        /// (Except for Basic Tiles, in which it is added to a large Mesh)
        /// </summary>
        /// <param name="type">Desired TileType at grid position (x,y)</param>
        /// <param name="x">Grid Position X</param>
        /// <param name="y">Grid Position Y</param>
        /// <returns>Newly instantiated Tile object</returns>
        private Tile CreateTile(TileType type, int x, int y) {
            Vector3 worldPos = GridToWorldPos(x, y);
            GameObject tileGO;
            Tile newTile;

            switch (type) {
                //NOTE: Only the basic tile is combined into a mesh, the rest of the tiles are simply instantiated by themselves
                //This is due to the fact that the basic tile never changes, whereas the other tiles can disappear, vary in opacity, glow, etc.
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
                    newTile = new ButtonTile(tileGO, this.playerManager, x, y);
                    break;

                case TileType.BRIDGE:
                    tileGO = GameObject.Instantiate(bridgeTile, worldPos, Quaternion.identity);
                    newTile = new BridgeTile(bridgeTileAlpha, this.playerManager, this.LevelData, tileGO, x, y);
                    break;

                case TileType.BRIDGE_BROKEN:
                    tileGO = GameObject.Instantiate(bridgeBrokenTile, worldPos, Quaternion.identity);
                    newTile = new BridgeBrokenTile(bridgeBrokenTileAlpha, this.playerManager, this.LevelData, tileGO, x, y);
                    break;

                case TileType.BRIDGE_BUTTON:
                    tileGO = GameObject.Instantiate(bridgeButtonTile, worldPos, Quaternion.identity);
                    newTile = new BridgeButtonTile(bridgeButtonTileAlpha, this.playerManager, this.LevelData, tileGO, x, y);
                    break;

                default:
                    return null;
            }
            
            return newTile;
        }

        /// <summary>
        /// Adds a basic tile at specified world position to the combined mesh data.
        /// If the vertex count exceeds the max, it creates a new one.
        /// </summary>
        /// <param name="worldPos">World Position of Basic Tile (NOT Grid Position)</param>
        private void AddToBasicMesh(Vector3 worldPos) {
            //Set basic tile position to desired pos
            basicTile.transform.position = worldPos;

            //Create a combine instance and set its mesh and transform to proper values
            CombineInstance combine = new CombineInstance();
            combine.mesh = basicMeshFilter.sharedMesh;
            combine.transform = basicTile.transform.localToWorldMatrix;

            //If verticies are over max, make new mesh
            if (!this.basicMesh.Last().HasSpace(combine)) {
                this.basicMesh.Add(new MeshCombineData());
            }
            
            //Add combine instance to the list
            this.basicMesh.Last().Add(combine);
        }

        /// <summary>
        /// Takes the combined mesh data (Stored as a list of MeshCombineData objects)
        /// and creates GameObjects off of them.
        /// </summary>
        private void RenderBasicMesh() {
            Renderer prefabRenderer = basicTile.GetComponent<Renderer>();

            foreach (MeshCombineData data in this.basicMesh) {
                GameObject obj = new GameObject("Basic Tile Mesh");

                MeshFilter filter = obj.AddComponent<MeshFilter>();
                MeshRenderer rend = obj.AddComponent<MeshRenderer>();

                rend.material = prefabRenderer.sharedMaterial;
                filter.mesh.CombineMeshes(data.ToArray());
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
}

