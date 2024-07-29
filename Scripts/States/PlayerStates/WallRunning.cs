using Godot;
using System;

namespace ZoomToHome {
    public partial class WallRunning : State {
        [Export] private CameraEffects camera;
        private static readonly float tiltAngle = 0.4f;
        private static readonly float tiltSpeed = 0.15f; // in seconds
        private Player player;

        public override void _Ready() {
            player = parentBody as Player;
        }

        public override void EnterState() {
            player.Velocity += Vector3.Up * player.JumpSpeed;
            Vector3 wallNormal = player.GetWallNormal();
            Vector3 perpindicular = wallNormal * player.Velocity.Dot(wallNormal);
            Vector3 parallel = player.Velocity - perpindicular;
            player.Velocity = parallel;

            Vector3 rightVector = player.GetForwardVectorOnHorizontalPlane(Vector3.Right, 1);
            if (rightVector.Dot(wallNormal) < 0) camera.TiltCamera(tiltAngle, tiltSpeed);
            else camera.TiltCamera(-tiltAngle, tiltSpeed);
        }

        public override void ExitState() {
            if (player.IsOnWallOnly()) {
                float launchMagnitude = Mathf.Max(player.GetForwardVelocityHorizontalMagnitude(), player.JumpSpeed);
                player.ApplyForce(player.GetWallNormal() * launchMagnitude, isOneShot: true);
             
            } else player.Velocity = new Vector3(player.Velocity.X, 0, player.Velocity.Z);
            camera.TiltCamera(0, tiltSpeed);
        }

        public override void ProcessFrame(double delta) {
            
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (Input.IsActionJustPressed("jump")) manager.ChangeState(manager.AllStates["Jumping"]);
            else if (Input.IsActionJustPressed("zip")) manager.ChangeState(manager.AllStates["Zipping"]);
            else if (Input.IsActionJustPressed("swing")) manager.ChangeState(manager.AllStates["Swinging"]);
        }

        public override void ProcessPhysics(double delta) {
            player.ApplyForce(Vector3.Down * player.Gravity / 3, isOneShot: false);
            player.ApplyForce(-player.GetWallNormal() * player.Gravity, isOneShot: false);
            player.SumForces();
            player.MoveAndSlide();

            if (player.IsOnFloor()) manager.ChangeState(manager.AllStates["Recovering"]);
            else if (!player.IsOnWall()) manager.ChangeState(manager.AllStates["Falling"]);
        }
    }

}
