using UnityEngine;

namespace Split.Builder.CameraStates {
    public abstract class TopDownCam : CameraState {
        protected CameraFollow follow;
        
        public TopDownCam(Camera camera, CameraFollow follow) : base(camera) {
            this.follow = follow;
        }

        public override void Start() {
            camera.transform.rotation = Quaternion.Euler(90, 0, 0);
            follow.Offset = new Vector3(0, 10, 0);
            follow.TeleportToPosition();
        }
        
    }
}