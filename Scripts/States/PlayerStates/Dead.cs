using Godot;

namespace ZoomToHome {
    public partial class Dead : State {
        private Player player;

        public override void _Ready() {
            player = parentBody as Player;
            player.Connect(Entity.SignalName.Damaged, Callable.From(() => OnDamage()));
        }

        public override void EnterState() {
            player.PlayAnimation("Hide");
            Input.MouseMode = Input.MouseModeEnum.Visible;
            player.ToggleCrouch(true);
            player.FloorSnapLength = 0.4f;
        }

        public override void ExitState() {
            player.FloorSnapLength = 0.1f;
            player.ToggleCrouch(false);
        }

        public override void ProcessInput(InputEvent inputEvent) {

        }

        public override void ProcessFrame(double delta) {

        }

        public override void ProcessPhysics(double delta) {
            player.ApplyForce(Vector3.Down * player.Gravity, isOneShot: false);
            player.SumForces();
            player.MoveAndSlide();
            player.Velocity *= 0.85f;
            player.AddCameraRotation(player.Velocity / 10);
        }

        private void OnDamage() {
            manager.ChangeState(this);
        }
    }
}