using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotTetrin.Utility.Tiling {
    public class TileHelper {
        public Rect Rect { get; private set; }
        public Vector2 Size { get; private set; }
        public int DivideNumX { get; private set; }
        public int DivideNumY { get; private set; }

        public Vector2 Rate { get; private set; }
        public Vector2 Unit { get; private set; }

        public static TileHelper Calculate(Rect rect, Vector2 size, int devideNumX, int devideNumY) {
            var distance = new Vector2(rect.xMax - rect.xMin, rect.yMax - rect.yMin);
            var unit = new Vector2(distance.x / devideNumX, distance.y / devideNumY);
            var rate = new Vector2(unit.x / size.x, unit.y / size.y);

            return new TileHelper {
                Rect = rect,
                Size = size,
                DivideNumX = devideNumX,
                DivideNumY = devideNumY,
                Rate = rate,
                Unit = unit
            };
        }
    }
}
