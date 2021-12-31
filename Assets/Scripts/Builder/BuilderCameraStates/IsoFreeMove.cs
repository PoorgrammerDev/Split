using UnityEngine;

namespace Split.Builder.CameraStates {
    public class IsoFreeMove : IsometricCam {
        private Vector3 velocity;

        public IsoFreeMove(Camera camera, CameraFollow follow) : base(camera, follow) {
            
        }

        public override void Move(Vector2 vec) {
            velocity = camera.transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"))) * 2f * Time.deltaTime;
        }
    }
}