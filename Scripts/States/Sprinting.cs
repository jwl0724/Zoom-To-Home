
using Godot;

namespace ZoomToHome {
    public partial class Sprinting : State {
        public override void _PhysicsProcess(double delta) {
            if (parentBody.SprintMultiplier > 1) SetSprint();
        }
        private void SetSprint() {
            if (manager.State is Sprinting) return;
            EmitSignal(SignalName.UpdateState, this);
        }
    }

}
