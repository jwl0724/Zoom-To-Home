using Godot;

namespace ZoomToHome {
    public partial class PlayerRotationManager : Node3D {
        public bool EnableInput { get; set; } = true;
        public bool EnableRotationEnforcement { get; set; } = false;
        public Vector3 LookPoint { get; set; } = Vector3.Inf;
        public bool IsGlobalPoint { get; set; } = true; 
        public float EnforceWeight { get; set; } = 1;
        private Player player;
        private RayCast3D raycast;
        public override void _Ready() {
            player = Owner as Player;
            raycast = GetNode<RayCast3D>("Raycast");
        }

        public override void _Input(InputEvent mouseMovement) {
            if (mouseMovement is not InputEventMouseMotion movement || Input.MouseMode != Input.MouseModeEnum.Captured 
            || player.StateManager.CurrentState is Cleared) 
                return;
            else ProcessMouseMovement(movement);
        }

        public override void _PhysicsProcess(double delta) {
            if (EnableRotationEnforcement) {
                Vector3 oldRotation = Rotation;

                if (IsGlobalPoint) LookAt(LookPoint);
                else LookAt(LookPoint + GlobalPosition);

                Vector3 interpolatedRotation = oldRotation.Lerp(Rotation, EnforceWeight);
                Rotation = new Vector3(interpolatedRotation.X, Mathf.LerpAngle(oldRotation.Y, Rotation.Y, EnforceWeight), 0);
            }
        }

        public Vector3 GetRaycastImpactPoint() {
            var collidingBody = raycast.GetCollider();
            if (collidingBody is not StaticBody3D surface) return Vector3.Inf;
            if (surface.GetCollisionLayerValue(4)) return Vector3.Inf;
            return raycast.GetCollisionPoint();
        }

        private void ProcessMouseMovement(InputEventMouseMotion movement) {
            if (!EnableInput) return;

            // looking left/right
            float rotationDegreesY = Rotation.Y + Mathf.DegToRad(-movement.Relative.X * player.MouseSensitivity);

            // looking up/down
            float rotationDegreesX = Mathf.Clamp(
                Mathf.DegToRad(-movement.Relative.Y * player.MouseSensitivity) + Rotation.X,
                Mathf.DegToRad(-89.99f), Mathf.DegToRad(89.99f)
            );
            Rotation = new Vector3(rotationDegreesX, rotationDegreesY, 0);
        }
    }
}
