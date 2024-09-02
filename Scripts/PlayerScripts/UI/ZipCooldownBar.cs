using Godot;
using System;

namespace ZoomToHome {
    public partial class ZipCooldownBar : TextureProgressBar {
        private Player player;

        public override void _Ready() {
            player = Owner as Player;

            // set progress bar values
            MinValue = 0;
            MaxValue = player.ZipCooldown;
            Step = 0.05f;
            Value = 0;
        }

        public override void _Process(double delta) {
            Value = player.ZipTimer;
            if (Value == MinValue) Visible = false;
            else Visible = true;
        }
    }
}
