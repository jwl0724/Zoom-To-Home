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
            if (mouseMovement is not InputEventMouseMotion movement || Input.MouseMode != Input.MouseModeEnum.Captured) 
                return;
            else ProcessMouseMovement(movement);
        }

        public Vector3 GetRaycastImpactPoint() {
            var collidingBody = raycast.GetCollider();
            if (collidingBody is not StaticBody3D) return Vector3.Inf;
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
            // TODO: FIX THIS GARBAGE
            // if (EnableRotationEnforcement) {
            //     if (IsGlobalPoint) {
            //         float angleX = new Vector2(GlobalPosition.X, GlobalPosition.Y).AngleToPoint(new Vector2(LookPoint.X, LookPoint.Y));
            //         float angleY = new Vector2(GlobalPosition.X, GlobalPosition.Z).AngleToPoint(new Vector2(LookPoint.X, LookPoint.Z));
            //         Rotation = Rotation.Lerp(new Vector3(angleX, angleY, 0), EnforceWeight);
            //         // Rotation = Rotation.Lerp(GlobalPosition.DirectionTo(LookPoint), EnforceWeight);

            //     } else {
            //         float angleX = new Vector2(Position.X, Position.Y).AngleToPoint(new Vector2(LookPoint.X, LookPoint.Y));
            //         float angleY = new Vector2(Position.X, Position.Z).AngleToPoint(new Vector2(LookPoint.X, LookPoint.Z));
            //         Rotation = Rotation.Lerp(new Vector3(angleX, angleY, 0), EnforceWeight);
            //         // Rotation = Rotation.Lerp(Position.DirectionTo(LookPoint), EnforceWeight);
            //     }
            // }
        }
    }
}
