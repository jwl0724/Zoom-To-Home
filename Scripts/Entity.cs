using Godot;

namespace ZoomToHome {
    public partial class Entity : CharacterBody3D {
        public float MoveSpeed { get; set; }
        public float Gravity { get; set; }
        public float SprintMultiplier { get; set; }
        public float SFXVolumeMultiplier { get; set; }
    }
}