using Godot;

namespace ZoomToHome {
    public partial class Running : State {
        private Player player;
        public override void _Ready() {
            player = parentBody as Player;
        }
        
        public override void EnterState() {
            // TODO IMPLEMENT STUFF LATER WHEN VISUALS ARE ADDED
        }

        public override void ExitState() {
            // TODO IMPLEMENT STUFF LATER WHEN VISUALS ARE ADDED
        }

        protected override void ProcessAction() {
            if (!player.Velocity.IsZeroApprox() && manager.CurrentState is not Sprinting && manager.CurrentState is not Running) {
                EmitSignal(SignalName.UpdateState, this);
            }
        }

        public override void _PhysicsProcess(double delta) {
            ProcessAction();
        }
    }
}
