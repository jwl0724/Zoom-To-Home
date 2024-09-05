using System.Linq;
using Godot;

namespace ZoomToHome {
    public partial class ScoreScreen : Control {
        // exports for scene
        [Export] private LabelSettings letterGradeSettings;
        [Export] private ShaderMaterial SRankShader;
        
        // exports specific to each level
        [Export] private string levelName;
        [Export] private float SRankThreshold;
        [Export] private float ARankThreshold;
        [Export] private float BRankThreshold;
        [Export] private float CRankThreshold;

        // modulate colors
        private static Color transparent = new(1, 1, 1, 0);
        private static Color visibleColor = new(1, 1, 1, 1);
        
        // letter grade colors
        private static Color SRankColor = new(1, 1, 1, 1); // white
        private static Color ARankColor = new("e8cd27"); // gold
        private static Color BRankColor = new("c4d0e0"); // silver
        private static Color CRankColor = new("bb4500"); // bronze
        private static Color FRankColor = new("251d1d"); // dark gray

        public override void _Ready() {
            Modulate = transparent;
            Label letterGrade = GetNode("Results/Rank/Grade") as Label;
            letterGrade.Material = null;
            
            // set rank threshold text
            (GetNode("Results/Rankings/S Rank") as Label).Text = $"S - {StopWatch.ConvertFormat(SRankThreshold)}";
            (GetNode("Results/Rankings/A Rank") as Label).Text = $"A - {StopWatch.ConvertFormat(ARankThreshold)}";
            (GetNode("Results/Rankings/B Rank") as Label).Text = $"B - {StopWatch.ConvertFormat(BRankThreshold)}";
            (GetNode("Results/Rankings/C Rank") as Label).Text = $"C - {StopWatch.ConvertFormat(CRankThreshold)}";

            // modulate all child nodes
            foreach(Control child in GetChildren().Cast<Control>()) child.Modulate = transparent;
        }

        public void PlayScoreScreen(float timeElapsed) {
             if (timeElapsed < SRankThreshold) SetLetterGrade('S');
             else if (timeElapsed < ARankThreshold) SetLetterGrade('A');
             else if (timeElapsed < BRankThreshold) SetLetterGrade('B');
             else if (timeElapsed < CRankThreshold) SetLetterGrade('C');
             else SetLetterGrade('F');
             
            Modulate = visibleColor;
            Tween scoreScreenTween = CreateTween();
            scoreScreenTween.TweenProperty(GetNode("Fade") as Control, "modulate", visibleColor, 0.8f);
            scoreScreenTween.TweenCallback(Callable.From(() => {
                (GetNode("Background") as Control).Modulate = visibleColor;
                ShowResults(timeElapsed);
            }));
            scoreScreenTween.Play();
        }

        private void ShowResults(float timeElapsed) {
            Control results = GetNode("Results") as Control;
            foreach(Control child in results.GetChildren().Cast<Control>()) child.Modulate = transparent;
            (GetNode("Results") as Control).Modulate = visibleColor;

            Tween resultsTween = CreateTween();
            foreach(Control child in results.GetChildren().Cast<Control>()) {
                child.Modulate = transparent;
                if (child.Name == "Rank") PlayGradeAnimation();
                else {
                    Vector2 originalPosition = child.Position;
                    child.Position = new Vector2(originalPosition.X - 4000, originalPosition.Y);
                    child.Modulate = visibleColor;
                    resultsTween.TweenProperty(child, "position", originalPosition, 0.2f * (child.GetIndex() + 1));
                }
            }

            (results.GetNode("Clear") as Label).Text = $"{levelName} cleared";
            (results.GetNode("Time") as Label).Text = $"Time Taken: {StopWatch.ConvertFormat(timeElapsed)}";
            resultsTween.Play();
            resultsTween.TweenCallback(Callable.From(() => ShowButtons()));
        }

        private void ShowButtons() {
            Control buttons = GetNode("Buttons") as Control;
            
            Tween buttonsTween = CreateTween();
            Button leftButton = buttons.GetChild(0) as Button;
            Button rightButton = buttons.GetChild(1) as Button;
            
            Vector2 originalLeftPosition = leftButton.Position;
            leftButton.Position = new Vector2(originalLeftPosition.X - 3000, originalLeftPosition.Y);
            Vector2 originalRightPosition = rightButton.Position;
            rightButton.Position = new Vector2(originalRightPosition.X + 3000, originalRightPosition.Y);

            buttonsTween.TweenProperty(leftButton, "position", originalLeftPosition, 0.3f);
            buttonsTween.TweenProperty(rightButton, "position", originalRightPosition, 0.3f);
            buttonsTween.Play();

            leftButton.Disabled = false;
            rightButton.Disabled = false;
            buttons.Modulate = visibleColor;
        }

        private void PlayGradeAnimation() {
            Control rank = GetNode("Results/Rank") as Control;
            float originalFontSize = letterGradeSettings.FontSize;
            letterGradeSettings.FontSize = 6000;
            
            Tween gradeTween = CreateTween();
            gradeTween.TweenProperty(letterGradeSettings, "font_size", originalFontSize, 0.3f);
            rank.Modulate = visibleColor;
            gradeTween.Play();
        }

        private void SetLetterGrade(char grade) {
            Label letterGrade = GetNode("Results/Rank/Grade") as Label;
            letterGrade.Material = SRankShader;

            // set properties that are not color
            if (grade == 'S') {
                letterGradeSettings.OutlineSize = 0;
                letterGradeSettings.ShadowSize = 0;
                letterGrade.Material = SRankShader;
            } else {
                letterGradeSettings.OutlineSize = 20;
                letterGradeSettings.ShadowSize = 10;
                letterGrade.Material = null;
            }
        
            // set color and text
            if (grade == 'S') {
                letterGradeSettings.FontColor = SRankColor;
                letterGrade.Text = "S";

            } else if (grade == 'A') {
                letterGradeSettings.FontColor = ARankColor;
                letterGrade.Text = "A";

            } else if (grade == 'B') {
                letterGradeSettings.FontColor = BRankColor;
                letterGrade.Text = "B";

            } else if (grade == 'C') {
                letterGradeSettings.FontColor = CRankColor;
                letterGrade.Text = "C";

            } else if (grade == 'F') {
                letterGradeSettings.FontColor = FRankColor;
                letterGrade.Text = "F";

            } else  GD.PushError($"{grade} does not exist in the ranks");
        }
    }
}
