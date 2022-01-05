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
            //TODO: fix bad solution
            levelGenerator = GameObject.FindObjectOfType<LevelGenerator>();
        }

        public override void Start() {
            //Color
            LeanTween.color(player.gameObject, player.Colors.LockedColor, 0.5f);
        }

        public override bool Activate() {
            if (levelGenerator == null) return false;

            Tile tile = levelGenerator.Grid[player.Position.x, player.Position.y];

            if (tile is IToggleable) {
                IToggleable bridgeTile = tile as IToggleable;

                if (bridgeTile.IsActive()) {
                    player.SetState(new Active(player));
                    return true;
                }
            }

            return false;
        }

    }
}