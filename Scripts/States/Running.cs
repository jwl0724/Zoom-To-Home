using Godot;

namespace ZoomToHome {
    public partial class Running : State {
        public override void _PhysicsProcess(double delta) {
            if (!parentBody.Velocity.IsZeroApprox() && parentBody.IsOnFloor() && parentBody.SprintMultiplier == 1) 
                SetRunning();
        }
        private void SetRunning() {
            if (manager.State is Running) return;
            else EmitSignal(SignalName.UpdateState, this);
        }
    }
}
