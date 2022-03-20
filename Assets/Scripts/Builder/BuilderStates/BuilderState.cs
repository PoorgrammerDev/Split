using UnityEngine;

namespace Split.Builder.States {
    public abstract class BuilderState {
        protected BuilderManager manager;
        protected CameraController camera;
        protected BuilderLevelLoader loader;

        public BuilderState(BuilderManager manager, CameraController camera, BuilderLevelLoader loader) {
            this.manager = manager;
            this.camera = camera;
            this.loader = loader;
        }

        public virtual void Start() {}
        public virtual void End() {}

        public virtual void SetPosition (Vector2Int pos) {}

        public virtual bool PlaceKey() {
            return false;
        }

        public virtual bool ShiftPlaceKey() {
            return false;
        }


    }
}