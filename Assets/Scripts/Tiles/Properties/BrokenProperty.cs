using UnityEngine;

namespace Split.Tiles.Properties {
    public class BrokenProperty : ITileProperty {
        private Vector2Int position;
        private GameObject gameObject;
        public bool Active {get; private set;}

        public BrokenProperty(Vector2Int position, GameObject gameObject) {
            this.Active = true;
            this.position = position;
            this.gameObject = gameObject;

            GameEvents.current.playerMoveToTile += OnLeaveTile;
        }

        private void OnLeaveTile(Vector2Int from, Vector2Int to) {
            if (from.Equals(this.position)) {
                //TODO: play breaking animation

                this.Active = false;
                this.gameObject.SetActive(false);
                GameEvents.current.BridgeDeactivate(this.position); //fire bridge deactivate event
            }
        }
    }
}