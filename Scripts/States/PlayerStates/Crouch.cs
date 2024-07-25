using Godot;
using System;

namespace ZoomToHome {
    public partial class Crouch : State {
        private Player player;

        // BOOST TIMER VARIABLES
        private bool boostDisabled = false;
        private static readonly float boostCooldown = 1f;
        private float boostTimer = 0;

        public override void _Ready() {
            player = parentBody as Player;
        }

        // need to increment timer, even when state isnt active
        public override void _Process(double delta) {
            if (boostDisabled && boostTimer < boostCooldown) boostTimer += (float) delta;
            else {
                boostTimer = 0;
                boostDisabled = false;
            }
        }

        public override void EnterState() {
            float velocityMagnitude = player.Velocity.Length();
            if (velocityMagnitude > player.MoveSpeed && !boostDisabled) {
                player.Velocity *= 1.3f;
                boostDisabled = true;
            }
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

            if (player.Velocity.Length() > player.MoveSpeed) manager.ChangeState(manager.AllStates["Recovering"]);
            else if (player.Velocity.IsZeroApprox()) manager.ChangeState(manager.AllStates["Idle"]);
            else if (!Input.GetVector("left", "right", "forward", "backward").IsZeroApprox())
                manager.ChangeState(manager.AllStates["Running"]);
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
