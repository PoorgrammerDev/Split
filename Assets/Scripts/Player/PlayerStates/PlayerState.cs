using UnityEngine;

namespace Split.Player.State {
    public abstract class PlayerState {
        protected Player player;
        
        public PlayerState(Player player) {
            this.player = player;
        }

        public virtual void Start() {}
        public virtual void Move(Vector2Int to) {}
        public virtual void Activate() {}
        public virtual void Deactivate() {}
        public virtual void Lock() {}
    }
}
