using Godot;
using System;

namespace ZoomToHome {
    public partial class WallRunning : State {
        [Export] private CameraEffects camera;
        private static readonly float tiltAngle = 0.4f;
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
            if (rightVector.Dot(wallNormal) < 0) camera.TiltCamera(tiltAngle);
            else camera.TiltCamera(-tiltAngle);
        }

        public override void ExitState() {
            if (player.IsOnWallOnly()) {
                player.ApplyForce(Vector3.Up * player.JumpSpeed, isOneShot: true);
                player.ApplyForce(player.GetWallNormal() * player.Velocity.Length(), isOneShot: true);
            }
            camera.TiltCamera(0);
        }

        public override void ProcessFrame(double delta) {
            
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (Input.IsActionJustPressed("jump")) manager.ChangeState(manager.AllStates["Jumping"]);
            if (Input.IsActionJustPressed("zip")) manager.ChangeState(manager.AllStates["Zipping"]);
            if (Input.IsActionJustPressed("swing")) manager.ChangeState(manager.AllStates["Swinging"]);
        }

        public override void ProcessPhysics(double delta) {
            player.ApplyForce(Vector3.Down * player.Gravity / 3, isOneShot: false);
            player.ApplyForce(-player.GetWallNormal() * player.Gravity, isOneShot: false);
            player.SumForces();
            player.MoveAndSlide();

            if (player.IsOnFloor()) { 
                manager.ChangeState(manager.AllStates["Recovering"]);
                return;
            }
            if (!player.IsOnWall()) manager.ChangeState(manager.AllStates["Jumping"]);
            else if (!player.IsOnWall()) manager.ChangeState(manager.AllStates["Falling"]);
        }
    }

}
