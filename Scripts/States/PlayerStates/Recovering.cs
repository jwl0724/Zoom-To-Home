using Godot;
using System;

namespace ZoomToHome {
    // TODO: CONNECT FALLING AND JUMPING STATES TO RECOVERY
    // TODO: IMPLEMENT CROUCH (FOR SLIDING MECHS)
    // TODO: IMPLEMENT SWINGING
    public partial class Recovering : State {
        private Player player;

        public override void _Ready() {
            player = parentBody as Player;
        }

        public override void EnterState() {
            
        }

        public override void ExitState() {
            
        }

        public override void ProcessFrame(double delta) {
            
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (Input.IsActionJustPressed("jump")) manager.ChangeState(manager.AllStates["Jumping"]);
            if (Input.IsActionJustPressed("zip")) manager.ChangeState(manager.AllStates["Zipping"]);
            // if (Input.IsActionJustPressed("swing")) manager.ChangeState(manager.AllStates["Swinging"]);
        }

        public override void ProcessPhysics(double delta) {
            player.Velocity *= player.FrictionCoefficient;
            player.MoveAndSlide();
            
            if (!player.IsOnFloor()) {
                manager.ChangeState(manager.AllStates["Falling"]);
                return;
            }

            if (player.Velocity.Length() > player.MoveSpeed) return;
            if (Input.GetVector("left", "right", "forward", "backward").IsZeroApprox()) {
                if (Input.IsActionPressed("sprint")) manager.ChangeState(manager.AllStates["Sprinting"]);
                else manager.ChangeState(manager.AllStates["Running"]);
            }
            if (player.Velocity.IsZeroApprox()) manager.ChangeState(manager.AllStates["Idle"]);
        }
    }
}
