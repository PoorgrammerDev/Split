using UnityEngine;
using Split.LevelLoading;
using Split.Tiles;

namespace Split.Player.State {
    /// <summary>
    /// Player instance is residing on a deactivated panel. This player cannot move,
    /// cannot be switched/controlled, etc. until the panel is reactivated.
    /// </summary>
    public class Locked : PlayerState {
        private LevelGenerator levelGenerator;

        public Locked(Player player) : base(player) {
            levelGenerator = player.Manager.LevelGenerator;
        }

        public override void Start() {
            //Color
            LeanTween.color(player.gameObject, player.Colors.LockedColor, 0.5f);
            LeanTween.value(player.Icon.gameObject, player.SetIconColor, player.Icon.color, player.Colors.LockedColor, 0.5f);
        }

        public override bool Unlock() {
            Tile tile = levelGenerator.Grid[player.Position.x, player.Position.y];

            if (tile is IToggleable) {
                IToggleable bridgeTile = tile as IToggleable;

                if (bridgeTile.IsActive()) {
                    player.SetState(new Unselected(player));
                    return true;
                }
            }

            return false;
        }

    }
}