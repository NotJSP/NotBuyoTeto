using UnityEngine;
using NotTetrin.Ingame.Marathon;

namespace NotTetrin.Ingame.MultiPlay.Marathon {
    [RequireComponent(typeof(NetworkDirector))]
    public class NetworkMarathonDirector : MarathonDirector {
        [SerializeField] GameObject[] sideWalls;
        [SerializeField] CollidersField[] collidersFields;
        [SerializeField] GarbageIndicator[] garbageIndicators;

        private NetworkDirector director;

        public override GameObject SideWall => sideWalls[director.PlayerIndex];
        public override CollidersField CollidersField => collidersFields[director.PlayerIndex];
        public GarbageIndicator GarbageIndicator => garbageIndicators[director.PlayerIndex];

        private void Awake() {
            director = GetComponent<NetworkDirector>();
        }
    }
}
