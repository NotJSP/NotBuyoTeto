using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class BuyoView : MonoBehaviour {
        [SerializeField]
        private SpriteRenderer glowEffect;
        [SerializeField]
        private ParticleSystem deleteEffect;

        public bool IsGlow => glowEffect.enabled;

        protected virtual void Reset() {
            deleteEffect = GetComponentInChildren<ParticleSystem>();
        }

        protected virtual void Awake() {
            HideGlow();
        }

        public virtual void Glow() {
            glowEffect.enabled = true;
        }

        public virtual void HideGlow() {
            glowEffect.enabled = false;
        }

        public virtual void Destroy() {
            deleteEffect.Play();
        }
    }
}
