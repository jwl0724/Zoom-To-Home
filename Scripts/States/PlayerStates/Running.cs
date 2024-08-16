using Godot;

namespace ZoomToHome {
    public partial class Running : State {
        private Player player;
        public override void _Ready() {
            player = parentBody as Player;
        }
        
        public override void EnterState() {
            player.FloorSnapLength = 1;
            player.PlayAnimation("Reset");
        }

        public override void ExitState() {
            player.FloorSnapLength = 0.1f;
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (Input.IsActionJustPressed("sprint")) manager.ChangeState(manager.AllStates["Sprinting"]);
            if (Input.IsActionJustPressed("jump")) manager.ChangeState(manager.AllStates["Jumping"]);
            if (Input.IsActionJustPressed("zip")) manager.ChangeState(manager.AllStates["Zipping"]);
            if (Input.IsActionJustPressed("swing")) manager.ChangeState(manager.AllStates["Swinging"]);
            if (Input.IsActionJustPressed("crouch")) manager.ChangeState(manager.AllStates["Crouch"]);
        }

        public override void ProcessFrame(double delta) {
            if (!player.IsOnFloor()) {
                if (player.Velocity.Y > 0) manager.ChangeState(manager.AllStates["Falling"]);
                else  manager.ChangeState(manager.AllStates["Jumping"]);
            }
            Vector2 inputVector = Input.GetVector("left", "right", "forward", "backward");
            if (inputVector.IsZeroApprox()) manager.ChangeState(manager.AllStates["Idle"]);
            else if (Input.IsActionPressed("sprint")) manager.ChangeState(manager.AllStates["Sprinting"]);
        }

        public override void ProcessPhysics(double delta) {
            player.SetVelocityToInputVector(player.MoveSpeed);
            player.ApplyForce(Vector3.Down * player.Gravity, isOneShot: false);
            player.SumForces();
            player.MoveAndSlide();

            if (Input.GetVector("left", "right", "forward", "backward").IsZeroApprox() && player.IsOnFloor())
                manager.ChangeState(manager.AllStates["Idle"]);
            if (!player.IsOnFloor()) manager.ChangeState(manager.AllStates["Falling"]);
        }
    }
}
