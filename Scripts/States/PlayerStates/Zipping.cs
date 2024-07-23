using Godot;
using System;

namespace ZoomToHome {
    public partial class Zipping : State {
        private Vector3 grapplePoint;
        private Player player;

        public override void _Ready() {
            player = parentBody as Player;
        }

        public override void EnterState() {
            grapplePoint = player.GetRaycastImpactPoint();
            if (!grapplePoint.IsFinite()) 
                manager.ChangeState(manager.PreviousState);
        }

        public override void ExitState() {
            grapplePoint = Vector3.Inf; // set point to something invalid
        }

        public override void ProcessFrame(double delta) {
            
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (!Input.IsActionJustReleased("zip")) return;
            if (player.IsOnFloor()) manager.ChangeState(manager.AllStates["Recovering"]);
            else if (player.Velocity.Y > 0) manager.ChangeState(manager.AllStates["Jumping"]);
            else if (player.Velocity.Y < 0) manager.ChangeState(manager.AllStates["Falling"]);
        }

        public override void ProcessPhysics(double delta) {
            player.Velocity = player.Position.DirectionTo(grapplePoint) * player.ZipRetractSpeed;
            player.MoveAndSlide();
        }
    }
}
