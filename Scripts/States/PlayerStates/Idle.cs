namespace ZoomToHome {
    public partial class Idle : State {

        private Player player;
        public override void _Ready() {
            player = parentBody as Player;
        }

        public override void EnterState() {
            // TO BE IMPLEMENTED WHEN VISUALS ARE ADDED
        }

        public override void ExitState() {
            // TO BE IMPLEMENTED WHEN VISUALS ARE ADDED
        }

        protected override void ProcessAction() {
            if (manager.CurrentState is not Idle && player.Velocity.IsZeroApprox())
                EmitSignal(SignalName.UpdateState, this);
        }

        public override void _PhysicsProcess(double delta) {
            ProcessAction();
        }
    }
}
