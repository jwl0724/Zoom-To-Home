namespace ZoomToHome { // MARKED FOR DELETION MAYBE, NEED TO SEE LATER
    public partial class Jumping : State {
        public override void _PhysicsProcess(double delta) {
            if (!parentBody.IsOnFloor() && parentBody.Velocity.Y > 0) SetJumping();
        }
        private void SetJumping() {
            if (manager.State is Jumping) return;
            else EmitSignal(State.SignalName.UpdateState, this);
        }
    }
}