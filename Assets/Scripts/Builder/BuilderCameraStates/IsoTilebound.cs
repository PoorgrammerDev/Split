using UnityEngine;
using System.Collections;

namespace Split.Builder.CameraStates {
    /// <summary>
    /// This type of Builder camera is bound to target a tile
    /// </summary>
    public class IsoTilebound : IsometricCam {
        private BuilderLevelLoader loader;
        private Vector2Int position;

        public IsoTilebound(Camera camera, BuilderLevelLoader loader, CameraFollow follow) : base(camera, follow) {
            this.loader = loader;
            this.position = Vector2Int.zero;
        }

        public override void Move(Vector2 vec) {
            Move(new Vector2Int((int) vec.x, (int) vec.y));
        }

        private void Move(Vector2Int vec) {
            position += vec;
            follow.Target.transform.position = loader.GridToWorldPos(vec.x, vec.y);
        }
    }
}