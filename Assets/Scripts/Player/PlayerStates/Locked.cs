using UnityEngine;
using Split.LevelLoading;
using Split.Tiles;

namespace Split.Player.State {
    public class Locked : PlayerState {
        private LevelGenerator levelGenerator;

        public Locked(Player player) : base(player) {
            //TODO: fix bad solution
            levelGenerator = GameObject.FindObjectOfType<LevelGenerator>();
        }

        public override void Start() {
            //Color
            LeanTween.color(player.gameObject, player.LockedColor, 0.5f);
        }

        public override void Activate() {
            if (levelGenerator == null) return;

            Tile tile = levelGenerator.Grid[player.Position.x, player.Position.y];

            if (tile is IBridgeTile) {
                IBridgeTile bridgeTile = tile as IBridgeTile;

                if (bridgeTile.IsActive()) {
                    player.SetState(new Active(player));
                }
            }

        }

    }
}