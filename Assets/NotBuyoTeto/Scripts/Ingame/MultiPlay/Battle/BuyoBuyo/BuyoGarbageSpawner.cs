using System.Collections;
using UnityEngine;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.BuyoBuyo {
    public class BuyoGarbageSpawner : GarbageSpawner {
        [SerializeField]
        private BuyoManager buyoManager;
        [SerializeField]
        private BuyoResolver resolver;
        [SerializeField]
        private BuyoSpawner spawner;
        [SerializeField]
        private Transform ceiling;

        private static float OffsetRange = 1.5f;
        private static float TorqueRange = 60.0f;
        private static Vector2 ForceGarbage = new Vector2(0, -10.0f);

        public override void Clear() {
        }

        public override void Spawn(int count) {
            IsSpawning = true;
            StartCoroutine(fallCoroutine(count));
        }

        private IEnumerator fallCoroutine(int count) {
            for (int i = 0; i < count; i++) {
                var index = Random.Range(0, resolver.Length);
                var buyoType = (BuyoType)index;
                var ceilingPosition = ceiling.position;
                var offset = Random.Range(-OffsetRange, OffsetRange);
                var spawnPosition = new Vector2(ceilingPosition.x + offset, ceilingPosition.y);
                var obj = spawner.Spawn(buyoType, spawnPosition, 0);
                obj.layer = LayerMask.NameToLayer("Default");
                buyoManager.Add(obj);
                var rigidbody = obj.GetComponent<Rigidbody2D>();
                var torque = Random.Range(-TorqueRange, TorqueRange);
                rigidbody.isKinematic = false;
                rigidbody.AddTorque(torque);
                rigidbody.AddForce(ForceGarbage);
                yield return new WaitForSeconds(0.45f);
            }
            yield return new WaitForSeconds(0.8f);
            IsSpawning = false;
        }
    }
}
