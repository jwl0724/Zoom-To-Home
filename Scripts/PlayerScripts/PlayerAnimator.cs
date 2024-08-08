using Godot;
using System;
using System.Linq;

// TODO: IN BLENDER, EITHER DELETE THE HEAD OF THE MODEL, OR SEPARATE THE HEAD INTO DIFFERENT COMPONENT
namespace ZoomToHome {
    public partial class PlayerAnimator : Node3D {
        [Export] private Player player;
        [Export] private AnimationPlayer animation;
        private string[] specialAnimations = {"Stand", "Swing", "Zip", "Stop", "CrouchIdle"};

        public override void _Process(double delta) {
            if (player.StateManager.CurrentState is WallRunning) return; // BUG: NEED TO FIX ROTATION IF PLAYER NOT LOOKING PROPERLY WHILE ATTACHING TO WALL
            Rotation = Vector3.Up * (Mathf.Pi + player.RotationHelper.Rotation.Y);
        }

        public void Play(string animationName, bool playOver = true) {
            if (specialAnimations.Contains(animationName)) {
                // handle special animations
                if (!playOver && animation.IsPlaying()) return;
                switch(animationName) {
                    case "Stand":
                        animation.PlayBackwards("Crouch");
                        break;
                    case "Swing":
                        break;
                    case "Zip":
                        break;
                    case "Stop":
                        animation.Stop(keepState: true);
                        break;
                    case "CrouchIdle":
                        if (animation.AssignedAnimation == "Crouch") return;
                        animation.Pause();
                        break;
                    default:
                        GD.PushError($"You forgot to add a switch case for {animationName} stupid");
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
