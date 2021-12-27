using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Split.Player.State;

namespace Split.Player {
    public class Player : MonoBehaviour {
        [Header("Colors")]
        [SerializeField] private Color activeColor;
        [SerializeField] private Color deactivatedColor;
        [SerializeField] private Color lockedColor;

        public Color ActiveColor => activeColor;
        public Color DeactivatedColor => deactivatedColor;
        public Color LockedColor => lockedColor;

        public Vector2Int Position {get; set;}
        public PlayerManager Manager {get; set;}
        private PlayerState state;

        private void Awake() {
            SetState(new Uninitialized(this));
        }

        /********************
        * ----- STATE ----- *
        ********************/

        public PlayerState GetState() {
            return state;
        }

        public void SetState(PlayerState state) {
            this.state = state;
            this.state.Start();
        }
        
        /*********************
        * ----- MOVING ----- *
        *********************/
        //NOTE: The directions on the Vectors don't match because the game is being viewed in a different angle
        public void MoveForward(InputAction.CallbackContext context) {
            if (!context.performed) return;
            state.Move(Position + Vector2Int.right);
        }

        public void MoveBackward(InputAction.CallbackContext context) {
            if (!context.performed) return;
            state.Move(Position + Vector2Int.left);
        }

        public void MoveLeft(InputAction.CallbackContext context) {
            if (!context.performed) return;
            state.Move(Position + Vector2Int.down);
        }

        public void MoveRight(InputAction.CallbackContext context) {
            if (!context.performed) return;
            state.Move(Position + Vector2Int.up);
        }

    }
}


