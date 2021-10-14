using UnityEngine;
using UnityEngine.InputSystem;
using Split.Map;

namespace Split.Player {
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

        public void MoveForward(InputAction.CallbackContext context) {
            if (!context.performed) return;

            Vector3 movePos;
            if (ValidateMovePosition(currentPosition.x + 1, currentPosition.y, out movePos)) {
                currentPosition.x++;
                movePos.y = this.transform.position.y;
                LeanTween.move(this.gameObject, movePos, moveSpeed);
            }
        }

        public void MoveBackward(InputAction.CallbackContext context) {
            if (!context.performed) return;

            Vector3 movePos;
            if (ValidateMovePosition(currentPosition.x - 1, currentPosition.y, out movePos)) {
                currentPosition.x--;
                movePos.y = this.transform.position.y;
                LeanTween.move(this.gameObject, movePos, moveSpeed);
            }
        }

        public void MoveLeft(InputAction.CallbackContext context) {
            if (!context.performed) return;

            Vector3 movePos;
            if (ValidateMovePosition(currentPosition.x, currentPosition.y - 1, out movePos)) {
                currentPosition.y--;
                movePos.y = this.transform.position.y;
                LeanTween.move(this.gameObject, movePos, moveSpeed);
            }
        }

        public void MoveRight(InputAction.CallbackContext context) {
            if (!context.performed) return;

            Vector3 movePos;
            if (ValidateMovePosition(currentPosition.x, currentPosition.y + 1, out movePos)) {
                currentPosition.y++;
                movePos.y = this.transform.position.y;
                LeanTween.move(this.gameObject, movePos, moveSpeed);
            }
        }

        private bool ValidateMovePosition(int x, int y, out Vector3 worldPos) {
            //Validates that specified position is in bounds of the board
            if ((x >= 0 && x < mapData.FieldSize.x) && (y >= 0 && y < mapData.FieldSize.y)) {
                //Validates that there is a panel at that position
                if (mapGenerator.Grid[x, y] != null) {
                    //TODO: Somehow determine that there's not a wall there in the future
                    worldPos = mapGenerator.Grid[x,y].TileObject.position;
                    return true;
                }
            }

            worldPos = Vector3.zero;
            return false;
        }
    }
}