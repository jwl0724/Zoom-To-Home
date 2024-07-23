using Godot;

// NOTES: UNUSED VARIABLES WILL HAVE 4 REFERENCES CAUSE OF GODOT
namespace ZoomToHome {
    public partial class Player : Entity {
        // STAT VARIABLES

        // PHYSICS VARIABLES
        public float JumpSpeed { get; set; } = 18f;
        public float ZipRetractSpeed { get; set; } = 100f;
        public float SwingRetractSpeed { get; set; } = 200f;
        public float FrictionCoefficient { get; set; } = 0.75f;

        // MOUSE VARIABLES
        public float MouseSensitivity { get; set; } = 0.1f;

        // SOUND VARIABLES
        public float MusicVolumeMultiplier { get; set; } = 1f;

        // TIMER VARIABLES


        // HELPER METHODS RELATED TO ATTRIBUTES
        private void OverrideParentVariables() {
            MoveSpeed = 25f;
            Gravity = 20f;
            SprintMultiplier = 2f;
            SFXVolumeMultiplier = 1f;
        }
    }
}
