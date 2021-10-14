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

        //TODO: Refactor this to prevent repitition
        public void MoveForward(InputAction.CallbackContext context) {
            if (!context.performed) return;

            Vector3 movePos;
            if (ValidateMovePosition(currentPosition.x + 1, currentPosition.y, out movePos)) {
                Vector2Int oldPosition = currentPosition;

                //Update position and move player
                currentPosition.x++;
                movePos.y = this.transform.position.y;
                LeanTween.move(this.gameObject, movePos, moveSpeed);

                //Fire Move Event
                GameEvents.current.PlayerMoveToTile(oldPosition, currentPosition);
            }
        }

        public void MoveBackward(InputAction.CallbackContext context) {
            if (!context.performed) return;

            Vector3 movePos;
            if (ValidateMovePosition(currentPosition.x - 1, currentPosition.y, out movePos)) {
                Vector2Int oldPosition = currentPosition;

                //Update position and move player
                currentPosition.x--;
                movePos.y = this.transform.position.y;
                LeanTween.move(this.gameObject, movePos, moveSpeed);

                //Fire Move Event
                GameEvents.current.PlayerMoveToTile(oldPosition, currentPosition);
            }
        }

        public void MoveLeft(InputAction.CallbackContext context) {
            if (!context.performed) return;

            Vector3 movePos;
            if (ValidateMovePosition(currentPosition.x, currentPosition.y - 1, out movePos)) {
                Vector2Int oldPosition = currentPosition;

                //Update position and move player
                currentPosition.y--;
                movePos.y = this.transform.position.y;
                LeanTween.move(this.gameObject, movePos, moveSpeed);

                //Fire Move Event
                GameEvents.current.PlayerMoveToTile(oldPosition, currentPosition);
            }
        }

        public void MoveRight(InputAction.CallbackContext context) {
            if (!context.performed) return;

            Vector3 movePos;
            if (ValidateMovePosition(currentPosition.x, currentPosition.y + 1, out movePos)) {
                Vector2Int oldPosition = currentPosition;

                //Update position and move player
                currentPosition.y++;
                movePos.y = this.transform.position.y;
                LeanTween.move(this.gameObject, movePos, moveSpeed);

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