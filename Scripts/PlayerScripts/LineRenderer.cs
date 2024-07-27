using Godot;
using System;

namespace ZoomToHome {
    public partial class LineRenderer : Node {
        [Export] private StandardMaterial3D lineMaterial;
        private ImmediateMesh line = new();
        private Color lineColor;
        private Tween currentTween;

        public override void _Ready() {
            lineColor = lineMaterial.AlbedoColor;

            MeshInstance3D lineMesh = new() {
                Mesh = line, 
                MaterialOverride = lineMaterial, 
                CastShadow = GeometryInstance3D.ShadowCastingSetting.Off
            };
            (Engine.GetMainLoop() as SceneTree).Root.CallDeferred(MethodName.AddChild, lineMesh);
        }

        public void RenderLine(Vector3 globalPoint1, Vector3 globalPoint2, float radius, uint vertices) {
            if (360 % vertices > 0) GD.PushError($"Cannot render line, {vertices} is not a multiple of 360");

            // undo tween changes
            if (currentTween != null && currentTween.IsRunning()) currentTween.Kill();
            lineMaterial.AlbedoColor = new Color(lineColor.R, lineColor.G, lineColor.B, 1);
            float angleMultiple = Mathf.DegToRad(360 / vertices);

            line.ClearSurfaces(); // clear previous lines
            line.SurfaceBegin(Mesh.PrimitiveType.TriangleStrip);
            
            for (int i = 0; i < vertices; i++) {
                float firstVertexXOffset = Mathf.Cos(angleMultiple * i) * radius;
                float firstVertexYOffset = Mathf.Sin(angleMultiple * i) * radius;
                float secondVertexXOffset = Mathf.Cos(angleMultiple * (i + 1)) * radius;
                float secondVertexYOffset = Mathf.Sin(angleMultiple * (i + 1)) * radius;

                // add first point
                line.SurfaceAddVertex(globalPoint1 + Vector3.Right * firstVertexXOffset + Vector3.Up * firstVertexYOffset);

                // add second point
                line.SurfaceAddVertex(globalPoint1 + Vector3.Right * secondVertexXOffset + Vector3.Up * secondVertexYOffset);

                // add third point
                line.SurfaceAddVertex(globalPoint2 + Vector3.Right * secondVertexXOffset + Vector3.Up * secondVertexYOffset);

                // go back to original point
                line.SurfaceAddVertex(globalPoint1 + Vector3.Right * firstVertexXOffset + Vector3.Up * firstVertexYOffset);

                // add fourth point
                line.SurfaceAddVertex(globalPoint2 + Vector3.Right * firstVertexXOffset + Vector3.Up * firstVertexYOffset);

                // go back to original point on other point
                line.SurfaceAddVertex(globalPoint2 + Vector3.Right * secondVertexXOffset + Vector3.Up * secondVertexYOffset);
            }
            line.SurfaceEnd();
        }

        public void ClearLine() {
            currentTween = CreateTween();
            currentTween.TweenProperty(lineMaterial, "albedo_color", new Color(lineColor.R, lineColor.G, lineColor.B, 0), 0.5f);
            currentTween.TweenCallback(Callable.From(() => line.ClearSurfaces()));
        }
    }
}
