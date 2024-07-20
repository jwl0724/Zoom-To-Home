namespace ZoomToHome {
    public partial class Idle : State {
        public override void _PhysicsProcess(double delta) {
            if (parentBody.Velocity.IsZeroApprox()) SetIdle();
        }

        private void SetIdle() {
            if (manager.State is Idle) return;
            EmitSignal(State.SignalName.UpdateState, this);
        }
    }
}
