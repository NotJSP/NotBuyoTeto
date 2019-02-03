using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Utility;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.BuyoBuyo {
    public class BuyoGarbageManager : GarbageManager {
        [SerializeField]
        private Instantiator instantiator;
        [SerializeField]
        private BuyoResolver resolver;
        [SerializeField]
        private BuyoSpawner spawner;
        [SerializeField]
        private Transform ceiling;

        [Header("Prefabs")]
        [SerializeField]
        private Rigidbody2D buyoRigidbody;

        private static float OffsetRange = 0.9f;
        private static float TorqueRange = 120.0f;
        private static Vector2 ForceGarbage = new Vector2(0, -80.0f);

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
                var buyoType = (BuyoType)index;
                var ceilingPosition = ceiling.position;
                var offset = Random.Range(-OffsetRange, OffsetRange);
                var spawnPosition = new Vector2(ceilingPosition.x + offset, ceilingPosition.y);
                var obj = spawner.Spawn(buyoType, spawnPosition, 0);
                var rigidbody = obj.AddComponent<Rigidbody2D>().CopyOf(buyoRigidbody);
                var torque = Random.Range(-TorqueRange, TorqueRange);
                obj.layer = LayerMask.NameToLayer("Default");
                rigidbody.isKinematic = false;
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
