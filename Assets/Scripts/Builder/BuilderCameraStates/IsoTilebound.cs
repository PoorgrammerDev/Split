using UnityEngine;

namespace Split.Builder.CameraStates {
    /// <summary>
    /// Camera is in the isometric angle and its movement bound to target a tile
    /// </summary>
    public class IsoTilebound : IsometricCam {
        private BuilderLevelLoader loader;
        private GameObject highlighter;
        private Vector2Int position;

        public IsoTilebound(CameraController controller, BuilderLevelLoader loader) : base(controller) {
            this.loader = loader;
            this.position = Vector2Int.zero;
            this.highlighter = follow.Target.gameObject;

            highlighter.transform.position = loader.GridToWorldPos(0, 0);
        }

        public override void Start() {
            base.Start();
            follow.Active = true;
            highlighter.SetActive(true);
        }

        public override void End() {
            base.End();
            highlighter.SetActive(false);
        }

        //NOTE: The directions on the Vectors don't match because the game is being viewed in a different angle
        public override void MoveForward() {
            Move(Vector2Int.right);
        }

        public override void MoveBackwards() {
            Move(Vector2Int.left);
        }

        public override void MoveLeft() {
            Move(Vector2Int.down);
        }

        public override void MoveRight() {
            Move(Vector2Int.up);
        }

        private void Move(Vector2Int vec) {
            Vector2Int newPos = position + vec;
            if (newPos.x >= 0 && newPos.y >= 0) {
                //Set position to the new position
                position = newPos;

                //Set camera target position and highlighter position to new location
                highlighter.transform.position = loader.GridToWorldPos(position.x, position.y);
            }
        }
    }
}