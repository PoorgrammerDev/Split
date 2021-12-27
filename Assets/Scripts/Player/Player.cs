using UnityEngine;
using System.Collections.Generic;
using Split.Player.State;

namespace Split.Player {
    public class Player : MonoBehaviour {
        [Header("Colors")]
        [SerializeField] private Color activeColor;
        [SerializeField] private Color deactivatedColor;
        [SerializeField] private Color lockedColor;

        public Color ActiveColor => activeColor;
        public Color DeactivatedColor => deactivatedColor;
        public Color LockedColor => lockedColor;

        public Vector2Int Position {get; private set;}
        private PlayerState state;

        private void Awake() {
            SetState(new Uninitialized(this));
        }

        public PlayerState GetState() {
            return state;
        }

        public void SetState(PlayerState state) {
            this.state = state;
            this.state.Start();
        }


    }
}


