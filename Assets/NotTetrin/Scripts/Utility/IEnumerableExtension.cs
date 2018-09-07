using System;
using System.Collections.Generic;
using System.Linq;

namespace NotTetrin.Utility {
    public static class IEnumerableExtension {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection) {
            return collection.OrderBy(i => Guid.NewGuid());
        }
    }
}
