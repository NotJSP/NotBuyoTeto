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

        //親のプレハブ
        public GameObject buyoparent;

        private List<GameObject> buyos = new List<GameObject>();
        private BuyoType currentType;
        private bool controlable = true;
        private float fallSpeed = 1.5f;
        private float defaultfallSpeed = 1.5f;

        public event EventHandler HitBuyo;

        private BuyoPerspective perspective => director.Perspective;
        private BuyoField field => perspective.Field;
        private NextBuyo nextBuyo => perspective.NextBuyo;

        public GameObject CurrentBuyo => buyos.Count != 0 ? buyos[buyos.Count - 1] : null;

        private void Update() {
            if (!controlable) { return; }
        }

        public void Reset() {
            fallSpeed = defaultfallSpeed;
            controlable = true;

            nextBuyo.Clear();

            buyos.ForEach(instantiator.Destroy);
            buyos.Clear();
        }

        public void Next() {
            var type = nextBuyo.Pop();
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

        private void set(List<BuyoType> types) {
            //親オブジェクト作成
            GameObject parent = Instantiate(buyoparent);
            parent.AddComponent<Parent>().Initialize(sfxManager, fallSpeed);

            //子オブジェクト(ぶよ)作成
            controlable = true;
            currentType = types[0];
            var obj0 = spawner.Spawn(types[0], field.Ceiling, 0);
            //obj0.AddComponent<Rigidbody2D>().CopyOf(buyoRigidbody);
            currentType = types[1];
            var obj1 = spawner.Spawn(types[1], field.Ceiling, 1);
            //obj1.AddComponent<Rigidbody2D>().CopyOf(buyoRigidbody);
            
            //ペアをつくる
            var controller1 = obj0.AddComponent<BuyoController>().Initialize(sfxManager, obj1);
            controller1.Hit += onHitBuyo;
            var controller2 = obj1.AddComponent<BuyoController>().Initialize(sfxManager, obj0);
            controller2.Hit += onHitBuyo;
            
            buyos.Add(obj0);
            buyos.Add(obj1);
            //parentを親にする
            obj0.transform.parent = parent.transform;
            obj1.transform.parent = parent.transform;
        }

        private void onHitBuyo(object sender, EventArgs args) {
            HitBuyo?.Invoke(sender, args);
        }

        public void fallSpeedUp(int level) {
            fallSpeed = fallSpeed + (0.01f * level);
        }
    }
}
