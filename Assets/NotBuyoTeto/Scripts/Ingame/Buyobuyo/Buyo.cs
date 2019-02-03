using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class Buyo : MonoBehaviour, IEqualityComparer<Buyo> {
        [SerializeField]
        private BuyoType type;
        public BuyoType Type => type;
        [SerializeField]
        private SpriteRenderer glowEffect;

        private Instantiator instantiator;
        private HashSet<Buyo> chainObjects = new HashSet<Buyo>();
        private ParticleSystem BuyoDeleteEffect;

        public event EventHandler<Tuple<Vector2, int>> DeleteBuyo;

        private void Awake() {
            type = GetComponent<Buyo>().Type;
            BuyoDeleteEffect = gameObject.GetComponentInChildren<ParticleSystem>();
            glowEffect.enabled = false;
        }

        public void Initialize(Instantiator instantiator) {
            this.instantiator = instantiator;
        }

        // Update is called once per frame
        void Update() {
            int chainCount = GetChainCount(null);
            if (chainCount >= 4) {
                DestroyChain(null);
                var tuple = new Tuple<Vector2, int>(transform.position, chainCount);
                DeleteBuyo?.Invoke(this, tuple);
            }
            if (chainCount < 3) {
                glowEffect.enabled = false;
            }
            if (chainCount >= 3) {
                glowEffect.enabled = true;
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

            BuyoDeleteEffect.Play();
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

        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.collider.CompareTag("Buyo")) {
                var chainObj = collision.gameObject.GetComponent<Buyo>();

                if (chainObj.type == this.type) {
                    chainObjects.Add(chainObj);
                }
            }
        }


        private void OnCollisionExit2D(Collision2D collision) {
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