using Godot;

namespace ZoomToHome {
    public partial class StateManager : Node {
        public State CurrentState { get; private set; }
        public override void _Ready() {
            foreach (State child in GetChildren()) {
                child.Connect(State.SignalName.UpdateState, 
                    Callable.From((State newState) => ChangeState(newState))
                );
            }
        }

        private void ChangeState(State state) {
            CurrentState?.ExitState();
            CurrentState = state;
            state.EnterState();
        }
    }
}

