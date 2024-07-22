using Godot;

namespace ZoomToHome {
    public partial class Dead : State {
        private Player player;

        public override void _Ready() {
            player = parentBody as Player;
            player.Connect(Entity.SignalName.Damaged, Callable.From(() => OnDamage()));
        }

        public override void EnterState() {
            // ADD STUFF WHEN VISUALS ARE IMPLEMENTED
        }

        public override void ExitState() {
            // ADD STUFF WHEN VISUALS ARE IMPLEMENTED
        }

        public override void ProcessInput(InputEvent inputEvent) {

        }

        public override void ProcessFrame(double delta) {

        }

        public override void ProcessPhysics(double delta) {
            player.ApplyForce(Vector3.Down * player.Gravity, isOneShot: false);
            player.SumForces();
            player.MoveAndSlide();
        }

        private void OnDamage() {
            manager.ChangeState(this);
        }
    }
}