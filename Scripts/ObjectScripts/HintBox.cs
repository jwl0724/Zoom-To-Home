using Godot;

namespace ZoomToHome {
    public partial class HintBox : Node3D {
        [Export] string hintText;
        private Area3D hintArea;

        public override void _Ready() {
            hintArea = GetNode("Hint Area") as Area3D;
            hintArea.Connect(Area3D.SignalName.BodyEntered, Callable.From((Node3D body) => DisplayHint()));
        }

        public override void _PhysicsProcess(double delta) {
            Rotation += Vector3.Up * (float) delta;
        }

        private void DisplayHint() {
            // WILL ADD LATER
            GD.Print(hintText);
        }
    }
}
