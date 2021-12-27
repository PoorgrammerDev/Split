using UnityEngine;

namespace Split.Player.State {
    public class Active : PlayerState {
        public Active(Player player) : base(player) {
        }

        public override void Start() {
            LeanTween.color(player.gameObject, player.ActiveColor, 0.5f);
        }
        
        public override void Move(Vector2Int to) {
            if (player.Manager.ValidateMovePosition(to)) {
                Vector2Int oldPosition = player.Position;

                //Update position
                player.Position = to;
                
                //Move player object
                Vector3 worldPos = player.Manager.LevelGenerator.GridToWorldPos(to.x, to.y);
                worldPos.y = player.transform.position.y; //Keep vertical position 
                LeanTween.move(player.gameObject, worldPos, 0.5f);

                //Fire Move Event
                GameEvents.current.PlayerMoveToTile(oldPosition, to);
            }
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