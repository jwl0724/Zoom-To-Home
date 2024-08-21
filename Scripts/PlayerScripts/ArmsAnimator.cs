using Godot;
using System;
using System.Linq;

// NEED TO IMPLEMENT WALL RUNNING
namespace ZoomToHome {
    public partial class ArmsAnimator : Node3D {
        private HandAnimator leftHand;
        private HandAnimator rightHand;
        private string[] allowedAnimations = {"Sprint", "Reset", "Zip", "Swing", "Vault", "Hide"};

        // animation arm positions
        private static Vector3 defaultHandRotation = new(0, -Mathf.DegToRad(180), 0);
        private static Vector3 leftHandDefaultPosition = new(-0.8f, -0.4f, -0.4f);
        private static Vector3 rightHandDefaultPosition = new(0.8f, -0.4f, -0.4f);
        private static Vector3 leftHandSprintRotation = new(0, Mathf.DegToRad(180), Mathf.DegToRad(30));
        private static Vector3 rightHandSprintRotation = new(0, Mathf.DegToRad(180), Mathf.DegToRad(-30));
        private static Vector3 leftHandPullPosition = new(-0.65f, -0.2f, -0.4f);
        private static Vector3 rightHandPullPosition = new(0.65f, -0.2f, -0.4f);

        public override void _Ready() {
            leftHand = GetNode("Left Hand") as HandAnimator;
            rightHand = GetNode("Right Hand") as HandAnimator;
        }

        public void Play(string animationName) {
            if (!allowedAnimations.Contains(animationName)) {
                GD.PushError($"{animationName} does not exist in the library");
                return;
            }

            switch(animationName) {
                case "Sprint":
                    PlaySprint();
                    break;
                case "Reset":
                    PlayReset();
                    break;
                case "Zip":
                    PlayZip();
                    break;
                case "Swing":
                    PlaySwing();
                    break;
                case "Vault":
                    PlayVault();
                    break;
                case "Hide":
                    PlayHide();
                    break;
                default:
                    GD.PushError($"You forgot to add a switch case for {animationName} stupid");
                    break; 
            }
        }

        public void PlayWallRun(bool isLeft) {
            ResetOrientation();
            if (isLeft) {
                leftHand.Play("WallRun");
                rightHand.Rotation = rightHandSprintRotation;
                rightHand.Play("Sprint", 2);

            } else {
                rightHand.Play("WallRun");
                leftHand.Rotation = leftHandSprintRotation;
                leftHand.Play("Sprint", 2);
            }
        }

        private void PlayHide() {
            leftHand.Visible = false;
            rightHand.Visible = false;
        }

        private void PlaySprint() {
            ResetOrientation();
            leftHand.Rotation = leftHandSprintRotation;
            rightHand.Rotation = rightHandSprintRotation;
            leftHand.Play("Sprint", 2);
            rightHand.Play("Sprint", 2, fromEnd: true);
        }

        private void PlayReset() {
            ResetOrientation();
            leftHand.Play("Reset");
            rightHand.Play("Reset");
        }

        private void PlayZip() {
            ResetOrientation();
            leftHand.Position = leftHandPullPosition;
            rightHand.Play("Reset");
            leftHand.Play("Pull", 2);
        }

        private void PlaySwing() {
            ResetOrientation();
            rightHand.Position = rightHandPullPosition;
            leftHand.Play("Reset");
            rightHand.Play("Pull", 2);
        }

        private void PlayVault() {
            ResetOrientation();
            rightHand.Position = rightHandPullPosition;
            leftHand.Position = leftHandPullPosition;
            rightHand.Play("Pull", -3, fromEnd: true);
            leftHand.Play("Pull", -3, fromEnd: true);
        }

        private void ResetOrientation() {
            leftHand.Visible = true;
            rightHand.Visible = true;
            Rotation = Vector3.Zero;
            Position = Vector3.Zero;
            leftHand.Position = leftHandDefaultPosition;
            rightHand.Position = rightHandDefaultPosition;
            leftHand.Rotation = defaultHandRotation;
            rightHand.Rotation = defaultHandRotation;
        }
    }
}
