using System;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Utility;
using NotBuyoTeto.Ingame.SinglePlay.Tokoton;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class BuyoManager : MonoBehaviour {
        [SerializeField]
        private Instantiator instantiator;
        [SerializeField]
        private BuyoPerspective perspective;
        [SerializeField]
        private BuyoSpawner spawner;
        [SerializeField]
        private BuyoSfxManager sfxManager;
        [SerializeField]
        private ComboManager comboManager;
        [SerializeField]
        private LevelManager levelManager;
        [SerializeField]
        private Rigidbody2D buyoRigidbody;
        [SerializeField]
        public GameObject buyoparent;
        [SerializeField]
        private BuyoControlSettings controlSettings;
        
        private List<GameObject> buyos = new List<GameObject>();
        private GameObject parent;
        private bool controlable = true;
        private float fallSpeed;

        public event EventHandler HitBuyo;

        private NextBuyo nextBuyo => perspective.NextBuyo;

        public GameObject CurrentBuyo => buyos.Count != 0 ? buyos[buyos.Count - 1] : null;

        private void Update() {
            if (!controlable) { return; }
        }

        public void Initialize(float fallSpeed) {
            controlable = true;

            nextBuyo.Clear();

            buyos.ForEach(instantiator.Destroy);
            buyos.Clear();
            Destroy(parent);
            SetFallSpeed(fallSpeed);
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

        private void set(BuyoType[] types) {
            //親オブジェクト作成
            GameObject parent = Instantiate(buyoparent);
            parent.AddComponent<Parent>().Initialize(sfxManager, controlSettings, fallSpeed);
            this.parent = parent;

            controlable = true;

            //子オブジェクト(ぶよ)作成
            var position = perspective.Field.Ceiling.transform.position;
            var obj0 = spawner.Spawn(types[0], position, 0);
            obj0.GetComponent<Buyo>().DeleteBuyo += onDeleteBuyo;         
            obj0.AddComponent<Rigidbody2D>().CopyOf(buyoRigidbody);
            var obj1 = spawner.Spawn(types[1], position, 1);
            obj1.GetComponent<Buyo>().DeleteBuyo += onDeleteBuyo;
            obj1.AddComponent<Rigidbody2D>().CopyOf(buyoRigidbody);
            
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

        public void SetFallSpeed(float speed) {
            fallSpeed = speed;
        }

        private void onDeleteBuyo(object sender, Vector2 position) {
            comboManager.countUp(position);
            levelManager.DeleteCountUp();
            gameObject.GetComponent<BuyoDeleteScoring>().buyoDeleteScoring(comboManager.getComboCount());
        }

    }
}
