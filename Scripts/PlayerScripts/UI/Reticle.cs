using Godot;
using System;

namespace ZoomToHome {
    public partial class Reticle : TextureRect {
        private Player player;

        public override void _Ready() {
            player = Owner as Player;
        }

        public override void _Process(double delta) {
            if (!player.GetRaycastImpactPoint().IsFinite()) Modulate = new Color(1, 1, 1, 1); // white
            else Modulate = new Color(0.08f, 0.76f, 0.12f, 1); // green
        }
    }
}
