using System.Collections.Generic;
using Godot;

namespace ZoomToHome {
    public partial class Player : Entity {
        public StateManager StateManager;
        private PlayerRotationManager rotationHelper;
        private readonly LinkedList<Vector3> forceList = new();

        public override void _Ready() {
            OverrideParentVariables();

            // connect essential nodes
            rotationHelper = GetNode<PlayerRotationManager>("Rotational Helper");
            StateManager = GetNode<StateManager>("State Manager");

            // connect signals
            InputManager manager = GetNode<InputManager>("Script Helpers/Input Manager");
            manager.Connect(InputManager.SignalName.Jump, 
                Callable.From(() => ApplyForce(Vector3.Up * JumpSpeed, isOneShot: true))
            );
            manager.Connect(InputManager.SignalName.Move, Callable.From((Vector2 inputVector) => OnMove(inputVector)));
            manager.Connect(InputManager.SignalName.Sprint, Callable.From(() => OnSprint()));
            manager.Connect(InputManager.SignalName.Swing, Callable.From(() => OnSwing()));
            manager.Connect(InputManager.SignalName.Zip, Callable.From(() => OnZip()));
            manager.Connect(InputManager.SignalName.Crouch, Callable.From(() => OnCrouch()));
        }

        public override void _PhysicsProcess(double delta) {
            ApplyForce(Vector3.Down * Gravity, isOneShot: false);
            foreach (Vector3 force in forceList) {
                Velocity += force;
            }
            forceList.Clear();
            MoveAndSlide();
        }

        private void OnSprint() {
            if (SprintMultiplier == 1.5f) SprintMultiplier = 1;
            else SprintMultiplier = 1.5f;
        }
        
        private void OnSwing() {

        }

        private void OnZip() {

        }

        private void OnCrouch() {

        }

        private void OnMove(Vector2 inputVector) {
            Vector3 forwardVector = rotationHelper.Transform.Basis * new Vector3(inputVector.X, 0, inputVector.Y);
            Vector3 movementDirection = forwardVector * MoveSpeed * SprintMultiplier;
            Velocity = new (movementDirection.X, Velocity.Y, movementDirection.Z);
        }

        // TODO, ADD SOME SORT OF DISABLING CONTROL MECHANICS WHEN FORCE IS APPLIED
        public void ApplyForce(Vector3 force, bool isOneShot) {
            if (isOneShot) forceList.AddFirst(force);
            else forceList.AddFirst(force * (float) GetProcessDeltaTime());
        }
    }
}
