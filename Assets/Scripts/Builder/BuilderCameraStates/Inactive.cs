using UnityEngine;

namespace Split.Builder.CameraStates {
    /// <summary>
    /// Camera is inactive. Cannot move.
    /// </summary>
    public class Inactive : CameraState {
        public Inactive(CameraController controller) : base(controller) {}

        public override Vector2Int? GetPosition() {
            return null;
        }
    }
}