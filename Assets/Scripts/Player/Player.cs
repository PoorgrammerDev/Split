using UnityEngine;
using Split.Player.State;
using Unity.VectorGraphics;

namespace Split.Player {
    /// <summary>
    /// Holds information for one player instance
    /// </summary>
    public class Player : MonoBehaviour {
        public PlayerColors Colors {get; set;}
        public Vector2Int Position {get; set;}
        public PlayerManager Manager {get; set;}
        public SVGImage Icon {get; set;}

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

        //Used for LeanTween event
        public void SetIconColor(Color color) {
            Icon.color = color;
        }

    }
}


