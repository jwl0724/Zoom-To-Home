using Godot;

namespace ZoomToHome {
    public partial class GoalRing : Node3D {
        [Export] private Vector3 winAreaSize;
        private Area3D winArea;
        private Node3D ringVisuals;

        public override void _Ready() {
            winArea = GetNode("Win Area") as Area3D;
            ringVisuals = GetNode("Ring") as Node3D;

            CollisionShape3D winCollision = winArea.GetNode("CollisionShape3D") as CollisionShape3D;
            BoxShape3D collisionBox = winCollision.Shape as BoxShape3D;
            if (winAreaSize.IsZeroApprox()) collisionBox.Size = new Vector3(5, 40, 40);
            else collisionBox.Size = winAreaSize;
            
            CollisionShape3D areaShape = winArea.GetNode("CollisionShape3D") as CollisionShape3D;
            areaShape.Shape = collisionBox;

            winArea.Connect(Area3D.SignalName.BodyEntered, Callable.From((Node3D body) => EndLevel(body)));
        }

        public override void _PhysicsProcess(double delta) {
            ringVisuals.Rotation += Vector3.Up * (float) delta;
            ringVisuals.Position += Vector3.Up * 0.05f * Mathf.Cos(ringVisuals.Rotation.Y * 1.5f);
        }

        private static void EndLevel(Node3D body) {
            if (body is not Player player) return;
            player.FinishLevel();
        }
    }
}
