using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D), typeof(BuyoView))]
    public class Buyo : MonoBehaviour, IEqualityComparer<Buyo> {
        [SerializeField]
        private BuyoType type;
        public BuyoType Type => type;
        [SerializeField]
        private BuyoView view;

        private Instantiator instantiator;
        private BuyoSfxManager sfxManager;
        private HashSet<Buyo> chainObjects = new HashSet<Buyo>();

        public event EventHandler<Tuple<Vector2, int>> DeleteBuyo;

        public void Initialize(Instantiator instantiator , BuyoSfxManager sfxManager) {
            this.instantiator = instantiator;
            this.sfxManager = sfxManager;
        }

        protected virtual void Update() {
            int chainCount = GetChainCount(null);
            if (chainCount >= 4) {
                DestroyChain(null);
                var tuple = new Tuple<Vector2, int>(transform.position, chainCount);
                DeleteBuyo?.Invoke(this, tuple);
                sfxManager.Play(BuyoSfxType.Delete);
            }
            if (view.IsGlow && chainCount < 3) {
                view.HideGlow();
            }
            if (!view.IsGlow && chainCount >= 3) {
                view.Glow();
            }
        }

        protected void DestroyChain(HashSet<Buyo> alreadyDestroyed) {
            var destroyObjects = new HashSet<Buyo>(chainObjects);

            if (alreadyDestroyed == null) {
                alreadyDestroyed = new HashSet<Buyo>();
            }
            alreadyDestroyed.Add(this);

            destroyObjects.ExceptWith(alreadyDestroyed);
            alreadyDestroyed.UnionWith(destroyObjects);

            var destroyList = new List<Buyo>(destroyObjects);
            destroyList.ForEach(o => o.DestroyChain(alreadyDestroyed));

            view.Destroy();
            gameObject.transform.localScale = Vector3.zero;
            instantiator.Destroy(gameObject, 1.5f);
            chainObjects.Clear();
        }

        public int GetChainCount(HashSet<Buyo> alreadyCounted) {
            var findObjects = new HashSet<Buyo>(chainObjects);

            if (alreadyCounted == null) {
                alreadyCounted = new HashSet<Buyo>();
            }
            alreadyCounted.Add(this);

            findObjects.ExceptWith(alreadyCounted);

            alreadyCounted.UnionWith(findObjects);
            return findObjects.Sum(o => o.GetChainCount(alreadyCounted)) + 1;
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision) {
            if (collision.collider.CompareTag("Buyo")) {
                var chainObj = collision.gameObject.GetComponent<Buyo>();

                if (chainObj.type == this.type) {
                    chainObjects.Add(chainObj);
                }
            }
        }

        protected virtual void OnCollisionExit2D(Collision2D collision) {
            if (collision.collider.CompareTag("Buyo")) {
                var chainObj = collision.gameObject.GetComponent<Buyo>();

                if (chainObj.type == this.type) {
                    if (chainObjects.Contains(chainObj)) {
                        chainObjects.Remove(chainObj);
                    }
                }
            }
        }

        #region IEqualityComparer implements
        bool IEqualityComparer<Buyo>.Equals(Buyo x, Buyo y) {
            if (x == null) { return false; }
            return x.GetInstanceID().Equals(y?.GetInstanceID());
        }

        int IEqualityComparer<Buyo>.GetHashCode(Buyo obj) {
            return obj.GetInstanceID().GetHashCode();
        }
        #endregion
    }
}