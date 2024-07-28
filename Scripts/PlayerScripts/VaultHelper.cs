using Godot;
using System;

namespace ZoomToHome {
    public partial class VaultHelper : Node3D {
        [Export] private PlayerRotationManager rotationalHelper;
        private RayCast3D vaultRaycast;

        public override void _Ready() {
            vaultRaycast = GetNode<RayCast3D>("Vault Raycast");
        }

        public override void _Process(double delta) {
            GlobalRotation = Vector3.Up * rotationalHelper.Rotation.Y;
            GlobalPosition = (Owner as Player).GlobalPosition;
        }

        public bool CanVault() {
            return vaultRaycast.IsColliding();
        }

        // POSSIBLE BUG WHERE YOU CLIP INTO GROUND, MARKED FOR OBSERVATION
        public Vector3 CalculateVaultLocation() {
            Vector3 forwardVector = rotationalHelper.Transform.Basis * Vector3.Forward;
            Vector3 flattenedVectorForward = new (forwardVector.X, 0, forwardVector.Z);
            flattenedVectorForward = flattenedVectorForward.Normalized() * 1.5f;

            Vector3 vaultDestination = vaultRaycast.GetCollisionPoint() + flattenedVectorForward;
            return vaultDestination;
        }
    }
}
