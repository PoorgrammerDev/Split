using UnityEngine;
using System.Collections;

namespace Split.Builder.CameraStates {
    public abstract class CameraState {
        protected Camera camera;

        public CameraState(Camera camera) {
            this.camera = camera;
        }

        public virtual void Start() {}
        public virtual void End() {}
        public virtual void Move(Vector2 vec) {}
    }
}