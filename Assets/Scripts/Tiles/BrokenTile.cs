using UnityEngine;

namespace Split.Tiles {
    /// <summary>
    /// Tile that holds the Broken property (disappears once stepped off of)
    /// </summary>
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