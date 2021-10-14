using UnityEngine;
using Split.Map;

namespace Split.Player {
    public class PlayerSpawn : MonoBehaviour {
        [SerializeField] private MapData mapData; //TODO: Maybe replace this with a manager class?
        [SerializeField] private MapGenerator mapGenerator;

        // Start is called before the first frame update
        void Start() {
            Vector3 spawnPosition = mapGenerator.Grid[mapData.SpawnPosition.x, mapData.SpawnPosition.y].TileObject.position;
            spawnPosition.y = this.transform.localScale.y;

            this.transform.position = spawnPosition;
        }
    }
}