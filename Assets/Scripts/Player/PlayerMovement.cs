using UnityEngine;
using UnityEngine.InputSystem;

namespace Split.Player {
    public class PlayerMovement : MonoBehaviour {
        [Header("References")]
        [SerializeField] private PlayerManager manager;
        [SerializeField] private float moveDelay;

        private Vector2Int moveVec;
        public float counter;

        private void Awake() {
            counter = moveDelay;
        }

        private void Update() {
            if (!manager.ActivePlayer.GetState().IsMoving()) {
                if (counter >= moveDelay) {
                    if (this.moveVec != Vector2Int.zero) {
                        manager.ActivePlayer.GetState().Move(manager.ActivePlayer.Position + this.moveVec);
                        counter = 0;
                    }
                }
                else {
                    counter += Time.deltaTime;
                }
            }
        }
        
        //Flip Y and X due to how grid is set up
        public void Move(InputAction.CallbackContext context) {
            Vector2 input = context.ReadValue<Vector2>();
            this.moveVec = new Vector2Int((int) input.y, (int) input.x);
        }
    }
}