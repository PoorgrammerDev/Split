using System.Collections.Generic;
using UnityEngine;
using Split.Tiles;
using System.Linq;

namespace Split.LevelLoading
{
    public class LevelGenerator : MonoBehaviour
    {
        private const int MAX_MESH_VERTICIES = 65535;

        [Header("Tile Prefabs")]
        [SerializeField] private GameObject basicTile;
        [SerializeField] private GameObject startTile;
        [SerializeField] private GameObject endTile;
        [SerializeField] private GameObject brokenTile;
        [SerializeField] private GameObject buttonTile;
        [SerializeField] private GameObject bridgeTile;
        [SerializeField] private GameObject bridgeBrokenTile;
        [SerializeField] private GameObject bridgeButtonTile;

        public Tile[,] Grid {get; private set;}
        private List<MeshCombineData> basicMesh;

        private MeshFilter basicMeshFilter;

        public void Start() {
            LevelData data = new LevelData();
            data.gridData = new TileType[,]{
                    {TileType.BASIC,TileType.BASIC,TileType.BASIC,TileType.BASIC,TileType.BASIC},
                    {TileType.BASIC,TileType.BASIC,TileType.BASIC,TileType.BASIC,TileType.BASIC},
                    {TileType.BASIC,TileType.EMPTY,TileType.EMPTY,TileType.EMPTY,TileType.BASIC},
                    {TileType.BASIC,TileType.BASIC,TileType.BASIC,TileType.BROKEN,TileType.BASIC},
                    {TileType.BASIC,TileType.BASIC,TileType.BROKEN,TileType.BROKEN,TileType.BASIC},
                };

            Generate(data);
        }


        public void Generate(LevelData data) {
            Grid = new Tile[data.gridData.GetLength(0), data.gridData.GetLength(1)];
            basicMesh = new List<MeshCombineData>();
            basicMesh.Add(new MeshCombineData());

            basicMeshFilter = basicTile.GetComponent<MeshFilter>();

            //Save original position of basic prefab
            Vector3 origBasicPos = basicTile.transform.position;

            //Create tiles based on data in 2D array
            for (int x = 0; x < Grid.GetLength(0); ++x) {
                for (int y = 0; y < Grid.GetLength(1); ++y) {
                    if (data.gridData[x, y] == TileType.EMPTY) continue;

                    Grid[x, y] = CreateTile(data, data.gridData[x, y], x, y);
                }
            }

            RenderBasicMesh();

            //Restore original position
            basicTile.transform.position = origBasicPos;
        }

        private Tile CreateTile(LevelData data, TileType type, int x, int y) {
            Vector3 worldPos = new Vector3(-Grid.GetLength(0) / 2 + 0.5f + y, -basicTile.transform.localScale.y / 2, -Grid.GetLength(1) / 2 + 0.5f + x); //TODO: I have no idea what this does and why it does it
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
                    newTile = new ButtonTile(tileGO, x, y);
                    break;

                case TileType.BRIDGE:
                    tileGO = GameObject.Instantiate(bridgeTile, worldPos, Quaternion.identity);
                    newTile = new BridgeTile(0.25f, data, tileGO, x, y);
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
            if (this.basicMesh.Last().vertexCount > MAX_MESH_VERTICIES) {
                this.basicMesh.Add(new MeshCombineData());
            }
            //Set basic tile position to desired pos
            basicTile.transform.position = worldPos;

            //Create a combine instance and set its mesh and transform to proper values
            CombineInstance combine = new CombineInstance();
            combine.mesh = basicMeshFilter.sharedMesh;
            combine.transform = basicTile.transform.localToWorldMatrix;
            
            //Add combine instance to the list
            this.basicMesh.Last().combineInstances.Add(combine);
            this.basicMesh.Last().vertexCount += combine.mesh.vertexCount;
        }

        private void RenderBasicMesh() {
            Renderer prefabRenderer = basicTile.GetComponent<Renderer>();

            foreach (MeshCombineData data in this.basicMesh) {
                GameObject obj = new GameObject("BasicMesh");

                MeshFilter filter = obj.AddComponent<MeshFilter>();
                MeshRenderer rend = obj.AddComponent<MeshRenderer>();

                rend.material = prefabRenderer.sharedMaterial;
                filter.mesh.CombineMeshes(data.combineInstances.ToArray());
                filter.mesh.RecalculateNormals();
            }
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

