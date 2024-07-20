using Godot;

namespace ZoomToHome {
    public partial class StateManager : Node {
        public State State { get; private set; }
        public override void _Ready() {
            foreach (State child in GetChildren()) {
                child.Connect(State.SignalName.UpdateState, 
                    Callable.From((State newState) => State = newState)
                );
            }
        }
    }
}

