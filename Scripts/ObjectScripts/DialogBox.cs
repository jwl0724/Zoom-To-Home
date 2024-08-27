using Godot;

namespace ZoomToHome {
    public partial class DialogBox : Control {
        private static Color shownColor = new(1, 1, 1, 1);
        private static Color hiddenColor = new(1, 1, 1, 0);
        private Label label;
        private Tween textTween;

        public override void _Ready() {
            label = GetNode("Dialog Box/Text") as Label;
            label.VisibleRatio = 0;
            Modulate = new Color(1, 1, 1, 0);
        }

        public void ShowDialog() {
            if (textTween != null && textTween.IsRunning()) return;
            textTween = CreateTween();
            textTween.TweenProperty(this, "modulate", shownColor, 0.1f);
            textTween.TweenProperty(label, "visible_ratio", 1, 0.4f);
            textTween.Play();
        }

        public void CloseDialog() {
            textTween?.Kill();
            textTween = CreateTween();
            textTween.TweenProperty(this, "modulate", hiddenColor, 0.1f);
            textTween.TweenProperty(label, "visible_ratio", 0, 0.2f);
            textTween.Play();
        }

        public void SetText(string text) {
            label.Text = text;
        }
    }
}
