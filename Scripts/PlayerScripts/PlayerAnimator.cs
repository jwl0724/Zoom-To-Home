using Godot;
using System;
using System.Linq;

namespace ZoomToHome {
    public partial class PlayerAnimator : Node3D {
        [Export] private Player player;
        [Export] private AnimationPlayer animation;
        private string[] specialAnimations = {"Stand", "Swing", "Zip"};

        public override void _Process(double delta) {
            if (player.StateManager.CurrentState is WallRunning) return; // BUG: NEED TO FIX ROTATION IF PLAYER NOT LOOKING PROPERLY WHILE ATTACHING TO WALL
            Rotation = Vector3.Up * (Mathf.Pi + player.RotationHelper.Rotation.Y);
        }

        public void Play(string animationName, bool playOver = true) {
            if (specialAnimations.Contains(animationName)) {
                // handle special animations
                switch(animationName) {
                    case "Stand":
                        animation.PlayBackwards("Crouch");
                        break;
                    case "Swing":
                        break;
                    case "Zip":
                        break;
                    default:
                        GD.PushError("Something went horribly wrong, this should never run");
                        break;
                }

            } else if (animation.GetAnimationList().Contains(animationName)) {
                // handle animations in animation player library
                if (!playOver && animation.IsPlaying()) return;
                else animation.Play(animationName);

            } else GD.PushError($"{animationName} does not exists in either animation library or special animation list");
        }
    }
}
