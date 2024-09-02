using Godot;
using System;

namespace ZoomToHome {
    public partial class Cleared : State {
        private Player player;

        public override void _Ready() {
            player = parentBody as Player;
        }

        public override void EnterState() {
            // DO SOME EFFECT, FIGURE IT OUT LATER, MAYBE FADE TO WHITE?
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }

        public override void ExitState() {
            // UNDO EVERYTHING DONE DURING ENTER STATE
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }

        public override void ProcessFrame(double delta) {
            
        }

        public override void ProcessInput(InputEvent inputEvent) {
            
        }

        public override void ProcessPhysics(double delta) {
            
        }
    }
}
