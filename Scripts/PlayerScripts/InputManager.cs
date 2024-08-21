using Godot;

namespace ZoomToHome {
    public partial class InputManager : Node {
        private Player player;

        public override void _Ready() {
            player = Owner as Player;
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }

        public override void _Input(InputEvent inputEvent) {
            if (inputEvent is InputEventMouseButton) ProcessMouseButtons();
            if (inputEvent is InputEventKey) ProcessKeys();
        }

        private void ProcessMouseButtons() {
            // TODO: FIGURE OUT WHAT THIS IS GONNA DO
        }

        private void ProcessKeys() {
            if (Input.IsActionJustPressed("pause")) {
                PauseGame();
                return;
            }
        }

        private void PauseGame() {
            SceneTree tree = GetTree();
            tree.Paused = !tree.Paused;
            if (tree.Paused) Input.MouseMode = Input.MouseModeEnum.Visible;
            else Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }
}
