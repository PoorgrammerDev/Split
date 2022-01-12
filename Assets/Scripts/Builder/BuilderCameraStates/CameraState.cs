using UnityEngine;

namespace Split.Builder.CameraStates {
    /// <summary>
    /// [Abstract] Represents the different movement and viewing states the Builder camera can be in
    /// </summary>
    public abstract class CameraState {
        protected CameraController controller;

        public CameraState(CameraController controller) {
            this.controller = controller;
        }

        public virtual void Start() {}
        public virtual void End() {}
        
        public virtual void Move(Vector2Int vec) {}
        public virtual void FreeMove(Vector2 vec) {}

        public abstract Vector2Int? GetPosition();
    }
}