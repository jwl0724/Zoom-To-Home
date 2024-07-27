using Godot;
using System;

// TODO: DRAW THE STRING FROM A NODE POINT (CIRCLE)
namespace ZoomToHome {
    public partial class Swinging : State {
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
            grapplePoint = Vector3.Inf;
        }

        public override void ProcessFrame(double delta) {
            if (grapplePoint.IsFinite())
                renderer.RenderLine(player.GlobalPosition, grapplePoint, 4, 16);
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (!Input.IsActionJustReleased("swing")) return;
            if (player.IsOnFloor()) manager.ChangeState(manager.AllStates["Recovering"]);
            else if (player.IsOnCeiling()) manager.ChangeState(manager.AllStates["Falling"]);
            else if (player.Velocity.Y > 0) manager.ChangeState(manager.AllStates["Jumping"]);
            else if (player.Velocity.Y < 0) manager.ChangeState(manager.AllStates["Falling"]);
        }

        public override void ProcessPhysics(double delta) {
            Vector3 swingForce = player.Position.DirectionTo(grapplePoint) * 
                (player.Velocity.Length() + player.SwingRetractSpeed);
            
            player.ApplyForce(swingForce, isOneShot: false);
            player.ApplyForce(Vector3.Down * player.Gravity / 2, isOneShot: false);
            player.SumForces();
            player.MoveAndSlide();

        }
    }

}
