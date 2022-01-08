using UnityEngine;
using UnityEngine.InputSystem;
using Split.Builder.CameraStates;

namespace Split.Builder {
    /// <summary>
    /// Controls the movement of the camera
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(CameraFollow))]
    public class CameraController : MonoBehaviour {
        [Header("References")]
        [SerializeField] private BuilderLevelLoader loader;

        [Header("Options")]
        [SerializeField] private Vector3 isometricOffset;
        [SerializeField] private Vector3 topDownOffset;
        [SerializeField] private float tileMoveDelay;

        public Camera Camera {get; private set;}
        public CameraFollow Follow {get; private set;}
        public Vector3 IsometricOffset => isometricOffset;
        public Vector3 TopDownOffset => topDownOffset;

        private CameraState state;
        private bool freeMoveActivated;
        private Vector2Int gridMoveVec;
        private Vector2 freeMoveVec;
        private float delayCounter;

        private void Awake() {
            this.Camera = GetComponent<Camera>();
            this.Follow = GetComponent<CameraFollow>();
            this.freeMoveActivated = false;

            SetState(new Inactive(this));
        }

        private void Update() {
            this.state.FreeMove(freeMoveVec);

            //TODO: Find a better way to do this
            if (this.state is IsoTilebound) {
                if (delayCounter >= tileMoveDelay) {
                    if (gridMoveVec != Vector2Int.zero) {
                        this.state.Move(gridMoveVec);
                        delayCounter = 0;
                    }
                }
                else {
                    delayCounter += Time.deltaTime;
                }
            }
            else {
                this.state.Move(gridMoveVec);
            }
        }

        /*************************
        *         MOVING         *
        *************************/
    
        public void Move(InputAction.CallbackContext context) {
            if (!context.performed) return;
            Vector2 input = context.ReadValue<Vector2>();
            this.gridMoveVec = new Vector2Int((int) input.x, (int) input.y);
        }

        public void FreeMove(InputAction.CallbackContext context) {
            if (freeMoveActivated && context.performed) {
                Debug.Log((-context.ReadValue<Vector2>()).ToString());
                this.freeMoveVec = -context.ReadValue<Vector2>();
            }
            else {
                this.freeMoveVec = Vector2.zero;
            }
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

        /**************************
        *      CAMERA STATES      *
        **************************/

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