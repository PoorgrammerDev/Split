namespace Split.Player.State {
    public class Uninitialized : PlayerState {

        public Uninitialized(Player player) : base(player) {
        }

        public override void Start() {
            player.gameObject.SetActive(false);
        }

        public override void Activate() {
            player.SetState(new Initializing(player));
        }

    }
}