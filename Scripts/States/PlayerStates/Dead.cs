using Godot;

namespace ZoomToHome {
    public partial class Dead : State {
        private Player player;

        public override void _Ready() {
            player = parentBody as Player;
            player.Connect(Entity.SignalName.Damaged, Callable.From(() => ProcessAction()));
        }

        public override void EnterState() {
            // ADD STUFF WHEN VISUALS ARE IMPLEMENTED
        }

        public override void ExitState() {
            // ADD STUFF WHEN VISUALS ARE IMPLEMENTED
        }

        protected override void ProcessAction() {
            EmitSignal(SignalName.UpdateState, this);
        }
    }
}