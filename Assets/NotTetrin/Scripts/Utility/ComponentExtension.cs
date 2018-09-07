using UnityEngine;

namespace NotTetrin.Utility {
    public static class ComponentExtension {
        public static Rigidbody2D CopyOf(this Rigidbody2D dst, Rigidbody2D src) {
            dst.bodyType = src.bodyType;
            dst.sharedMaterial = src.sharedMaterial;
            dst.simulated = src.simulated;
            dst.useAutoMass = src.useAutoMass;
            dst.mass = src.mass;
            dst.drag = src.drag;
            dst.angularDrag = src.angularDrag;
            dst.gravityScale = src.gravityScale;
            dst.collisionDetectionMode = src.collisionDetectionMode;
            dst.sleepMode = src.sleepMode;
            dst.interpolation = src.interpolation;
            return dst;
        }
    }
}
