using UnityEngine;
using Split.LevelLoading;

namespace Split.Tiles {
    /*
     * This class represents a tile that becomes a hole once stepped off
     */
     
    public class BrokenTile : TileEntity, IToggleable {
        private Properties.BrokenProperty brokenProperty;

        public BrokenTile(GameObject gameObject, int gridX, int gridY) : base(gameObject, gridX, gridY) {
            brokenProperty = new Properties.BrokenProperty(GridPosition, gameObject);
        }

        public bool IsActive() {
            return brokenProperty.Active;
        }



     
    }
}