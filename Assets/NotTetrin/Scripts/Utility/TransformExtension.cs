using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Utility {
    public static class TransformExtension {
        public static string path(this Transform self) {
            var path = self.name;
            var parent = self.transform.parent;
            while (parent != null) {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }
            return path;
        }
    }
}
