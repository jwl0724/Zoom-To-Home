using System.Collections.Generic;
using Godot;

namespace ZoomToHome {
    public partial class Player : Entity {
        public StateManager StateManager;
        public PlayerRotationManager RotationHelper;
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

        public void ApplyForce(Vector3 force, bool isOneShot) {
            if (isOneShot) forceList.AddFirst(force);
            else forceList.AddFirst(force * (float) GetProcessDeltaTime());
        }
    }
}
