using UnityEngine;

namespace Split.Player.State {
    public class Initializing : PlayerState {

        public Initializing(Player player) : base(player) {
        }

        public override void Start() {
            //TODO: set active, teleport to current player, play flashing animation
        }

        public override void Move(Vector2Int to) {
            player.SetState(new Active(player));
            player.GetState().Move(to);
        }

        public override void Deactivate() {
            player.SetState(new Uninitialized(player));
        }


    }
}