using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    [RequireComponent(typeof(Rigidbody2D))]
    public class BuyoController : MonoBehaviour {
        private BuyoSfxManager sfxManager;
        private Rigidbody2D buyoRigidbody;

        [SerializeField]
        private GameObject pair;

        public event EventHandler Hit;

        public BuyoController Initialize(BuyoSfxManager sfxManager, GameObject pair) {
            this.sfxManager = sfxManager;
            this.pair = pair;
            return this;
        }

        public void Awake() {
            buyoRigidbody = GetComponent<Rigidbody2D>();
        }

        public void BuyoHit(Rigidbody2D rigidbody) {
            var parent = OnHit(rigidbody);
            pair.GetComponent<BuyoController>().OnHit(rigidbody);
            if (parent != null) {
                Destroy(parent.gameObject);
            }
            sfxManager.Play(BuyoSfxType.BuyoHit);
            
            Destroy(this);
            
            Hit?.Invoke(this, EventArgs.Empty);
        }

        public Transform OnHit(Rigidbody2D rigidbody) {
            var parent = transform.parent;

            transform.SetParent(transform.parent.parent);
            gameObject.layer = LayerMask.NameToLayer("Default");
            buyoRigidbody.velocity = rigidbody.velocity;
            buyoRigidbody.isKinematic = false;
            return parent;
        }
    }
}
