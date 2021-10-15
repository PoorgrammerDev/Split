using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Split.Map;

namespace Split.Player {
    public class PlayerManager : MonoBehaviour {
        [Header("References")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private CameraFollow cameraFollow;
        [SerializeField] private MapData mapData; //TODO: Maybe replace this with a manager class?
        [SerializeField] private MapGenerator mapGenerator;

        [Header("Settings")]
        [Min(1)]
        [SerializeField] private int maxCount;
        [SerializeField] private Color deactivatedColor;

        private GameObject[] playerObjects;
        private int activePlyIndex;
        
        //Properties for public read-access to data
        public int ActivePlayerIndex {get {return activePlyIndex;}}
        
        public GameObject ActivePlayer {get {
            return playerObjects[activePlyIndex];    
        }}

        public int MaxCount {get {return maxCount;}}

        // Start is called before the first frame update
        void Awake() {
            playerObjects = new GameObject[maxCount];

            for (int i = 0; i < maxCount; ++i) {
                playerObjects[i] = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, this.transform);
                playerObjects[i].SetActive(false);
            }

            playerObjects[0].SetActive(true);
            cameraFollow.target = playerObjects[0].transform;
            this.activePlyIndex = 0;
        }

        void Start() {
            foreach (GameObject obj in playerObjects) {

                //Set Spawn Position for each Player
                Vector3 spawnPosition = mapGenerator.Grid[mapData.SpawnPosition.x, mapData.SpawnPosition.y].TileObject.position;
                spawnPosition.y = obj.transform.localScale.y;

                obj.transform.position = spawnPosition;
            }
        }

        public void SwitchToPlayer(int ply) {
            if (ply < 0 || ply >= maxCount || ply == activePlyIndex) return;

            LeanTween.color(playerObjects[activePlyIndex], deactivatedColor, 0.5f);

            playerObjects[ply].SetActive(true);
            cameraFollow.target = playerObjects[ply].transform;
            LeanTween.color(playerObjects[ply], playerPrefab.GetComponent<Renderer>().sharedMaterial.color, 0.5f);

            this.activePlyIndex = ply;
        }

    }
}