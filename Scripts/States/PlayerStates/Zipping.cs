using Godot;
using System;

// TODO: DRAW THE STRING FROM A NODE POINT (4 POINTED)
namespace ZoomToHome {
    public partial class Zipping : State {
        [Export] private LineRenderer renderer;
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
            if (grapplePoint.IsFinite())
                renderer.RenderLine(player.GlobalPosition, grapplePoint, 4, 4);
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (!Input.IsActionJustReleased("zip")) return;            
            if (player.IsOnFloor()) manager.ChangeState(manager.AllStates["Recovering"]);
            else if (player.IsOnCeiling()) manager.ChangeState(manager.AllStates["Falling"]);
            else if (player.Velocity.Y > 0) manager.ChangeState(manager.AllStates["Jumping"]);
            else if (player.Velocity.Y < 0) manager.ChangeState(manager.AllStates["Falling"]);
        }

        public override void ProcessPhysics(double delta) {
            player.Velocity = player.Position.DirectionTo(grapplePoint) *
                Mathf.Max(player.Velocity.Length(), player.ZipRetractSpeed);
            player.MoveAndSlide();

        }
    }
}
