using UnityEngine;

namespace Split.Builder.CameraStates {
    /// <summary>
    /// Camera is in the isometric angle and the movement is not bound to any tiles.
    /// </summary>
    public class IsoFreeMove : IsometricCam {
        private Vector3 velocity;
        private float speed;

        public IsoFreeMove(CameraController controller, float speed) : base(controller) {
            this.velocity = Vector3.zero;
            this.speed = speed;
        }

        public override void Start() {
            base.Start();
            follow.Active = false;
        }

        public override void Move(Vector2Int vec) {
            FreeMove(vec);
        }

        public override void FreeMove(Vector2 vec) {
            if (vec == Vector2.zero) return;

            //TODO: this will eventually go into the ground and clip through
            //temporary fix right now is to set the camera really high, but this is a bad solution
            velocity = camera.transform.TransformDirection(new Vector3(vec.x, vec.y, 0f)) * speed * Time.deltaTime;

            camera.transform.position += velocity;
        }
    }
}