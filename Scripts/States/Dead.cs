using Godot;

namespace ZoomToHome {
    public partial class Dead : State {
        public override void _Ready() {
            manager = GetParent() as StateManager;
            parentBody = Owner as Entity;
            parentBody.Connect(Entity.SignalName.Damaged, Callable.From(() => SetDead()));
        }
        private void SetDead() {
            if (manager.State is Dead) return;
            else EmitSignal(State.SignalName.UpdateState, this);
        }
    }
}