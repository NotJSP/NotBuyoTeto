using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Utility;
using NotBuyoTeto.Ingame.Tetrin;
using Random = UnityEngine.Random;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    public class TetoGarbageManager : GarbageManager {
        [SerializeField]
        private Instantiator instantiator;
        [SerializeField]
        private MinoResolver resolver;
        [SerializeField]
        private MinoSpawner spawner;
        [SerializeField]
        private Transform ceiling;

        [Header("Prefabs")]
        [SerializeField]
        private Rigidbody2D minoRigidbody;

        private static float OffsetRange = 0.9f;
        private static float TorqueRange = 150.0f;
        private static Vector2 ForceGarbage = new Vector2(0, -100.0f);

        private List<GameObject> garbages = new List<GameObject>();

        public override void Clear() {
            base.Clear();
            garbages.ForEach(instantiator.Destroy);
        }

        public override void Fall() {
            if (ReadyGarbageCount == 0) { return; }
            IsFalling = true;
            var fallCount = Mathf.Min(ReadyGarbageCount, 10);
            StartCoroutine(fallCoroutine(fallCount));
            ReadyGarbageCount -= fallCount;
        }

        private IEnumerator fallCoroutine(int count) {
            for (int i = 0; i < count; i++) {
                var index = Random.Range(0, resolver.Length);
                var minoType = (MinoType)index;
                var ceilingPosition = ceiling.position;
                var offset = Random.Range(-OffsetRange, OffsetRange);
                var spawnPosition = new Vector2(ceilingPosition.x + offset, ceilingPosition.y);
                var obj = spawner.Spawn(minoType, spawnPosition);
                var rigidbody = obj.AddComponent<Rigidbody2D>().CopyOf(minoRigidbody);
                var torque = Random.Range(-TorqueRange, TorqueRange);
                rigidbody.AddTorque(torque);
                rigidbody.AddForce(ForceGarbage);
                garbages.Add(obj);
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(1.0f);
            IsFalling = false;
        }
    }
}
