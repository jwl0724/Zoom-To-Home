using Godot;
using System;

// TODO: DRAW THE STRING FROM A NODE POINT (CIRCLE)
namespace ZoomToHome {
    public partial class Swinging : State {
        [Export] private Node3D linePoint;
        [Export] private LineRenderer renderer;
        private AnimationPlayer linePointAnimator;
        private Vector3 grapplePoint;
        private Player player;
        public override void _Ready() {
            player = parentBody as Player;
            linePointAnimator = linePoint.GetNode("Line Point Animation") as AnimationPlayer;
        }

        public override void EnterState() {
            grapplePoint = player.GetRaycastImpactPoint();
            if (!grapplePoint.IsFinite())
                manager.ChangeState(manager.PreviousState);
            else {
                player.PlayAnimation("Swing");
                linePointAnimator.Play("Swing", customSpeed: 2);
                
                player.EnforceRotation(true, grapplePoint, 0.01f);
            }
        }

        public override void ExitState() {
            renderer.ClearLine();
            grapplePoint = Vector3.Inf;
            player.EnforceRotation(false);
        }

        public override void ProcessFrame(double delta) {
            if (grapplePoint.IsFinite())
                renderer.RenderLine(linePoint.GlobalPosition, grapplePoint, 0.08f, 20);
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (!Input.IsActionJustReleased("swing")) return;
            if (player.IsOnFloor()) manager.ChangeState(manager.AllStates["Recovering"]);
            else if (player.IsOnCeiling()) manager.ChangeState(manager.AllStates["Falling"]);
            else if (player.Velocity.Y > 0) manager.ChangeState(manager.AllStates["Jumping"]);
            else if (player.Velocity.Y < 0) manager.ChangeState(manager.AllStates["Falling"]);
        }

        public override void ProcessPhysics(double delta) {
            // Vector3 swingForce = player.Position.DirectionTo(grapplePoint) * 
                // (player.Velocity.Length() + player.SwingRetractSpeed);

            Vector3 swingForce = player.Position.DirectionTo(grapplePoint);
            float counterForceFactor = swingForce.Dot(player.Velocity.Normalized());
            if (counterForceFactor < 0) {
                swingForce *= (1 - counterForceFactor) * (player.Velocity.Length() * 2 + player.SwingRetractSpeed);
            } else swingForce *= (1 - counterForceFactor) * (player.Velocity.Length() + player.SwingRetractSpeed);

            player.ApplyForce(swingForce, isOneShot: false);
            player.ApplyForce(Vector3.Down * player.Gravity / 2, isOneShot: false);
            player.SumForces();
            player.MoveAndSlide();

        }
    }

}
