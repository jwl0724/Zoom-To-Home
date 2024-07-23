using Godot;

namespace ZoomToHome {
    public partial class PlayerRotationManager : Node3D {
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
