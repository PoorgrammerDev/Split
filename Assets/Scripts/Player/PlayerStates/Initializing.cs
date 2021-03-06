using System.Collections;
using UnityEngine;

namespace Split.Player.State {
    /// <summary>
    /// The state that bridges Uninitialized and Active.
    /// The player instance is being selected for the first time.
    /// </summary>
    public class Initializing : PlayerState {
        private Player lastPlayer;
        private bool finishAnimation = false;

        public Initializing(Player player) : base(player) {
        }

        public override void Start() {
            this.lastPlayer = player.Manager.ActivePlayer;

            //Set this player to be active
            player.gameObject.SetActive(true);

            //Teleport this player to the current player
            player.Position = lastPlayer.Position; // grid pos
            player.transform.position = lastPlayer.transform.position; // world pos

            //Begin flashing animation
            player.Manager.StartCoroutine(FlashingAnimation());
        }

        public override void End() {
            finishAnimation = true;
        }

        public override bool Move(Vector2Int to) {
            PlayerState active = new Active(player);
            bool output = active.Move(to);

            if (output) player.SetState(active);
            return output;
        }

        public override bool Deactivate() {
            player.SetState(new Uninitialized(player));
            return true;
        }

        private IEnumerator FlashingAnimation() {
            Renderer lastRend = lastPlayer.GetComponent<Renderer>();

            //Since both the last player and the new player are in the same spot,
            //make the old player invisible to prevent clipping
            lastRend.enabled = false;

            //TODO: Can probably be replaced with LeanTween

            //Flashes colors while waiting for player to move --- --- ---
            Material plyMat = player.GetComponent<Renderer>().material;
            float t = 0.0f;
            bool flip = false;

            //First, fade from old player color to activated new color
            while (t < 1.0f) {
                plyMat.color = Color.Lerp(lastPlayer.Colors.ActiveColor, player.Colors.ActiveColor, t);
                player.Icon.color = plyMat.color;
                
                t = Mathf.Clamp(t + (Time.deltaTime * 5f), 0, 1);
                yield return null;
            }

            //Then reset t to start and begin lerping cycle
            t = 0.0f;
            while (!finishAnimation) {
                plyMat.color = Color.Lerp(player.Colors.ActiveColor, player.Colors.InitializingColor, t);
                player.Icon.color = plyMat.color;

                t = Mathf.Clamp(t + (Time.deltaTime * 2f * (flip ? -1 : 1)), 0, 1);
                if (t == 1 || t == 0) {
                    flip = !flip;
                }

                yield return null;
            }
            //End flashing logic --- --- ---

            //restore last player's visibility
            lastRend.enabled = true;
        }

    }
}