using NotTetrin.Ingame.Marathon;
using UnityEngine;

namespace NotTetrin.Ingame.SinglePlay.Marathon {
    public class LocalMarathonDirector : MarathonDirector {
        [SerializeField] GameObject sideWall;
        [SerializeField] CollidersField collidersField;

        public override GameObject SideWall => sideWall;
        public override CollidersField CollidersField => collidersField;
    }
}
