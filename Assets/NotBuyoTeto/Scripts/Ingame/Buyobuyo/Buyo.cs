using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using SpriteGlow;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D), typeof(SpriteGlowEffect))]
    public class Buyo : MonoBehaviour, IEqualityComparer<Buyo> {
        [SerializeField]
        private BuyoType type;
        public BuyoType Type => type;

        private HashSet<Buyo> chainObjects = new HashSet<Buyo>();
        private ParticleSystem BuyoDeleteEffect;
        private SpriteGlowEffect glowEffect;

        public event EventHandler<Vector2> DeleteBuyo;

        private void Awake() {
            type = GetComponent<Buyo>().Type;
            BuyoDeleteEffect = gameObject.GetComponentInChildren<ParticleSystem>();
            glowEffect = GetComponent<SpriteGlowEffect>();
            glowEffect.GlowBrightness = 0.0f;
            glowEffect.OutlineWidth = 0;
        }

        // Update is called once per frame
        void Update() {
            int chainCount = GetChainCount(null);
            if (chainCount >= 4) {
                DestroyChain(null);
                DeleteBuyo?.Invoke(this, transform.position);
            }
            if (chainCount < 3) {
                glowEffect.GlowBrightness = 0.0f;
                glowEffect.OutlineWidth = 0;
            }
            if (chainCount >= 3) {
                glowEffect.GlowBrightness = 1.25f;
                glowEffect.OutlineWidth = 12;
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
            Destroy(gameObject, 1.5f);
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
            if (collision.collider.CompareTag(@"Wall")) { return; }
            if (collision.collider.CompareTag(@"Floor")) { return; }
            if (collision.collider.CompareTag(@"Parent")) { return; }

            var chainObj = collision.gameObject.GetComponent<Buyo>();

            if (chainObj.type == this.type) {
                chainObjects.Add(chainObj);                
            }
        }


        private void OnCollisionExit2D(Collision2D collision) {
            if (collision.collider.CompareTag(@"Wall")) { return; }
            if (collision.collider.CompareTag(@"Floor")) { return; }
            if (collision.collider.CompareTag(@"Parent")) { return; }

            var chainObj = collision.gameObject.GetComponent<Buyo>();

            if (chainObj.type == this.type) {
                if (chainObjects.Contains(chainObj)) {
                    chainObjects.Remove(chainObj);
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