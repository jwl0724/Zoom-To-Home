using Godot;

namespace ZoomToHome { // MARKED FOR DELETION MAYBE, NEED TO SEE LATER
    public partial class Falling : State {
        private Player player;

        public override void _Ready() {
            player = parentBody as Player;
        }
        
        public override void EnterState() {
            
        }

        public override void ExitState() {
            
        }

        public override void ProcessInput(InputEvent inputEvent) {
        }

        public override void ProcessFrame(double delta) {

        }

        public override void ProcessPhysics(double delta) {
            player.ApplyMidAirInputs(player.MoveSpeed * 1.5f);
            player.ApplyForce(Vector3.Down * player.Gravity, isOneShot: false);
            player.SumForces();
            player.MoveAndSlide();

            if (player.IsOnFloor()) {
                if (Input.GetVector("left", "right", "forward", "backward").IsZeroApprox())
                    manager.ChangeState(manager.AllStates["Idle"]);
                else if (Input.IsActionPressed("sprint"))
                    manager.ChangeState(manager.AllStates["Sprinting"]);
                else manager.ChangeState(manager.AllStates["Running"]);
            }
        }
    }
}