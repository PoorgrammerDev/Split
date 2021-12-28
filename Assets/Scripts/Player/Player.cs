using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Split.Player.State;

namespace Split.Player {
    public class Player : MonoBehaviour {
        public PlayerColors Colors {get; set;}
        public Vector2Int Position {get; set;}
        public PlayerManager Manager {get; set;}

        private PlayerState state;

        /********************
        * ----- STATE ----- *
        ********************/

        public PlayerState GetState() {
            return state;
        }

        public void SetState(PlayerState state) {
            if (this.state != null) this.state.End();
            
            this.state = state;
            this.state.Start();
        }

    }
}


