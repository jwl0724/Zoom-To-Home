using Godot;

namespace ZoomToHome {
    public abstract partial class State : Node {
        [Export] protected PhysicsBody3D parentBody;
        [Export] protected StateManager manager;
        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void ProcessInput(InputEvent inputEvent);
        public abstract void ProcessFrame(double delta);
        public abstract void ProcessPhysics(double delta);
    }
}
