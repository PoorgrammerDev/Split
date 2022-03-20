using UnityEngine;

namespace Split.Builder.CameraStates {
    /// <summary>
    /// [Abstract] Represents all camera states that hold the isometric angle.
    /// </summary>
    public abstract class Tilebound : CameraState {
        private BuilderLevelLoader loader;
        private GameObject highlighter;
        protected Camera camera;
        protected CameraFollow follow;
        private Vector2Int position;

        public Tilebound(CameraController controller, BuilderLevelLoader loader) : base(controller) {
            this.loader = loader;
            this.follow = controller.Follow;
            this.camera = controller.Camera;
            this.highlighter = follow.Target.gameObject;
            this.position = Vector2Int.zero;

            highlighter.transform.position = loader.GridToWorldPos(0, 0);
        }

        public void Start(int x, int y, int z, Vector3 cameraOffset) {
            camera.transform.rotation = Quaternion.Euler(x, y, z);
            follow.Offset = cameraOffset;
            follow.TeleportToTarget();

            follow.Active = true;
            highlighter.SetActive(true);
        }

        public override void End() {
            base.End();
            highlighter.SetActive(false);
        }

        public override void Move(Vector2Int vec) {
            //Flip X and Y for grid position
            vec = new Vector2Int(vec.y, vec.x);

            Vector2Int newPos = position + vec;
            if (newPos.x >= 0 && newPos.y >= 0) {
                //Set position to the new position
                position = newPos;

                //Set camera target position and highlighter position to new location
                highlighter.transform.position = loader.GridToWorldPos(position.x, position.y);

                //Expand level
                if (newPos.x >= loader.LevelData.gridData.Count || newPos.y >= loader.LevelData.gridData[0].Count) {
                	loader.AddSize(newPos.x - loader.LevelData.gridData.Count + 1, newPos.y - loader.LevelData.gridData[0].Count + 1);
				}
            }
        }

        public override Vector2Int? GetPosition() {
            return position;
        }


    }
}
