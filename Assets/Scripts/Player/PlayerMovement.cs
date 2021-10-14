using UnityEngine;
using UnityEngine.InputSystem;
using Split.Map;
using Split.Map.Tiles;

namespace Split.Player {

    /*
     * This class handles player movement using the new action-based Unity Input system
     */

    public class PlayerMovement : MonoBehaviour {
        [Header("References")]
        [SerializeField] private MapGenerator mapGenerator;
        [SerializeField] private MapData mapData;

        [Header("Settings")]
        [SerializeField] private float moveSpeed;

        private Vector2Int currentPosition;

        // Start is called before the first frame update
        void Start() {
            currentPosition = mapData.SpawnPosition;
        }

        //NOTE: The directions on the Vectors don't match because the game is being viewed in a different angle
        public void MoveForward(InputAction.CallbackContext context) {
            if (!context.performed) return;
            MoveToPosition(currentPosition + Vector2Int.right);
        }

        public void MoveBackward(InputAction.CallbackContext context) {
            if (!context.performed) return;
            MoveToPosition(currentPosition + Vector2Int.left);
        }

        public void MoveLeft(InputAction.CallbackContext context) {
            if (!context.performed) return;
            MoveToPosition(currentPosition + Vector2Int.down);
        }

        public void MoveRight(InputAction.CallbackContext context) {
            if (!context.performed) return;
            MoveToPosition(currentPosition + Vector2Int.up);
        }

        private void MoveToPosition(Vector2Int newPosition) {
            Vector3 moveWorldPos;
            if (ValidateMovePosition(newPosition.x, newPosition.y, out moveWorldPos)) {
                Vector2Int oldPosition = currentPosition;

                //Update position and move player
                currentPosition = newPosition;
                moveWorldPos.y = this.transform.position.y;
                LeanTween.move(this.gameObject, moveWorldPos, moveSpeed);

                //Fire Move Event
                GameEvents.current.PlayerMoveToTile(oldPosition, currentPosition);
            }
        }

        private bool ValidateMovePosition(int x, int y, out Vector3 worldPos) {
            //Validates that specified position is in bounds of the board
            if ((x >= 0 && x < mapData.FieldSize.x) && (y >= 0 && y < mapData.FieldSize.y)) {
                //Validates that there is a panel at that position
                if (mapGenerator.Grid[x, y] != null) {
                    Tile tile = mapGenerator.Grid[x, y];
                    bool output = true;
                    
                    //Testing Special Tiles
                    if (tile is BridgeTile) {
                        BridgeTile bridge = tile as BridgeTile;
                        output = bridge.Activated;
                    }

                    worldPos = mapGenerator.Grid[x,y].TileObject.position;
                    return output;
                }
            }

            worldPos = Vector3.zero;
            return false;
        }
    }
}