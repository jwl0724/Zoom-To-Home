using Godot;
using System;
using System.Linq;
using System.Reflection;

namespace ZoomToHome {
    public partial class ArmsAnimator : Node3D {
        [Export] private Player player;
        private HandAnimator leftHand;
        private HandAnimator rightHand;
        private string[] allowedAnimations = {"Fall", "Sprint", "WallRun", "Reset", "Jump", "Zip", "Swing"};

        // animation arm positions
        private static Vector3 defaultHandRotation = new(0, -Mathf.DegToRad(180), 0);
        private static Vector3 fallArmsRotation = new(Mathf.DegToRad(140), 0, 0);
        private static Vector3 leftHandDefaultPosition = new(-0.8f, -0.4f, -0.4f);
        private static Vector3 rightHandDefaultPosition = new(0.8f, -0.4f, -0.4f);
        private static Vector3 leftHandSprintRotation = new(0, Mathf.DegToRad(180), Mathf.DegToRad(30));
        private static Vector3 rightHandSprintRotation = new(0, Mathf.DegToRad(180), Mathf.DegToRad(-30));
        private static Vector3 leftHandPullPosition = new(-0.65f, -0.2f, -0.4f);
        private static Vector3 rightHandPullPosition = new(0.65f, -0.2f, -0.4f);
        private static Vector3 leftHandJumpPosition = new(-1.5f, -0.4f, 0.2f);
        private static Vector3 leftHandJumpRotation = new(Mathf.DegToRad(-49), Mathf.DegToRad(20), Mathf.DegToRad(135));
        private static Vector3 rightHandJumpPosition = new(0.9f, -0.7f, 0.2f);
        private static Vector3 rightHandJumpRotation = new(Mathf.DegToRad(-38), Mathf.DegToRad(-38), Mathf.DegToRad(-147));

        public override void _Ready() {
            leftHand = GetNode("Left Hand") as HandAnimator;
            rightHand = GetNode("Right Hand") as HandAnimator;
        }

        public void Play(string animationName) {
            if (!allowedAnimations.Contains(animationName)) {
                GD.PushError($"{animationName} does not exist in the library");
                return;
            }

            MethodInfo animationMethod = GetType().GetMethod($"Play{animationName}");
            if (animationMethod == null) GD.PushError($"You mistyped a method name stupid");
            else animationMethod.Invoke(this, null);

            // switch(animationName) {
            //     case "Fall":
            //         PlayFall();
            //         break;
            //     case "Sprint":
            //         PlaySprint();
            //         break;
            //     case "WallRun":
            //         PlayWallRun();
            //         break;
            //     case "Reset":
            //         PlayReset();
            //         break;
            //     case "Jump":
            //         PlayJump();
            //         break;
            //     case "Zip":
            //         PlayZip();
            //         break;
            //     case "Swing":
            //         PlaySwing();
            //         break;
            //     case "Crouch":
            //         PlayCrouch();
            //         break;
            //     default:
            //         GD.PushError($"You forgot to add a switch case for {animationName} stupid");
            //         break; 
            // }
        }

        private void PlayFall() {
            Tween fallTween = CreateTween();
            fallTween.TweenProperty(this, "rotation", fallArmsRotation, 0.2f);
            fallTween.Play();
        }

        private void PlaySprint() {
            ResetOrientation();
            leftHand.Rotation = leftHandSprintRotation;
            rightHand.Rotation = rightHandSprintRotation;
            leftHand.Play("Sprint", 2);
            rightHand.Play("Sprint", -2);
        }

        private void PlayWallRun() {
            GD.Print("Need to figure out if I still want this lel");
        }
        
        private void PlayReset() {
            ResetOrientation();
            leftHand.Play("Reset");
            rightHand.Play("Reset");
        }

        private void PlayJump() {
            ResetOrientation();
            Tween jumpTween = CreateTween();
            jumpTween.TweenProperty(leftHand, "rotation", leftHandJumpRotation, 0.2f);
            jumpTween.TweenProperty(leftHand, "position", leftHandJumpPosition, 0.2f);
            jumpTween.TweenProperty(rightHand, "rotation", rightHandJumpRotation, 0.2f);
            jumpTween.TweenProperty(rightHand, "position", rightHandJumpPosition, 0.2f);
            jumpTween.Play();
        }

        private void PlayZip() {
            ResetOrientation();
            leftHand.Position = leftHandPullPosition;        
            leftHand.Play("Pull");
        }

        private void PlaySwing() {
            ResetOrientation();
            rightHand.Position = rightHandPullPosition;
            rightHand.Play("Pull");
        }

        private void ResetOrientation() {
            Rotation = Vector3.Zero;
            leftHand.Position = leftHandDefaultPosition;
            rightHand.Position = rightHandDefaultPosition;
            leftHand.Rotation = defaultHandRotation;
            rightHand.Rotation = defaultHandRotation;
        }
    }
}
