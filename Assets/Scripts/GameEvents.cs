using System;
using UnityEngine;

namespace Split {
    /// <summary>
    /// Handles Events in the Game Scene
    /// </summary>
    public class GameEvents : MonoBehaviour {
        //Singleton reference
        public static GameEvents current;

        private void Awake() {
            current = this;
        }

        public event Action<Vector2Int, Vector2Int> playerMoveToTile;
        public void PlayerMoveToTile(Vector2Int from, Vector2Int to) {
            if (playerMoveToTile != null) {
                playerMoveToTile(from, to);
            }
        }

        public event Action<Vector2Int> onButtonActivate; 
        public void ButtonActivate(Vector2Int buttonPosition) {
            if (onButtonActivate != null) {
                onButtonActivate(buttonPosition);
            }
        }

        public event Action<Vector2Int> onButtonDeactivate; 
        public void ButtonDeactivate(Vector2Int buttonPosition) {
            if (onButtonDeactivate != null) {
                onButtonDeactivate(buttonPosition);
            }
        }

        public event Action<Vector2Int> onBridgeActivate; 
        public void BridgeActivate(Vector2Int bridgePosition) {
            if (onBridgeActivate != null) {
                onBridgeActivate(bridgePosition);
            }
        }

        public event Action<Vector2Int> onBridgeDeactivate; 
        public void BridgeDeactivate(Vector2Int bridgePosition) {
            if (onBridgeDeactivate != null) {
                onBridgeDeactivate(bridgePosition);
            }
        }
    }
}