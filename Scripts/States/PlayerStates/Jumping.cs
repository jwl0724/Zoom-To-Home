using Godot;

namespace ZoomToHome {
    public partial class Jumping : State {
        private Player player;

        public override void _Ready() {
            player = parentBody as Player;
        }

        public override void EnterState() {
            if (manager.PreviousState is Zipping || manager.PreviousState is Swinging) return;
            if (manager.PreviousState is Crouch)
                player.ApplyForce(Vector3.Up * (player.JumpSpeed + player.Velocity.Length() / 4), isOneShot: true);
            else player.ApplyForce(Vector3.Up * player.JumpSpeed, isOneShot: true);
        }

        public override void ExitState() {
            
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (Input.IsActionJustPressed("zip")) manager.ChangeState(manager.AllStates["Zipping"]);
            if (Input.IsActionJustPressed("swing")) manager.ChangeState(manager.AllStates["Swinging"]);
        }

        public override void ProcessFrame(double delta) {

        }

        public override void ProcessPhysics(double delta) {
            player.ApplyMidAirInputs(player.MoveSpeed * 1.5f);
            player.ApplyForce(Vector3.Down * player.Gravity, isOneShot: false);
            player.SumForces();
            player.MoveAndSlide();

            if (player.Velocity.Y < 0) {
                manager.ChangeState(manager.AllStates["Falling"]);
                return;
            }
            if (player.IsOnFloor()) {
                if (Input.IsActionPressed("crouch")) manager.ChangeState(manager.AllStates["Crouch"]);
                else manager.ChangeState(manager.AllStates["Recovering"]);
                return;
            }

            if (Input.IsActionPressed("jump") && player.CanVault()) {
                manager.ChangeState(manager.AllStates["Vaulting"]);
                return;
            }
            
            float velocityMagnitude = player.GetForwardVelocityHorizontalMagnitude();
            if (player.IsOnWallOnly() && velocityMagnitude > player.MoveSpeed * 1.3f)
                manager.ChangeState(manager.AllStates["WallRunning"]);
        }
    }
}