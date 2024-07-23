using System.Collections.Generic;
using Godot;

namespace ZoomToHome {
    public partial class Player : Entity {
        public StateManager StateManager;
        private PlayerRotationManager RotationHelper;
        private readonly LinkedList<Vector3> forceList = new();

        public override void _Ready() {
            OverrideParentVariables();

            // connect essential nodes
            RotationHelper = GetNode<PlayerRotationManager>("Rotational Helper");
            StateManager = GetNode<StateManager>("State Manager");
        }

        public void SumForces() {
            foreach (Vector3 force in forceList) {
                Velocity += force;
            }
            forceList.Clear();
        }

        public Vector3 GetRaycastImpactPoint() {
            return RotationHelper.GetRaycastImpactPoint();
        }

        public void ApplyForce(Vector3 force, bool isOneShot) {
            if (isOneShot) forceList.AddFirst(force);
            else forceList.AddFirst(force * (float) GetProcessDeltaTime());
        }

        public void SetVelocityToInputVector(float moveSpeed) {
            Vector2 inputVector = Input.GetVector("left", "right", "forward", "backward");
            Vector3 movementDirection = GetForwardVectorOnHorizontalPlane(new Vector3(inputVector.X, 0, inputVector.Y), moveSpeed);
            Velocity = new(movementDirection.X, Velocity.Y, movementDirection.Z);
        }

        public void ApplyMidAirInputs(float adjustSpeed) {
            Vector2 inputVector = Input.GetVector("left", "right", "forward", "backward");
            if (inputVector.IsZeroApprox()) return;

            Vector3 horizontalVelocity = new(Velocity.X, 0, Velocity.Z);
            Vector3 movementDirection = GetForwardVectorOnHorizontalPlane(new Vector3(inputVector.X, 0, inputVector.Y), adjustSpeed);
            Vector3 summedVector = horizontalVelocity + movementDirection * (float) GetProcessDeltaTime();

            if (Mathf.Abs(summedVector.Length()) < MoveSpeed || movementDirection.Dot(horizontalVelocity) <= 0)
                ApplyForce(movementDirection, isOneShot: false);
        }

        public Vector3 GetForwardVectorOnHorizontalPlane(Vector3 vector, float moveSpeed) {
            Vector3 forwardVector = RotationHelper.Transform.Basis * new Vector3(vector.X, 0, vector.Z);
            Vector3 normalizedVector = new(forwardVector.X, 0, forwardVector.Z);
            return normalizedVector.Normalized() * moveSpeed;
        }
    }
}
