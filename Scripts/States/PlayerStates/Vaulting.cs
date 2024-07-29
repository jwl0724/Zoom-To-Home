using Godot;
using System;

namespace ZoomToHome {
    public partial class Vaulting : State {
        [Export] CameraEffects cameraEffects;
        private readonly float tiltAngle = 0.1f;
        private Player player;
        private Vector3 vaultDestination;

        public override void _Ready() {
            player = parentBody as Player;
        }

        public override void EnterState() {
            vaultDestination = player.GetVaultDestination();
            cameraEffects.TiltCamera(0.1f, 0.1f);
        }

        public override void ExitState() {
            vaultDestination = Vector3.Inf;
            cameraEffects.TiltCamera(0, 0.1f);
            player.Velocity = new Vector3(player.Velocity.X, 0, player.Velocity.Z);
        }

        public override void ProcessFrame(double delta) {
            
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (Input.IsActionJustPressed("jump")) manager.ChangeState(manager.AllStates["Jumping"]);
        }

        public override void ProcessPhysics(double delta) {
            if (!vaultDestination.IsFinite()) {
                GD.PushError("Vaulting Destination is Infinite");
                return;
            }
            
            player.Position = player.Position.Lerp(vaultDestination, 0.9f);
            if (!player.Position.IsEqualApprox(vaultDestination)) return;
            manager.ChangeState(manager.AllStates["Recovering"]);
        }
    }
}
