using UnityEngine;

namespace Split.Builder.CameraStates {
    public class IsoFreeMove : IsometricCam {
        private const int KEY_MULTIPLIER = 100;

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

        public override void MoveForward() {
            MoveFreely(Vector2Int.up * KEY_MULTIPLIER);
        }

        public override void MoveBackwards() {
            MoveFreely(Vector2Int.down * KEY_MULTIPLIER);
        }

        public override void MoveLeft() {
            MoveFreely(Vector2Int.left * KEY_MULTIPLIER);
        }

        public override void MoveRight() {
            MoveFreely(Vector2Int.right * KEY_MULTIPLIER);
        }

        public override void MoveFreely(Vector2 vec) {
            velocity = camera.transform.TransformDirection(new Vector3(vec.x, vec.y, 0f)) * speed * Time.deltaTime;

            camera.transform.position += velocity;
        }
    }
}