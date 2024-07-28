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
        }

        public override void ProcessFrame(double delta) {
            
        }

        public override void ProcessInput(InputEvent inputEvent) {
            
        }

        public override void ProcessPhysics(double delta) {
            if (!vaultDestination.IsFinite()) {
                GD.PushError("Vaulting Destination is Infinite");
                return;
            }
            float velocityHorizontal = player.GetForwardVectorOnHorizontalPlane(player.Velocity, player.Velocity.Length()).Length();
            player.Position = player.Position.Lerp(vaultDestination, velocityHorizontal * 0.1f);
            
            if (!player.Position.IsEqualApprox(vaultDestination)) return;
            if (manager.PreviousState is Zipping) manager.ChangeState(manager.AllStates["Jumping"]);
            else manager.ChangeState(manager.AllStates["Recovering"]);
        }
    }
}
