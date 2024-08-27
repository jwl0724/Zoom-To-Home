using Godot;
using Godot.Collections;
using System.Text.RegularExpressions;

namespace ZoomToHome {
    public partial class HintBox : Node3D {
        [Export] private string hintText;
        private MeshInstance3D model;
        private DialogBox dialogBox;
        private Area3D hintArea;

        public override void _Ready() {
            model = GetNode("Model") as MeshInstance3D;
            hintArea = GetNode("Hint Area") as Area3D;
            dialogBox = GetNode("Hint Dialog") as DialogBox;
            
            hintArea.Connect(Area3D.SignalName.BodyEntered, Callable.From((Node3D body) => DisplayHint()));
            hintArea.Connect(Area3D.SignalName.BodyExited, Callable.From((Node3D body) => CloseHint()));

            string dialogText = TranslateText(hintText);
            dialogBox.SetText(dialogText);
        }

        public override void _PhysicsProcess(double delta) {
            model.Rotation += Vector3.Up * (float) delta;
        }

        private void DisplayHint() {
            dialogBox.ShowDialog();
        }

        private void CloseHint() {
            dialogBox.CloseDialog();
        }

        private string TranslateText(string text) {
            MatchCollection matchCollection = Regex.Matches(text, @"\[.*?\]");
            foreach(Match match in matchCollection) {
                string keybind = GetReplacementText(match.Value.Trim(']').Trim('['));
                text = text.Replace(match.Value, keybind);
            }
            return text;
        }

        private string GetReplacementText(string action) {
            if (!InputMap.HasAction(action)) {
                GD.PushError($"{action} does not exist on the input map");
                return "ERROR";
            }
            
            InputEvent inputEvent = InputMap.ActionGetEvents(action)[0];
            string keyName = inputEvent.AsText();
            keyName = keyName.Replace(" (Physical)", "");
            return keyName;
        }
    }
}
