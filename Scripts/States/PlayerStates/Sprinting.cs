
using Godot;

namespace ZoomToHome {
    public partial class Sprinting : State {
        private Player player;
        public override void _Ready() {
            player = parentBody as Player;
        }
        public override void EnterState() {
            
        }

        public override void ExitState() {
            
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (Input.IsActionJustReleased("sprint")) {
                if (Input.GetVector("left", "right", "forward", "backward").IsZeroApprox())
                    manager.ChangeState(manager.AllStates["Idle"]);
                else manager.ChangeState(manager.AllStates["Running"]); 
            }
            if (Input.IsActionJustPressed("jump")) manager.ChangeState(manager.AllStates["Jumping"]);
        }

        public override void ProcessFrame(double delta) {

        }

        public override void ProcessPhysics(double delta) {
            player.SetVelocityToInputVector(player.MoveSpeed * player.SprintMultiplier);
            player.ApplyForce(Vector3.Down * player.Gravity, isOneShot: false);
            player.SumForces();
            player.MoveAndSlide();

            if (Input.GetVector("left", "right", "forward", "backward").IsZeroApprox() && player.IsOnFloor()) 
                manager.ChangeState(manager.AllStates["Idle"]);
            if (!player.IsOnFloor()) manager.ChangeState(manager.AllStates["Falling"]);
        }
    }

}
