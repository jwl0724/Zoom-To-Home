using Godot;

namespace ZoomToHome {
    public partial class Idle : State {
        private Player player;
        public override void _Ready() {
            player = parentBody as Player;
        }

        public override void EnterState() {
            player.Velocity = new Vector3(0, player.Velocity.Y, 0);
        }

        public override void ExitState() {
            // TO BE IMPLEMENTED WHEN VISUALS ARE ADDED
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (!player.IsOnFloor() || manager.CurrentState is Dead) return;
            if (!Input.GetVector("left", "right", "forward", "backward").IsZeroApprox()) {
                if (Input.IsActionPressed("sprint"))
                    manager.ChangeState(manager.AllStates["Sprinting"]);
                else manager.ChangeState(manager.AllStates["Running"]);
            }
            if (Input.IsActionJustPressed("jump")) manager.ChangeState(manager.AllStates["Jumping"]);
            if (Input.IsActionJustPressed("zip")) manager.ChangeState(manager.AllStates["Zipping"]);
        }

        public override void ProcessFrame(double delta) {
        
        }

        public override void ProcessPhysics(double delta) {
            player.ApplyForce(Vector3.Down * player.Gravity, isOneShot: false);
            player.SumForces();
            player.MoveAndSlide();
            
            if (!player.IsOnFloor() && manager.CurrentState is not Dead)
                manager.ChangeState(manager.AllStates["Falling"]);
        }
    }
}
