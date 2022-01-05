using UnityEngine;

namespace Split.Player.State {
    /// <summary>
    /// "Normal Operating" state. Player instance is currently selected and being controlled.
    /// </summary>
    public class Active : PlayerState {
        public Active(Player player) : base(player) {
        }

        public override void Start() {
            player.gameObject.SetActive(true);
            LeanTween.color(player.gameObject, player.Colors.ActiveColor, 0.5f);
        }
        
        public override bool Move(Vector2Int to) {
            if (player.Manager.ValidateMovePosition(to)) {
                Vector2Int oldPosition = player.Position;

                //Update position
                player.Position = to;
                
                //Move player object
                Vector3 worldPos = player.Manager.LevelGenerator.GridToWorldPos(to.x, to.y);
                worldPos.y = player.transform.position.y; //Keep vertical position 
                LeanTween.move(player.gameObject, worldPos, 0.125f);

                //Fire Move Event
                GameEvents.current.PlayerMoveToTile(oldPosition, to);
                return true;
            }
            return false;
        }

        public override bool Deactivate() {
            player.SetState(new Unselected(player));
            return true;
        }

        public override bool Lock() {
            player.SetState(new Locked(player));
            //TODO: transfer control to a different player?
            return true;
        }
    }
}