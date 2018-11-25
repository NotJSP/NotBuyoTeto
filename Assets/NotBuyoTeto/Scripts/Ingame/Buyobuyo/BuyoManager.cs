using System;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class BuyoManager : MonoBehaviour {
        [SerializeField]
        private Instantiator instantiator;
        [SerializeField]
        private BuyoDirector director;
        [SerializeField]
        private BuyoSpawner spawner;
        [SerializeField]
        private BuyoSfxManager sfxManager;
        [SerializeField]
        private Rigidbody2D buyoRigidbody;

        private List<GameObject> buyos = new List<GameObject>();
        private BuyoType currentType;
        private bool controlable = true;
        private float fallSpeed = 1.5f;
        private float defaultfallSpeed = 1.5f;

        public event EventHandler HitBuyo;

        private BuyoPerspective perspective => director.Perspective;
        private BuyoField field => perspective.Field;
        private NextBuyo nextBuyo => perspective.NextBuyo;
        private HoldBuyo holdBuyo => perspective.HoldBuyo;

        public GameObject CurrentBuyo => buyos.Count != 0 ? buyos[buyos.Count - 1] : null;

        private void Update() {
            if (!controlable) { return; }

            if (Input.GetButtonDown(@"Hold")) {
                hold();
            }
        }

        public void Reset() {
            fallSpeed = defaultfallSpeed;
            controlable = true;

            nextBuyo.Clear();
            holdBuyo.Clear();

            buyos.ForEach(instantiator.Destroy);
            buyos.Clear();
        }

        public void Next() {
            var type = nextBuyo.Pop();
            set(type);
            holdBuyo.Free();
        }

        private void hold() {
            var holdIndex = holdBuyo.Type;
            if (!holdBuyo.Push(currentType)) { return; }

            Destroy();

            var type = holdIndex.HasValue ? holdIndex.Value : nextBuyo.Pop();
            set(type);
        }

        public void Release() {
            controlable = false;
            var controller = CurrentBuyo.GetComponent<BuyoController>();
            Destroy(controller);
        }

        public void Destroy() {
            controlable = false;
            instantiator.Destroy(CurrentBuyo);
            buyos.RemoveAt(buyos.Count - 1);
        }

        private void set(BuyoType type) {
            currentType = type;
            controlable = true;

            var obj = spawner.Spawn(type, field.Ceiling);
            obj.AddComponent<Rigidbody2D>().CopyOf(buyoRigidbody);
            var controller = obj.AddComponent<BuyoController>().Initialize(sfxManager, fallSpeed);
            controller.Hit += onHitBuyo;
            buyos.Add(obj);

            Debug.Log(fallSpeed);
        }

        private void onHitBuyo(object sender, EventArgs args) {
            HitBuyo?.Invoke(sender, args);
        }

        public void fallSpeedUp(int level) {
            fallSpeed = fallSpeed + (0.01f * level);
        }
    }
}
