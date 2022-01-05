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
        
        public virtual void MoveForward() {}
        public virtual void MoveBackwards() {}
        public virtual void MoveLeft() {}
        public virtual void MoveRight() {}
        public virtual void MoveFreely(Vector2 vec) {}
    }
}