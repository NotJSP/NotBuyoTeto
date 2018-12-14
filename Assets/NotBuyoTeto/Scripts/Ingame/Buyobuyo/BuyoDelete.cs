using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class BuyoDelete : MonoBehaviour, IEqualityComparer<BuyoDelete> {
        private BuyoType type;
        private HashSet<BuyoDelete> chainObjects = new HashSet<BuyoDelete>();
        private ParticleSystem BuyoDeleteEffect;

        public event EventHandler<Vector3> DeleteBuyo;

        private void Awake() {
            type = GetComponent<Buyo>().Type;
            BuyoDeleteEffect = gameObject.GetComponentInChildren<ParticleSystem>();
        }

        // Update is called once per frame
        void Update() {
            if (GetChainCount(null) >= 4) {
                DeleteBuyo?.Invoke(this, transform.position);
                DestroyChain(null);
            }
        }

        protected void DestroyChain(HashSet<BuyoDelete> alreadyDestroyed) {
            var destroyObjects = new HashSet<BuyoDelete>(chainObjects);

            if (alreadyDestroyed == null) {
                alreadyDestroyed = new HashSet<BuyoDelete>();
            }
            alreadyDestroyed.Add(this);

            destroyObjects.ExceptWith(alreadyDestroyed);
            alreadyDestroyed.UnionWith(destroyObjects);

            var destroyList = new List<BuyoDelete>(destroyObjects);
            destroyList.ForEach(o => o.DestroyChain(alreadyDestroyed));

            BuyoDeleteEffect.Play();
            gameObject.transform.localScale = Vector3.zero;
            Destroy(gameObject, 1.5f);
            chainObjects.Clear();
        }

        public int GetChainCount(HashSet<BuyoDelete> alreadyCounted) {
            var findObjects = new HashSet<BuyoDelete>(chainObjects);

            if (alreadyCounted == null) {
                alreadyCounted = new HashSet<BuyoDelete>();
            }
            alreadyCounted.Add(this);

            findObjects.ExceptWith(alreadyCounted);

            alreadyCounted.UnionWith(findObjects);
            return findObjects.Sum(o => o.GetChainCount(alreadyCounted)) + 1;
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.collider.CompareTag(@"Wall")) { return; }
            if (collision.collider.CompareTag(@"Floor")) { return; }
            if (collision.collider.CompareTag(@"Parent")) { return; }

            var chainObj = collision.gameObject.GetComponent<BuyoDelete>();

            if (chainObj.type == this.type) {
                chainObjects.Add(chainObj);
            }
        }


        private void OnCollisionExit2D(Collision2D collision) {
            if (collision.collider.CompareTag(@"Wall")) { return; }
            if (collision.collider.CompareTag(@"Floor")) { return; }
            if (collision.collider.CompareTag(@"Parent")) { return; }

            var chainObj = collision.gameObject.GetComponent<BuyoDelete>();

            if (chainObj.type == this.type) {
                if (chainObjects.Contains(chainObj)) {
                    chainObjects.Remove(chainObj);
                }
            }
        }

        #region IEqualityComparer implements
        bool IEqualityComparer<BuyoDelete>.Equals(BuyoDelete x, BuyoDelete y) {
            if (x == null) { return false; }
            return x.GetInstanceID().Equals(y?.GetInstanceID());
        }

        int IEqualityComparer<BuyoDelete>.GetHashCode(BuyoDelete obj) {
            return obj.GetInstanceID().GetHashCode();
        }
        #endregion
    }
}