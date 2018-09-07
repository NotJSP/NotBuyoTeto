using UnityEngine;

namespace NotTetrin.Utility {
    public static class GameObjectExtension {
        public static Vector3 size(this GameObject obj) {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer) {
                return renderer.bounds.size;
            }
            return obj.transform.localScale;
        }

        public static Rect rect(this GameObject obj) {
            var p = obj.transform.position;
            var s = obj.size();
            return new Rect(p - s / 2, s);
        }

        public static string path(this GameObject obj) => obj.transform.path();
    }
}