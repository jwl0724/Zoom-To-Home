using Godot;

namespace ZoomToHome {
    public partial class InputManager : Node {
        // SIGNALS
        [Signal] public delegate void MoveEventHandler();
        [Signal] public delegate void JumpEventHandler();
        [Signal] public delegate void CrouchEventHandler();
        [Signal] public delegate void SwingEventHandler();
        [Signal] public delegate void ZipEventHandler();

        private Player player;

        public override void _Ready() {
            player = Owner as Player;
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }

        public override void _PhysicsProcess(double delta) {
            ProcessMovement();
        }

        public override void _Input(InputEvent inputEvent) {
            if (inputEvent is InputEventMouseButton) ProcessMouseButtons();
            if (inputEvent is InputEventKey) ProcessKeys();
        }

        private void ProcessMouseButtons() {
            // TODO: IMPLEMENT LATER
        }

        private void ProcessKeys() {
            if (Input.IsActionJustPressed("pause")) {
                PauseGame();
                return;
            }
            if (Input.IsActionJustPressed("jump") && player.IsOnFloor()) {
                EmitSignal(SignalName.Jump);
            }
            if (Input.IsActionPressed("crouch")) {
                EmitSignal(SignalName.Crouch);
            }
        }

        private void ProcessMovement() {
            Vector2 inputVector = Input.GetVector("left", "right", "forward", "backward");
            EmitSignal(SignalName.Move, inputVector); // DROP THIS IF THERES PERFORMANCE DROP
        }

        // MOVE THIS TO SOMEWHERE ELSE LATER SO YOU CAN ACTUALLY UNPAUSE    
        private void PauseGame() {
            SceneTree tree = GetTree();
            tree.Paused = !tree.Paused;
            if (tree.Paused) Input.MouseMode = Input.MouseModeEnum.Visible;
            else Input.MouseMode = Input.MouseModeEnum.Captured;
        }
    }
}
