using Godot;
using System;

namespace ZoomToHome {
    public partial class Crouch : State {
        private Player player;
        public override void _Ready() {
            player = parentBody as Player;
        }

        public override void EnterState() {
            if (player.Velocity.Length() > player.MoveSpeed) player.Velocity *= 1.3f;
            player.ToggleCrouch(true);
        }

        public override void ExitState() {
            player.ToggleCrouch(false);
        }

        public override void ProcessFrame(double delta) {
            
        }

        public override void ProcessInput(InputEvent inputEvent) {
            // check if standing causes clipping into roof
            if (player.ClipsIntoCeilingOnStand()) return;
            
            if (Input.IsActionJustPressed("jump")) manager.ChangeState(manager.AllStates["Jumping"]);
            else if (Input.IsActionJustPressed("zip")) manager.ChangeState(manager.AllStates["Zipping"]);
            else if (Input.IsActionJustPressed("swing")) manager.ChangeState(manager.AllStates["Swinging"]);

            // handle releasing crouch
            if (Input.IsActionPressed("crouch")) return;

            if (player.Velocity.Length() > player.MoveSpeed * player.SprintMultiplier ||
                Input.GetVector("left", "right", "forward", "backward").IsZeroApprox())
                manager.ChangeState(manager.AllStates["Recovering"]);
            if (player.Velocity.IsZeroApprox()) manager.ChangeState(manager.AllStates["Idle"]);
            else manager.ChangeState(manager.AllStates["Running"]);
        }

        public override void ProcessPhysics(double delta) {
            player.ApplyForce(Vector3.Down * player.Gravity, isOneShot: false);
            if (player.Velocity.Length() > player.MoveSpeed) {
                player.Velocity *= player.SlideFrictionCoefficient;
                if (Input.IsActionPressed("left")) player.Velocity += Vector3.Left * player.MoveSpeed * (float) delta;
                else if (Input.IsActionPressed("right")) player.Velocity += Vector3.Right * player.MoveSpeed * (float) delta;

            } else player.SetVelocityToInputVector(player.MoveSpeed / 2);
            player.SumForces();
            player.MoveAndSlide();
            if (!player.IsOnFloor()) manager.ChangeState(manager.AllStates["Falling"]);
        }
    }
}
