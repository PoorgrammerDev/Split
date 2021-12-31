using UnityEngine;

namespace Split.Builder.CameraStates {
    public abstract class IsometricCam : CameraState {
        protected CameraFollow follow;
        public IsometricCam(Camera camera, CameraFollow follow) : base(camera) {
            this.follow = follow;
        }
        
        public override void Start() {
            camera.transform.rotation = Quaternion.Euler(30, -45, 0);
            follow.Offset = new Vector3(6, 4 ,-6); //TODO: maybe change for zooming support?
            follow.TeleportToPosition();
        }
    }
}