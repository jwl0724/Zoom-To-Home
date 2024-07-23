using Godot;

namespace ZoomToHome { // MARKED FOR DELETION MAYBE, NEED TO SEE LATER
    public partial class Jumping : State {
        private Player player;

        public override void _Ready() {
            player = parentBody as Player;
        }

        public override void EnterState() {
            player.ApplyForce(Vector3.Up * player.JumpSpeed, isOneShot: true);
        }

        public override void ExitState() {
            
        }

        public override void ProcessInput(InputEvent inputEvent) {
            if (Input.IsActionJustPressed("zip")) manager.ChangeState(manager.AllStates["Zipping"]);
        }

        public override void ProcessFrame(double delta) {

        }

        public override void ProcessPhysics(double delta) {
            player.ApplyMidAirInputs(player.MoveSpeed * 1.5f);
            player.ApplyForce(Vector3.Down * player.Gravity, isOneShot: false);
            player.SumForces();
            player.MoveAndSlide();

            if (player.Velocity.Y < 0) manager.ChangeState(manager.AllStates["Falling"]);
        }
    }
}