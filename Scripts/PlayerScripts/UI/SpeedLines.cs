using Godot;
using System;

namespace ZoomToHome {
    public partial class SpeedLines : ColorRect {
        private Player player;
        private ShaderMaterial speedLineShader;

        public override void _Ready() {
            player = Owner as Player;
            speedLineShader = Material as ShaderMaterial;
        }

        public override void _Process(double delta) {
            if (player.Velocity.Length() < player.MoveSpeed * player.SprintMultiplier) Visible = false;
            else Visible = true;
        }

        public override void _PhysicsProcess(double delta) {
            float maskEdgeValue = 1 - player.Velocity.Length() / player.ZipRetractSpeed / 3;
            maskEdgeValue = Mathf.Max(maskEdgeValue, 0.5f);
            speedLineShader.SetShaderParameter("mask_edge", maskEdgeValue);
        }
    }
}
