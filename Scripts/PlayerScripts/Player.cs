using System.Collections.Generic;
using Godot;
// FIX BUG WHERE PLAYER DOESNT MOVE FORWARD AFTER NON-SPRINT JUMP AND LAND EVEN THO KEY IS PRESSED
namespace ZoomToHome {
    public partial class Player : Entity {
        [Export] private ArmsAnimator armsAnimator;
        public StateManager StateManager;
        private PlayerRotationManager RotationHelper;
        private RayCast3D standChecker;
        private CapsuleShape3D capsuleShape;
        private VaultHelper vaultHelper;
        private readonly LinkedList<Vector3> forceList = new();

        public override void _Ready() {
            OverrideParentVariables();

            // connect essential nodes
            RotationHelper = GetNode<PlayerRotationManager>("Rotational Helper");
            StateManager = GetNode<StateManager>("State Manager");
            capsuleShape = GetNode<CollisionShape3D>("Collision Box").Shape as CapsuleShape3D;
            standChecker = GetNode<RayCast3D>("Helper Objects/Stand Check");
            vaultHelper = GetNode<VaultHelper>("Helper Objects/Vault Helper");
        }

        public void EnforceRotation(bool enforce, Vector3 lookPoint = new(), float enforceWeight = -1, bool globalLookPoint = true) {
            if (enforce) {
                if (lookPoint.IsZeroApprox() || enforceWeight < 0) {
                    GD.PushError("Incorrect usage of EnforceRotation(), please set valid values in parameters");
                    return;
                }
                RotationHelper.EnableRotationEnforcement = true;
                RotationHelper.LookPoint = lookPoint;
                RotationHelper.IsGlobalPoint = globalLookPoint;
                RotationHelper.EnforceWeight = enforceWeight;

            } else RotationHelper.EnableRotationEnforcement = false;
        }

        public void EnableInputRotation(bool enable) {
            RotationHelper.EnableInput = enable;
        }

        public void PlayAnimation(string animationName) {
            armsAnimator.Play(animationName);
        }

        public void PlayWallRun(bool isLeft) {
            armsAnimator.PlayWallRun(isLeft);
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

        public float GetForwardVelocityHorizontalMagnitude() {
            Vector3 forwardVector = RotationHelper.Transform.Basis * new Vector3(Velocity.X, 0, Velocity.Z);
            Vector3 normalizedVector = new(forwardVector.X, 0, forwardVector.Z);
            return (normalizedVector.Normalized() * Velocity.Length()).Length();
        }

        public bool ClipsIntoCeilingOnStand() {
            if (standChecker.GetCollider() == null) return false;
            else return true;
        }

        public void ToggleCrouch(bool crouch) {
            if (crouch) {
                capsuleShape.Height = CrouchHeight;
                GetNode<CollisionShape3D>("Collision Box").Position = Vector3.Down * (Height / 2 - CrouchHeight / 2);
                Tween crouchTween = CreateTween();
                crouchTween.TweenProperty(RotationHelper, "position", Vector3.Down * 0.875f, 0.1f);
            } else {
                capsuleShape.Height = Height;
                capsuleShape.Radius = DefaultCapsuleRadius;
                GetNode<CollisionShape3D>("Collision Box").Position = Vector3.Zero;
                Tween standTween = CreateTween();
                standTween.TweenProperty(RotationHelper, "position", Vector3.Up * 0.75f, 0.1f);
            }
        }

        public bool CanVault() {
            return vaultHelper.CanVault();
        }

        public Vector3 GetVaultDestination() {
            return vaultHelper.CalculateVaultLocation();
        }
    }
}
