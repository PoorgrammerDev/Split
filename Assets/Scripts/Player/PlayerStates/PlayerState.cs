using UnityEngine;

namespace Split.Player.State {
    public abstract class PlayerState {
        protected Player player;
        
        public PlayerState(Player player) {
            this.player = player;
        }

        public virtual void Start() {}
        public virtual void End() {}

        public virtual bool Move(Vector2Int to) {
            return false;
        }

        public virtual bool Activate() {
            return false;
        }

        public virtual bool Deactivate() {
            return false;
        }

        public virtual bool Lock() {
            return false;
        }
    }
}
