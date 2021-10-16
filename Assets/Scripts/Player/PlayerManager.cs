using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Split.Map;

namespace Split.Player {

    /*
     * This class handles the multiple instances of the player, specifically switching between them 
     */

    public class PlayerManager : MonoBehaviour {
        [Header("References")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private CameraFollow cameraFollow;
        [SerializeField] private MapData mapData;
        [SerializeField] private MapGenerator mapGenerator;

        [Header("Settings")]
        [Min(1)]
        [SerializeField] private int maxCount;
        [SerializeField] private Color deactivatedColor;
        [SerializeField] private Color waitingColor;
        [SerializeField] private float waitFlashSpeed;

        private GameObject[] playerObjects;
        private int activePlyIndex;
        private Color normalColor;

        private bool newPlayerMoved;
        public int OldPlayer{get; private set;}

        private void Awake() {
            this.playerObjects = new GameObject[maxCount];
            this.normalColor = playerPrefab.GetComponent<Renderer>().sharedMaterial.color;

            //Instantiates all player instances
            for (int i = 0; i < maxCount; ++i) {
                playerObjects[i] = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, this.transform);
                playerObjects[i].SetActive(false);
            }

            //Activates Main Player (0)
            playerObjects[0].SetActive(true);
            cameraFollow.target = playerObjects[0].transform;
            this.activePlyIndex = 0;
        }

        // Start is called before the first frame update
        private void Start() {
            foreach (GameObject obj in playerObjects) {

                //Set Spawn Position for each Player
                Vector3 spawnPosition = mapGenerator.Grid[mapData.SpawnPosition.x, mapData.SpawnPosition.y].TileObject.position;
                spawnPosition.y = obj.transform.localScale.y;
                obj.transform.position = spawnPosition;
            }
        }

        /*
            Input: Switching between player instances
        */
        public void SwitchToPlayer(int ply) {
            if (ply < 0 || ply >= maxCount || ply == activePlyIndex) return;

            //First=time enabling
            if (!playerObjects[ply].activeInHierarchy) {
                StartCoroutine(EnableNewPlayer(ply));

            }
            else {
                LeanTween.color(playerObjects[ply], normalColor, 0.5f);
            }

            LeanTween.color(playerObjects[activePlyIndex], deactivatedColor, 0.5f);
            cameraFollow.target = playerObjects[ply].transform;
            this.activePlyIndex = ply;
        }

        // === Coroutine ===
        private IEnumerator EnableNewPlayer(int ply) {
            //Sets spawn position in world-space
            //NOTE: Grid-space spawn is handled in PlayerMovement.cs upon first move
            Vector3 spawnPosition = playerObjects[activePlyIndex].transform.position;
            spawnPosition.y = playerObjects[ply].transform.localScale.y;
            playerObjects[ply].transform.position = spawnPosition;

            //Sets new object active and old deactive
            playerObjects[ply].SetActive(true);
            playerObjects[activePlyIndex].GetComponent<Renderer>().enabled = false;
            
            //Flashes colors while waiting for player to move --- --- ---
            Material plyMat = playerObjects[ply].GetComponent<Renderer>().material;
            float t = 0.0f;
            bool flip = false;

            this.newPlayerMoved = false;
            this.OldPlayer = activePlyIndex;

            while (!newPlayerMoved) {
                plyMat.color = Color.Lerp(normalColor, waitingColor, t);

                t = Mathf.Clamp(t + (Time.deltaTime * waitFlashSpeed * (flip ? -1 : 1)), 0, 1);
                if (t == 1 || t == 0) {
                    flip = !flip;
                }

                yield return null;
            }
            //--- --- ---

            playerObjects[OldPlayer].GetComponent<Renderer>().enabled = true;
            LeanTween.color(playerObjects[ply], normalColor, 0.5f);
        }


        //------------------------
        // "Accessor" Properties
        //------------------------

        public int ActivePlayerIndex {get {return activePlyIndex;}}
        
        public GameObject ActivePlayer {get {
            return playerObjects[activePlyIndex];    
        }}

        public int MaxCount {get {return maxCount;}}

        public void NewPlayerMoved() {
            this.newPlayerMoved = true;
        }

    }
}