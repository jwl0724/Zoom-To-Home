using Godot;

namespace ZoomToHome {
    public partial class CameraEffects : Camera3D {
        public static float DefaultFOV;
        private static readonly int FOVChangeSpeed = 50;
        private static readonly int minFOVDifference = 5;
        private StateManager playerStateManager;
        public override void _Ready() {
            Owner.Connect(SignalName.Ready, Callable.From(() => 
                playerStateManager = (Owner as Player).StateManager
            ));
            DefaultFOV = Fov;
        }

        public override void _Process(double delta) {
            HandleCameraFOV((float) delta);
        }

        public void TiltCamera(float angle) {
            Tween cameraTween = CreateTween();
            cameraTween.TweenProperty(this, "rotation", Vector3.Back * angle, 0.15f);
        }

        private void HandleCameraFOV(float delta) {
            if (playerStateManager.CurrentState is Sprinting) {
                if (Fov < DefaultFOV - minFOVDifference) return;
                Fov -= delta * FOVChangeSpeed;

            } else if (Fov < DefaultFOV) Fov += delta * FOVChangeSpeed;
        }
    }
}
