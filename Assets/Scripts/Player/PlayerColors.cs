using UnityEngine;

namespace Split.Player {
    //TODO: Change this to Materials instead of Colors for more flexibility
    /// <summary>
    /// Different Colors for different states
    /// </summary>
    [System.Serializable]
    public struct PlayerColors {
        public Color ActiveColor;
        public Color InitializingColor;
        public Color DeactivatedColor;
        public Color LockedColor;
    }
}