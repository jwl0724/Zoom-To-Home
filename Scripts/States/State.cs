using Godot;

namespace ZoomToHome {
    public abstract partial class State : Node {
        [Export] protected PhysicsBody3D parentBody;
        [Export] protected StateManager manager;
        [Signal] public delegate void UpdateStateEventHandler();
        public abstract void EnterState();
        public abstract void ExitState();
        protected abstract void ProcessAction();
    }
}
