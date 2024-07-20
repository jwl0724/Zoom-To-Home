using Godot;

namespace ZoomToHome {
    public partial class PlayerRotationManager : Node3D {
        private Player player;
        public override void _Ready() {
            player = Owner as Player;
        }

        public override void _Input(InputEvent mouseMovement) {
            if (mouseMovement is not InputEventMouseMotion movement || Input.MouseMode != Input.MouseModeEnum.Captured) 
                return;
            else ProcessMouseMovement(movement);
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
