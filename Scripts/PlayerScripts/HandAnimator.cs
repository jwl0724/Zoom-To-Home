using Godot;

namespace ZoomToHome {
    public partial class HandAnimator : Node3D {
        [Export] private AnimationPlayer animation;

        public void Play(string animationName, bool playOver = true) {
            if(!playOver && animation.IsPlaying()) return;
            animation.Play(animationName);
        }
    }
}
