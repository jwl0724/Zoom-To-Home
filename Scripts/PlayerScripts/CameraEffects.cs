using Godot;

namespace ZoomToHome {
    public partial class CameraEffects : Camera3D {
        public static float DefaultFOV;
        private Player player;
        private static readonly int FOVChangeSpeed = 50;
        private static readonly int minFOVDifference = 5;
        private StateManager playerStateManager;
        private FastNoiseLite screenShakeNoise = new();
        private uint noiseSamplerY = 0;
        private uint cycleAngle = 0;

        public override void _Ready() {
            player = Owner as Player;
            player.Connect(SignalName.Ready, Callable.From(() => 
                playerStateManager = (Owner as Player).StateManager
            ));
            
            DefaultFOV = Fov;
        }

        public override void _PhysicsProcess(double delta) {
            HandleCameraFOV((float) delta);
            HandleCameraShake();
        }

        public void TiltCamera(float angle, float time) {
            Tween cameraTween = CreateTween();
            cameraTween.TweenProperty(this, "rotation", Vector3.Back * angle, time);
        }

        private void HandleCameraFOV(float delta) {
            if (playerStateManager.CurrentState is Sprinting) {
                if (Fov < DefaultFOV - minFOVDifference) return;
                Fov -= delta * FOVChangeSpeed;

            } else if (Fov < DefaultFOV) Fov += delta * FOVChangeSpeed;
        }

        private void ShakeCamera(float maxRoll, float maxOffsetX, float maxOffsetY, float intensity = 1) {
                noiseSamplerY++;
                Rotation = new Vector3(0, 0, maxRoll * screenShakeNoise.GetNoise2D(screenShakeNoise.Seed, noiseSamplerY * intensity));
                HOffset = maxOffsetX * screenShakeNoise.GetNoise2D(screenShakeNoise.Seed * 2, noiseSamplerY * intensity);
                VOffset = maxOffsetY * screenShakeNoise.GetNoise2D(screenShakeNoise.Seed * 3, noiseSamplerY * intensity);
        }

        private void BobCamera(float maxOffsetX, float maxOffsetY, float intensity = 1) {
            float stepX = maxOffsetX * Mathf.Sin(Mathf.DegToRad(cycleAngle * intensity));
            float stepY = maxOffsetY * Mathf.Sin(Mathf.DegToRad(cycleAngle * intensity));
            
            HOffset = stepX;
            VOffset = stepY;
            cycleAngle++;
        }

        private void HandleCameraShake() {
            if (playerStateManager.CurrentState is Running) {
                BobCamera(0, 0.2f, 15);

            } else if (playerStateManager.CurrentState is Sprinting) {
                ShakeCamera(0.05f, 0, 0);
                BobCamera(0.01f, 0.2f, 30);
                VOffset += 0.2f;

            } else if (playerStateManager.CurrentState is WallRunning) {
                BobCamera(0.4f, 0.1f, player.Velocity.Length() / 4);

            } else if (playerStateManager.CurrentState is Falling) {
                if (player.Velocity.Length() > 75)
                    ShakeCamera(0.15f, 0.5f, 0.5f, player.Velocity.Length() / 100);

            } else if (playerStateManager.CurrentState is Zipping) {
                ShakeCamera(0.1f, 0.25f, 0.25f, player.Velocity.Length() / 100);

            } else if (playerStateManager.CurrentState is Swinging) {
                ShakeCamera(0.1f, 0.35f, 0.15f, player.Velocity.Length() / 100);

            } else {
                ResetCamera();
            }
        }

        private void ResetCamera() {
            if (!Rotation.IsZeroApprox() || HOffset != 0 || VOffset != 0) {
                noiseSamplerY = GD.Randi() % 69;
                cycleAngle = 0;

                Tween resetTween = CreateTween();
                resetTween.TweenProperty(this, "rotation", Vector3.Zero, 0.05f);
                resetTween.TweenProperty(this, "h_offset", 0, 0.05f);
                resetTween.TweenProperty(this, "v_offset", 0, 0.05f);
                resetTween.Play();
            }
        }
    }
}
