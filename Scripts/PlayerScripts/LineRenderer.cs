using Godot;
using System;

namespace ZoomToHome {
    public partial class LineRenderer : Node {
        [Export] private StandardMaterial3D lineMaterial;

        public void RenderLine(Vector3 globalPoint1, Vector3 globalPoint2, int radius, int vertices) {
            for (int i = 0; i < vertices; i++) {

            }
        }
    }

}
