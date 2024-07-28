using Godot;
using System;

namespace ZoomToHome {
    public partial class Vaulting : State {
        [Export] private VaultHelper vaultRaycasts;
        private Player player;

        public override void _Ready() {
            player = parentBody as Player;
        }

        public override void EnterState() {
            
        }

        public override void ExitState() {
            
        }

        public override void ProcessFrame(double delta) {
            
        }

        public override void ProcessInput(InputEvent inputEvent) {
            
        }

        public override void ProcessPhysics(double delta) {
            
        }
    }
}
