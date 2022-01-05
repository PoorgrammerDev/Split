using UnityEngine;

namespace Split.Player.State {
    /// <summary>
    /// Player instance has not been switched to/controlled yet. (Does not exist yet)
    /// </summary>
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