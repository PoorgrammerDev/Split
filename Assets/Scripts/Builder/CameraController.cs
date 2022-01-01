using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Split.Builder.CameraStates;

namespace Split.Builder {
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(CameraFollow))]
    public class CameraController : MonoBehaviour {
        [Header("References")]
        [SerializeField] private BuilderLevelLoader loader;
        [SerializeField] private GameObject tileHighlighter;

        private CameraState state;
        private bool freeMoveActivated;

        public Camera Camera {get; private set;}
        public CameraFollow Follow {get; private set;}
        public GameObject TileHighlighter => tileHighlighter;

        private void Awake() {
            this.Camera = GetComponent<Camera>();
            this.Follow = GetComponent<CameraFollow>();
            this.freeMoveActivated = false;

            SetState(new Inactive(this));
        }

        /*******
         MOVING
        *******/
    
        public void MoveForward(InputAction.CallbackContext context) {
            if (!context.performed) return;
            this.state.MoveForward();
        }

        public void MoveBackwards(InputAction.CallbackContext context) {
            if (!context.performed) return;
            this.state.MoveBackwards();
        }

        public void MoveLeft(InputAction.CallbackContext context) {
            if (!context.performed) return;
            this.state.MoveLeft();
        } 

        public void MoveRight(InputAction.CallbackContext context) {
            if (!context.performed) return;
            this.state.MoveRight();
        } 

        public void Move(InputAction.CallbackContext context) {
            if (freeMoveActivated && context.performed) this.state.MoveFreely(-context.ReadValue<Vector2>());
        } 

        public void ToggleFreeMove (InputAction.CallbackContext context) {
            if (context.performed) {
                Cursor.visible = false;
                this.freeMoveActivated = true;
            }
            else if (context.canceled) {
                Cursor.visible = true;
                this.freeMoveActivated = false;
            }
        }



        public CameraState GetState() {
            return state;
        }

        public void SetState(CameraState newState) {
            if (this.state != null) this.state.End();

            this.state = newState;
            this.state.Start();
        }

    }
}   