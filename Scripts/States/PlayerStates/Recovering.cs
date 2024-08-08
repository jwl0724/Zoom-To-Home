using Godot;
using System;

namespace ZoomToHome {
    public partial class Recovering : State {
        private Player player;

        public override void _Ready() {
            player = parentBody as Player;
        }

        public override void EnterState() {
            player.PlayAnimation("Land");
        }

        public override void ExitState() {
            player.FloorSnapLength = 0.1f;
        }

        public override void ProcessFrame(double delta) {
            player.PlayAnimation("Stand", playOver: false);
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (Input.IsActionJustPressed("jump")) manager.ChangeState(manager.AllStates["Jumping"]);
            if (Input.IsActionJustPressed("zip")) manager.ChangeState(manager.AllStates["Zipping"]);
            if (Input.IsActionJustPressed("swing")) manager.ChangeState(manager.AllStates["Swinging"]);
            if (Input.IsActionJustPressed("crouch")) manager.ChangeState(manager.AllStates["Crouch"]);
        }

        public override void ProcessPhysics(double delta) {
            Vector2 inputVector = Input.GetVector("left", "right", "forward", "backward");
            Vector3 movementDirection = player.GetForwardVectorOnHorizontalPlane(
                new Vector3(inputVector.X, 0, inputVector.Y), player.MoveSpeed
            );

            player.Velocity = player.Velocity.Lerp(Vector3.Zero, 0.025f);
            player.ApplyForce(movementDirection, isOneShot: false);
            player.SumForces();
            player.FloorSnapLength = player.GetForwardVelocityHorizontalMagnitude() * 
                Mathf.Tan(player.FloorMaxAngle) * (float) delta * 2;
            player.MoveAndSlide();
                      
            if (!player.IsOnFloor()) {
                manager.ChangeState(manager.AllStates["Falling"]);
                return;
            }
            if (player.Velocity.Length() > player.MoveSpeed * player.SprintMultiplier) return;
            if (!Input.GetVector("left", "right", "forward", "backward").IsZeroApprox()) {
                if (Input.IsActionPressed("sprint")) manager.ChangeState(manager.AllStates["Sprinting"]);
                else if (player.Velocity.Length() < player.MoveSpeed) manager.ChangeState(manager.AllStates["Running"]);
            }
            if (player.Velocity.Length() < player.MoveSpeed) manager.ChangeState(manager.AllStates["Idle"]);
        }
    }
}
