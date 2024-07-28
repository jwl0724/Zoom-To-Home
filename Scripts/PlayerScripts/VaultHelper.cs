using Godot;
using System;

namespace ZoomToHome {
    public partial class VaultHelper : Node3D {
        private RayCast3D topRaycast;
        private RayCast3D botRaycast;

        public override void _Ready() {
            topRaycast = GetNode<RayCast3D>("Top");
            botRaycast = GetNode<RayCast3D>("Bottom");
        }

        public bool ValidVaulting() {
            return !topRaycast.IsColliding() && botRaycast.IsColliding();
        }


    }
}
