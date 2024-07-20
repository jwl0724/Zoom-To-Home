namespace ZoomToHome { // MARKED FOR DELETION MAYBE, NEED TO SEE LATER
    public partial class Falling : State {
        public override void _PhysicsProcess(double delta) {
            if (!parentBody.IsOnFloor() && parentBody.Velocity.Y < 0) SetFalling();
        }
        private void SetFalling() {
            if (manager.State is Falling) return;
            else EmitSignal(State.SignalName.UpdateState, this);
        }
    }
}