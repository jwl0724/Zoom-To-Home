
using Godot;

namespace ZoomToHome {
    public partial class Sprinting : State {
        [Export] Idle idleState;
        [Export] Running runningState;
        private Player player;
        public override void _Ready() {
            player = parentBody as Player;
        }
        public override void EnterState() {
            player.SprintMultiplier = 1.5f;
        }

        public override void ExitState() {
            player.SprintMultiplier = 1f;
        }

        protected override void ProcessAction() {
            if (Input.IsActionPressed("sprint") && manager.CurrentState is not Sprinting)
                EmitSignal(SignalName.UpdateState, this);
            if (Input.IsActionJustReleased("sprint")) {
                if (player.Velocity.IsZeroApprox()) EmitSignal(SignalName.UpdateState, idleState);
                else EmitSignal(SignalName.UpdateState, runningState);
            }
        }

        public override void _Process(double delta) {
            ProcessAction();
        }
    }

}
