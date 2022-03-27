using System;
using UnityEngine;
using Split.LevelLoading;

namespace Split {
    /// <summary>
    /// Handles Events in the Game Scene
    /// </summary>
    public class MenuEvents : MonoBehaviour {
        //Singleton reference
        public static MenuEvents current;

        private void Awake() {
            current = this;
        }

        public event Action<LevelEntry> onLevelEntryPress;
        public void OnLevelEntryPress(LevelEntry entry) {
            if (onLevelEntryPress != null) {
                onLevelEntryPress(entry);
            }
        }
    }
}