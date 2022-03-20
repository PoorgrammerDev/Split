using UnityEngine;

namespace Split.Builder.CameraStates {
    /// <summary>
    /// Camera is in the isometric angle and its movement bound to target a tile
    /// </summary>
    public class IsoTilebound : Tilebound {
        public IsoTilebound(CameraController controller, BuilderLevelLoader loader) : base(controller, loader) {}

        public override void Start() {
            Start(30, -45, 0, controller.IsometricOffset);
        }
    }
}