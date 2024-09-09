using System.Collections.Generic;
using Godot;

namespace ZoomToHome {
    public partial class StateManager : Node {
        [Signal] public delegate void StateChangeEventHandler();
        public State CurrentState { get; private set; }
        public State PreviousState { get; private set; }
        public Dictionary<string, State> AllStates = new();
        
        public override void _Ready() {
            foreach(Node child in GetChildren()) {
                AllStates.Add(child.GetType().Name, child as State);
            }
            PreviousState = AllStates["Cleared"];
            ChangeState(AllStates["Cleared"]);
        }

        public void ChangeState(State state) {
            if (CurrentState?.GetType() == state.GetType()) return;
            CurrentState?.ExitState();
            PreviousState = CurrentState;
            CurrentState = state;
            state.EnterState();
            EmitSignal(SignalName.StateChange, state);
        }

        public override void _PhysicsProcess(double delta) {
            CurrentState.ProcessPhysics(delta);
        }

        public override void _Input(InputEvent inputEvent) {
            CurrentState.ProcessInput(inputEvent);
        }

        public override void _Process(double delta) {
            CurrentState.ProcessFrame(delta);
        }
    }
}

