using UnityEngine;
using Split.Map;

namespace Split.Player {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private MapGenerator mapGenerator;
        [SerializeField] private MapData mapData;
        private Vector2Int currentPosition;

        // Start is called before the first frame update
        void Start() {
            currentPosition = mapData.SpawnPosition;
        }

        // Update is called once per frame
        //TODO: FIXME: HORRIBLE HORRIBLE CODE REPLACE PLEASE THIS IS A PLACEHOLDER
        void Update() {
            Transform moveSpace = null;

            if (Input.GetKeyDown(KeyCode.W) && currentPosition.x != mapData.FieldSize.x - 1) {
                moveSpace = mapGenerator.Grid[currentPosition.x + 1, currentPosition.y];
                if (moveSpace != null) currentPosition.x++;
            }
            else if (Input.GetKeyDown(KeyCode.D) && currentPosition.y != mapData.FieldSize.y - 1) {
                moveSpace = mapGenerator.Grid[currentPosition.x, currentPosition.y + 1];
                if (moveSpace != null) currentPosition.y++;
            }
            else if (Input.GetKeyDown(KeyCode.S) && currentPosition.x != 0) {
                moveSpace = mapGenerator.Grid[currentPosition.x - 1, currentPosition.y];
                if (moveSpace != null) currentPosition.x--;
            }
            else if (Input.GetKeyDown(KeyCode.A) && currentPosition.y != 0) {
                moveSpace = mapGenerator.Grid[currentPosition.x, currentPosition.y - 1];
                if (moveSpace != null) currentPosition.y--;
            }

            if (moveSpace != null) {
                Vector3 playerPosition = this.transform.position;
                playerPosition.x = moveSpace.position.x;
                playerPosition.z = moveSpace.position.z;

                LeanTween.move(this.gameObject, playerPosition, 0.1f);
            }
        }
    }
}