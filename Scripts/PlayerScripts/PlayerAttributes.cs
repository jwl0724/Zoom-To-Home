using Godot;

// NOTES: UNUSED VARIABLES WILL HAVE 4 REFERENCES CAUSE OF GODOT
namespace ZoomToHome {
    public partial class Player : Entity {
        // STAT VARIABLES
        public float Height { get; set; } = 2f;
        public float CrouchHeight { get; set; } = 0.25f;
        public float DefaultCapsuleRadius { get; set; } = 0.5f;
        

        // PHYSICS VARIABLES
        public float JumpSpeed { get; set; } = 18f;
        public float ZipRetractSpeed { get; set; } = 100f;
        public float SwingRetractSpeed { get; set; } = 20f;
        public float SlideFrictionCoefficient { get; set; } = 0.995f;

        // MOUSE VARIABLES
        public float MouseSensitivity { get; set; } = 0.1f;

        // SOUND VARIABLES
        public float MusicVolumeMultiplier { get; set; } = 1f;

        // TIMER VARIABLES
        public float ZipCooldown { get; set; } = 1f; // in seconds
        public float ZipTimer { get; set; } = 0f;

        // HELPER METHODS RELATED TO ATTRIBUTES
        private void OverrideParentVariables() {
            MoveSpeed = 25f;
            Gravity = 40f;
            SprintMultiplier = 2f;
            SFXVolumeMultiplier = 1f;
        }
    }
}
