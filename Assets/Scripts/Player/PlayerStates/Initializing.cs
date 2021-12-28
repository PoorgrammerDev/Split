using System.Collections;
using UnityEngine;

namespace Split.Player.State {
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

            player.SetState(active);
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

            //Flashes colors while waiting for player to move --- --- ---
            Material plyMat = player.GetComponent<Renderer>().material;
            float t = 0.0f;
            bool flip = false;

            while (!finishAnimation) {
                plyMat.color = Color.Lerp(player.Colors.ActiveColor, player.Colors.InitializingColor, t);

                t = Mathf.Clamp(t + (Time.deltaTime * 2f * (flip ? -1 : 1)), 0, 1);
                if (t == 1 || t == 0) {
                    flip = !flip;
                }

                yield return null;
            }
            //--- --- ---

            //restore last player's visibility
            lastRend.enabled = true;
        }


    }
}