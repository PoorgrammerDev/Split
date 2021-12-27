using UnityEngine;

namespace Split.Player.State {
    public class Active : PlayerState {
        public Active(Player player) : base(player) {
        }

        public override void Start() {
            LeanTween.color(player.gameObject, player.ActiveColor, 0.5f);
        }
        
        public override void Move() {

        }

        public override void Deactivate() {
            player.SetState(new Unselected(player));
        }

        public override void Lock() {
            player.SetState(new Locked(player));
            //TODO: transfer control to a different player?
        }

    }
}