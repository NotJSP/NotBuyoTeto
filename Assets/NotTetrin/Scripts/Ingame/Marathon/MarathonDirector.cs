using UnityEngine;

namespace NotTetrin.Ingame.Marathon {
    public abstract class MarathonDirector : MonoBehaviour {
        public abstract GameObject SideWall { get; }
        public abstract CollidersField CollidersField { get; }
    }
}
