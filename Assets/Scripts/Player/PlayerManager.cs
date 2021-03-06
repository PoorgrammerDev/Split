using UnityEngine;
using Split.LevelLoading;
using Split.Tiles;

namespace Split.Player {
    /// <summary>
    /// This class handles the multiple instances of the player, specifically switching between them 
    /// </summary>
    public class PlayerManager : MonoBehaviour {
        [Header("References")]
        [SerializeField] private LevelGenerator levelGenerator;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private CameraFollow cameraFollow;
        [SerializeField] private PlayerColors[] colors;
        [SerializeField] private PlayerColors defaultColor;
        [SerializeField] private GameUIManager uiManager;

        private Player[] players;
        private int activePlayerIndex;

        /*********************
        *     ACCESSSORS     *
        *********************/

        public LevelGenerator LevelGenerator => levelGenerator;
        public Player ActivePlayer => this.players[this.activePlayerIndex];

        /******************
        *     METHODS     *
        ******************/

        private void OnEnable() {
            this.players = new Player[levelGenerator.LevelData.maxPlayers];

            //Instantiates all player instances
            for (int i = 0; i < levelGenerator.LevelData.maxPlayers; ++i) {
                players[i] = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, this.transform).GetComponent<Player>();
                players[i].name = ("Player " + i);
                players[i].Manager = this;
                players[i].Colors = (colors.Length > i) ? colors[i] : defaultColor;
                players[i].Icon = uiManager.PlayerIcons[i];

                players[i].SetState(new State.Uninitialized(players[i]));
            }

            //Sets Player 0 as starting player
            cameraFollow.Target = players[0].transform;
            this.activePlayerIndex = 0;

            GameEvents.current.onBridgeActivate += UnlockPlayer;
            GameEvents.current.onBridgeDeactivate += LockPlayer;
        }

        // Start is called before the first frame update
        private void Start() {
            //Activates Starting Player (0)
            players[0].SetState(new State.Active(players[0]));
            players[0].Position = levelGenerator.LevelData.startPosition;
            
            Vector3? retVal = levelGenerator.GetSpawnWorldPos();
            if (!retVal.HasValue) throw new System.Exception("Spawn Position empty: level data not loaded!");

            Vector3 spawnPosition = retVal.Value;
            spawnPosition.y = players[0].transform.localScale.y;
            players[0].transform.position = spawnPosition;
        }

        /// <summary>
        /// Input: Switching between player instances
        /// </summary>
        /// <param name="index">Player Number to switch to</param>
        public void SwitchToPlayer(int index) {
            if (index < 0 || index >= levelGenerator.LevelData.maxPlayers || index == activePlayerIndex) return;

            if (this.players[index].GetState().Activate()) {
                this.players[activePlayerIndex].GetState().Deactivate();

                cameraFollow.Target = players[index].transform;
                this.activePlayerIndex = index;
            }
        }

        /// <summary>
        /// Checks if the desired position is a valid spot for the player to be in
        /// </summary>
        /// <param name="pos">Grid Position</param>
        public bool ValidateMovePosition(Vector2Int pos) {
            //Check that it's in bounds
            if ((pos.x >= 0 && pos.x < levelGenerator.Grid.GetLength(0)) && (pos.y >= 0 && pos.y < levelGenerator.Grid.GetLength(1))) {
                //Check that there's a panel there
                if (levelGenerator.Grid[pos.x, pos.y] != null) {
                    Tile tile = levelGenerator.Grid[pos.x, pos.y];

                    if (GetPlayerAtPosition(pos) != null) return false;

                    //Checking if bridge tile
                    IToggleable bridge = tile as IToggleable;
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

        /// <summary>
        /// Returns the player standing on the specified position
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Player GetPlayerAtPosition(Vector2Int pos) {
            //Searches all players and tests their positions
            foreach (Player player in players) {
                if (player.Position.x == pos.x && player.Position.y == pos.y && !(player.GetState() is State.Uninitialized)) {
                    return player;
                }
            }
            return null;
        }

        /// <summary>
        /// Triggered by Bridge Deactivate Event: locks any players on top of it
        /// </summary>
        /// <param name="pos"></param>
        private void LockPlayer(Vector2Int pos) {
            Player player = GetPlayerAtPosition(pos);
            if (player != null) {
                player.GetState().Lock();
            }
        }

        /// <summary>
        /// Triggered by Bridge Activate Event: unlocks any locked players on top of it
        /// </summary>
        /// <param name="pos"></param>
        private void UnlockPlayer(Vector2Int pos) {
            Player player = GetPlayerAtPosition(pos);
            if (player != null) {
                player.GetState().Unlock();
            }
        }

    }
}