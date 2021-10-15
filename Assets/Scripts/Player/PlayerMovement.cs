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
        [SerializeField] private PlayerManager playerSwitcher;

        [Header("Settings")]
        [SerializeField] private float moveSpeed;

        private Vector2Int[] currentPosition;

        // Start is called before the first frame update
        void Start() {
            playerSwitcher = GetComponent<PlayerManager>();
            currentPosition = new Vector2Int[playerSwitcher.MaxCount];

            for (int i = 0; i < currentPosition.Length; ++i) {
                currentPosition[i] = mapData.SpawnPosition;
            }
        }

        //NOTE: The directions on the Vectors don't match because the game is being viewed in a different angle
        public void MoveForward(InputAction.CallbackContext context) {
            if (!context.performed) return;
            MoveToPosition(currentPosition[playerSwitcher.ActivePlayerIndex] + Vector2Int.right);
        }

        public void MoveBackward(InputAction.CallbackContext context) {
            if (!context.performed) return;
            MoveToPosition(currentPosition[playerSwitcher.ActivePlayerIndex] + Vector2Int.left);
        }

        public void MoveLeft(InputAction.CallbackContext context) {
            if (!context.performed) return;
            MoveToPosition(currentPosition[playerSwitcher.ActivePlayerIndex] + Vector2Int.down);
        }

        public void MoveRight(InputAction.CallbackContext context) {
            if (!context.performed) return;
            MoveToPosition(currentPosition[playerSwitcher.ActivePlayerIndex] + Vector2Int.up);
        }

        private void MoveToPosition(Vector2Int newPosition) {
            Vector3 moveWorldPos;
            if (ValidateMovePosition(newPosition.x, newPosition.y, out moveWorldPos)) {
                Vector2Int oldPosition = currentPosition[playerSwitcher.ActivePlayerIndex];

                //Update position and move player
                currentPosition[playerSwitcher.ActivePlayerIndex] = newPosition;
                moveWorldPos.y = playerSwitcher.ActivePlayer.transform.position.y;
                LeanTween.move(playerSwitcher.ActivePlayer, moveWorldPos, moveSpeed);

                //Fire Move Event
                GameEvents.current.PlayerMoveToTile(oldPosition, currentPosition[playerSwitcher.ActivePlayerIndex]);
            }
        }

        private bool ValidateMovePosition(int x, int y, out Vector3 worldPos) {
            //Validates that specified position is in bounds of the board
            if ((x >= 0 && x < mapData.FieldSize.x) && (y >= 0 && y < mapData.FieldSize.y)) {
                //Validates that there is a panel at that position
                if (mapGenerator.Grid[x, y] != null) {
                    Tile tile = mapGenerator.Grid[x, y];

                    //Checking if there's already a player there
                    foreach (Vector2Int pos in this.currentPosition) {
                        if (pos.x == x && pos.y == y) {
                            worldPos = Vector3.zero;
                            return false;
                        }
                    }
                    
                    //Testing Special Tiles
                    if (tile is BridgeTile) {
                        BridgeTile bridge = tile as BridgeTile;

                        worldPos = mapGenerator.Grid[x,y].TileObject.position;
                        return bridge.Activated;
                    }

                    worldPos = mapGenerator.Grid[x,y].TileObject.position;
                    return true;
                }
            }

            worldPos = Vector3.zero;
            return false;
        }
    }
}