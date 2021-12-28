using UnityEngine;

namespace Split.Player.State {
    public class Uninitialized : PlayerState {

        public Uninitialized(Player player) : base(player) {
        }

        public override void Start() {
            player.gameObject.SetActive(false);
        }

        public override bool Activate() {
            player.SetState(new Initializing(player));
            return true;
        }

    }
}