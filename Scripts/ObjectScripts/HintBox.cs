using Godot;

namespace ZoomToHome {
    public partial class HintBox : Node3D {
        [Export] private Label hintText;
        private DialogueBox dialogueBox;
        private Area3D hintArea;

        public override void _Ready() {
            hintArea = GetNode("Hint Area") as Area3D;
            hintArea.Connect(Area3D.SignalName.BodyEntered, Callable.From((Node3D body) => DisplayHint()));
            dialogueBox = GetNode("Hint Dialogue") as DialogueBox;
        }

        public override void _PhysicsProcess(double delta) {
            Rotation += Vector3.Up * (float) delta;
        }

        private void DisplayHint() {
            string dialogueText = TranslateText(hintText.Text);
            dialogueBox.ShowDialogue(dialogueText);
        }

        private string TranslateText(string text) {
            // IMPLEMENT LATER, WHEN DIFFERENT KEYBINDS FOR ACTIONS ARE IMPLEMENTED
            return text;
        }
    }
}
