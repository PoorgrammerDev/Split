using UnityEngine;

namespace Split.Builder.CameraStates {
    public class TopDownTilebound : Tilebound {
        public TopDownTilebound(CameraController camera, BuilderLevelLoader levelLoader) : base(camera, levelLoader) {}

        public override void Start() {
            base.Start(90, 0, 0, controller.TopDownOffset);
        }
    }
}