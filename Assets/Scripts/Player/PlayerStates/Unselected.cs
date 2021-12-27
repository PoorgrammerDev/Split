using UnityEngine;

namespace Split.Player.State {
    public class Unselected : PlayerState {
        public Unselected(Player player) : base(player) {
        }

        public override void Start() {
            //Color
            LeanTween.color(player.gameObject, player.DeactivatedColor, 0.5f);
        }

        public override void Activate() {
            //Change this player's state to active
            player.SetState(new Active(player));
        }

        public override void Lock() {
            //Change state to locked
            player.SetState(new Locked(player));
        }

    }
}