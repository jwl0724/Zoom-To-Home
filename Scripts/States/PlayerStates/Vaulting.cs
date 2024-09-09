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
            player.PlayAnimation("Vault");
            if (vaultDestination.IsFinite()) PlayVaultTween();
            else GD.PushError("Vaulting Destination is Infinite");
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
            
        }

        private void PlayVaultTween() {
            Tween vaultTween = CreateTween();
            float vaultDifference = vaultDestination.Y - player.Position.Y;
            Vector3 vaultHorizontalDestination = new(
                vaultDestination.X + player.Velocity.X * (float) GetProcessDeltaTime(),
                player.Position.Y + vaultDifference,
                vaultDestination.Z + player.Velocity.Z * (float) GetProcessDeltaTime()
            );
            vaultTween.TweenProperty(player, "position", player.Position + Vector3.Up * vaultDifference, 0.1f);
            vaultTween.TweenProperty(player, "position", vaultHorizontalDestination, 0.1f);
            vaultTween.TweenCallback(Callable.From(() => manager.ChangeState(manager.AllStates["Recovering"])));
            vaultTween.Play();
        }
    }
}
