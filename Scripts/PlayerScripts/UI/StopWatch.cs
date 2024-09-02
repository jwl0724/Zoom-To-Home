using Godot;
using System;

namespace ZoomToHome {
    public partial class StopWatch : Label {
        private float timeElapsed = 0f;
        private bool running = false;

        public override void _Ready() {
            GetTree().CurrentScene.Connect(Node.SignalName.Ready, Callable.From(() => Resume()));
        }   

        public override void _Process(double delta) {
            if (running == false) return;
            Text = ConvertFormat(timeElapsed);
            timeElapsed += (float) delta;
        }

        public void ResetWatch() {
            timeElapsed = 0f;
        }

        public float Stop() {
            running = false;
            return timeElapsed;
        }

        public void Resume() {
            running = true;
        }

        public static string ConvertFormat(float time) {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            return string.Format(@"{0:mm\:ss\.ff}", timeSpan);
        }
    }
}
