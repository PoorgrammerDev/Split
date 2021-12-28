using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Split.LevelLoading;
using Split.Tiles;

namespace Split.Player {

    /*
     * This class handles the multiple instances of the player, specifically switching between them 
     */

    public class PlayerManager : MonoBehaviour {
        [Header("References")]
        [SerializeField] private LevelGenerator levelGenerator;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private CameraFollow cameraFollow;
        [SerializeField] private PlayerColors[] colors;
        [SerializeField] private PlayerColors defaultColor;

        [Header("Settings")]
        [SerializeField] private int maxCount;

        private Player[] players;
        private int activePlayerIndex;

        private void Awake() {
            this.players = new Player[this.maxCount];

            //Instantiates all player instances
            for (int i = 0; i < maxCount; ++i) {
                players[i] = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, this.transform).GetComponent<Player>();
                players[i].name = ("Player " + i);
                players[i].Manager = this;
                players[i].Colors = (colors.Length > i) ? colors[i] : defaultColor;

                players[i].SetState(new State.Uninitialized(players[i]));
            }

            //Sets Player 0 as starting player
            cameraFollow.target = players[0].transform;
            this.activePlayerIndex = 0;
        }

        // Start is called before the first frame update
        private void Start() {
            //Activates Starting Player (0)
            players[0].SetState(new State.Active(players[0]));
            
            Vector3? retVal = levelGenerator.GetSpawnWorldPos();
            if (!retVal.HasValue) throw new System.Exception("Spawn Position empty: level data not loaded!");

            Vector3 spawnPosition = retVal.Value;
            spawnPosition.y = players[0].transform.localScale.y;
            players[0].transform.position = spawnPosition;
        }

        /*
            Input: Switching between player instances
        */
        public void SwitchToPlayer(int index) {
            if (index < 0 || index >= maxCount || index == activePlayerIndex) return;

            if (this.players[index].GetState().Activate()) {
                this.players[activePlayerIndex].GetState().Deactivate();

                cameraFollow.target = players[index].transform;
                this.activePlayerIndex = index;
            }
        }

        public bool ValidateMovePosition(Vector2Int pos) {
            //Check that it's in bounds
            if ((pos.x >= 0 && pos.x < levelGenerator.Grid.GetLength(0)) && (pos.y >= 0 && pos.y < levelGenerator.Grid.GetLength(1))) {
                //Check that there's a panel there
                if (levelGenerator.Grid[pos.x, pos.y] != null) {
                    Tile tile = levelGenerator.Grid[pos.x, pos.y];

                    //TODO: any way to make this more efficient?
                    //Checks if there's already a player there
                    foreach (Player player in players) {
                        if (player.Position.x == pos.x && player.Position.y == pos.y) {
                            return false;
                        }
                    }


                    //Checking if bridge tile
                    IBridgeTile bridge = tile as IBridgeTile;
                    if (bridge != null) {
                        return bridge.IsActive();
                    }

                    //Passes all checks - no abnormalities
                    return true;
                }
            }

            //Fails check
            return false;
        }


        //------------------------
        // Accessors
        //------------------------

        public LevelGenerator LevelGenerator => levelGenerator;
        
        public Player ActivePlayer {
            get {
                return this.players[this.activePlayerIndex];
            }
        }

        /*********************
        * ----- MOVING ----- *
        *********************/
        //NOTE: The directions on the Vectors don't match because the game is being viewed in a different angle
        public void MoveForward(InputAction.CallbackContext context) {
            if (!context.performed) return;
            ActivePlayer.GetState().Move(ActivePlayer.Position + Vector2Int.right);
        }

        public void MoveBackward(InputAction.CallbackContext context) {
            if (!context.performed) return;
            ActivePlayer.GetState().Move(ActivePlayer.Position + Vector2Int.left);
        }

        public void MoveLeft(InputAction.CallbackContext context) {
            if (!context.performed) return;
            ActivePlayer.GetState().Move(ActivePlayer.Position + Vector2Int.down);
        }

        public void MoveRight(InputAction.CallbackContext context) {
            if (!context.performed) return;
            ActivePlayer.GetState().Move(ActivePlayer.Position + Vector2Int.up);
        }

    }
}