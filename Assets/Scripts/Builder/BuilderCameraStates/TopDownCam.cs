using UnityEngine;

namespace Split.Builder.CameraStates {
    /// <summary>
    /// [Abstract] Represents all camera states with the flat Top-Down angle
    /// </summary>
    public abstract class TopDownCam : CameraState {
        protected Camera camera;
        protected CameraFollow follow;
        
        public TopDownCam(CameraController controller) : base(controller) {
            this.camera = controller.Camera;
            this.follow = controller.Follow;
        }

        public override void Start() {
            camera.transform.rotation = Quaternion.Euler(90, 0, 0);
            follow.Offset = controller.TopDownOffset;
            follow.TeleportToTarget();
        }
        
    }
}