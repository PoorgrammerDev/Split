using UnityEngine;

namespace Split.Builder.CameraStates {
    /// <summary>
    /// [Abstract] Represents all camera states that hold the isometric angle.
    /// </summary>
    public abstract class IsometricCam : CameraState {
        protected Camera camera;
        protected CameraFollow follow;

        public IsometricCam(CameraController controller) : base(controller) {
            this.camera = controller.Camera;
            this.follow = controller.Follow;
        }
        
        public override void Start() {
            camera.transform.rotation = Quaternion.Euler(30, -45, 0);
            follow.Offset = new Vector3(6, 4 ,-6); //TODO: maybe change for zooming support?
            follow.TeleportToPosition();
        }
    }
}