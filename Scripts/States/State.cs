using Godot;

namespace ZoomToHome {
    public abstract partial class State : Node {
        protected Entity parentBody;
        protected StateManager manager;
        [Signal] public delegate void UpdateStateEventHandler();
        public override void _Ready() {
            manager = GetParent() as StateManager;
            parentBody = Owner as Entity;
        }
    }
}
