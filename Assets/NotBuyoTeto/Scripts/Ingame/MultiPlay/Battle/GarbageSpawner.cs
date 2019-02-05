using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public abstract class GarbageSpawner : MonoBehaviour {
        public bool IsSpawning { get; protected set; }

        public abstract void Clear();
        public abstract void Spawn(int count);
    }
}
