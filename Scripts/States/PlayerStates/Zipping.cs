using Godot;
using System;

// TODO: DRAW THE STRING FROM A NODE POINT (4 POINTED)
namespace ZoomToHome {
    public partial class Zipping : State {
        [Export] private Node3D linePoint;
        [Export] private LineRenderer renderer;
        private AnimationPlayer linePointAnimator;
        private Vector3 grapplePoint;
        private Player player;

        // ZIP COOLDOWN TIMER
        private readonly float zipCooldown = 1f; // in seconds
        private float zipTimer = 0f;
        private bool onCooldown = false;

        public override void _Ready() {
            player = parentBody as Player;
            linePointAnimator = linePoint.GetNode("Line Point Animation") as AnimationPlayer;
        }

        public override void _Process(double delta) {
            if (!onCooldown || manager.CurrentState is Zipping) return;

            if (zipTimer >= zipCooldown) {
                zipTimer = 0;
                onCooldown = false;

            } else zipTimer += (float) delta;
        }

        public override void EnterState() {
            grapplePoint = player.GetRaycastImpactPoint();
            if (!grapplePoint.IsFinite() || onCooldown) 
                manager.ChangeState(manager.PreviousState);
            else {
                onCooldown = true;
                player.PlayAnimation("Zip");
                linePointAnimator.Play("Zip", customSpeed: 2);

                player.EnforceRotation(true, grapplePoint, 2);
            }
        }

        public override void ExitState() {
            renderer.ClearLine();
            grapplePoint = Vector3.Inf; // set point to something invalid
            player.EnforceRotation(false);
        }

        public override void ProcessFrame(double delta) {
            if (grapplePoint.IsFinite())
                renderer.RenderLine(linePoint.GlobalPosition, grapplePoint, 0.05f, 4);
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (!Input.IsActionJustReleased("zip")) return;            
            if (player.IsOnFloor()) manager.ChangeState(manager.AllStates["Recovering"]);
            else manager.ChangeState(manager.AllStates["Falling"]);
        }

        public override void ProcessPhysics(double delta) {
            player.Velocity = player.Position.DirectionTo(grapplePoint) *
                Mathf.Max(player.Velocity.Length(), player.ZipRetractSpeed);
            player.MoveAndSlide();

            if (player.IsOnWall() && player.IsOnFloor()) manager.ChangeState(manager.AllStates["Idle"]);
            else if (player.IsOnWall()) {
                manager.ChangeState(manager.AllStates["Falling"]);
                player.Velocity = new Vector3(player.Velocity.X, 0, player.Velocity.Z);
            }
        }
    }
}
