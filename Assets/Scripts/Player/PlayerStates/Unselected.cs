using UnityEngine;

namespace Split.Player.State {
    /// <summary>
    /// Player instance has been switched off from.
    /// It exists, but is not currently being controlled.
    /// It can be switched back to at any time.
    /// </summary>
    public class Unselected : PlayerState {
        public Unselected(Player player) : base(player) {
        }

        public override void Start() {
            //Color
            LeanTween.color(player.gameObject, player.Colors.DeactivatedColor, 0.5f);
            LeanTween.value(player.Icon.gameObject, player.SetIconColor, player.Icon.color, player.Colors.DeactivatedColor, 0.5f);
        }

        public override bool Activate() {
            //Change this player's state to active
            player.SetState(new Active(player));
            return true;
        }

        public override bool Lock() {
            //Change state to locked
            player.SetState(new Locked(player));
            return true;
        }

    }
}