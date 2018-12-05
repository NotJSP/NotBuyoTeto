using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class BuyoDelete : MonoBehaviour, IEqualityComparer<BuyoDelete> {
        private BuyoType type;
        private HashSet<BuyoDelete> chainObjects = new HashSet<BuyoDelete>();

        private void Awake() {
            type = GetComponent<Buyo>().Type;
        }

        // Update is called once per frame
        void Update() {
            if (GetChainCount(null) >= 4) {
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

            Destroy(gameObject);
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