using System;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Utility;

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
        private Rigidbody2D buyoRigidbody;
        [SerializeField]
        public GameObject buyoparent;
        [SerializeField]
        private BuyoControlSettings controlSettings;
        
        private List<GameObject> buyos = new List<GameObject>();
        private Tuple<BuyoType, BuyoType> currentTypes;
        private GameObject parent;
        private bool controlable = true;
        private float fallSpeed;

        public event EventHandler HitBuyo;
        public event EventHandler<DeleteBuyoInfo> DeleteBuyo;

        private NextBuyo nextBuyo => perspective.NextBuyo;

        public GameObject CurrentBuyo => buyos.Count != 0 ? buyos[buyos.Count - 1] : null;

        private void Update() {
            if (!controlable) { return; }
        }

        public void Restart(float fallSpeed) {
            controlable = true;

            nextBuyo.Clear();

            buyos.ForEach(instantiator.Destroy);
            buyos.Clear();
            Destroy(parent);

            SetFallSpeed(fallSpeed);

            perspective.Field.Ceiling.Clear();
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

        public void DestroyCurrentObject() {
            controlable = false;

            if (CurrentBuyo != null) {
                instantiator.Destroy(CurrentBuyo);
                buyos.RemoveAt(buyos.Count - 1);
                Destroy(parent);
            }
        }

        private void set(Tuple<BuyoType, BuyoType> types) {
            currentTypes = types;

            // 位置取得
            var position = perspective.Field.Ceiling.transform.position;
            position.y += 0.75f;

            // 親オブジェクト作成
            parent = Instantiate(buyoparent, position, Quaternion.identity);
            parent.AddComponent<Parent>().Initialize(sfxManager, controlSettings, fallSpeed);

            // 子オブジェクト(ぶよ)作成
            var obj0 = spawner.Spawn(types.Item1, position, 0);
            var obj1 = spawner.Spawn(types.Item2, position, 1);

            // オブジェクトリストに追加
            Add(obj0);
            Add(obj1);

            // parentを親にする
            obj0.transform.parent = parent.transform;
            obj1.transform.parent = parent.transform;

            // ペアをつくる
            var controller1 = obj0.AddComponent<BuyoController>().Initialize(sfxManager, obj1);
            controller1.Hit += onHitBuyo;
            var controller2 = obj1.AddComponent<BuyoController>().Initialize(sfxManager, obj0);
            controller2.Hit += onHitBuyo;
            
            controlable = true;
        }

        public void Add(GameObject obj) {
            obj.GetComponent<Buyo>().DeleteBuyo += onDeleteBuyo;
            obj.AddComponent<Rigidbody2D>().CopyOf(buyoRigidbody);
            buyos.Add(obj);
        }

        private void onHitBuyo(object sender, EventArgs args) {
            HitBuyo?.Invoke(sender, args);
        }

        public void SetFallSpeed(float speed) {
            fallSpeed = speed;
        }

        // TODO: 適当
        private void onDeleteBuyo(object sender, Tuple<Vector2, int> positionAndObjectCount) {
            var position = positionAndObjectCount.Item1;
            var objectCount = positionAndObjectCount.Item2;

            comboManager.CountUp();
            comboManager.Show(position);

            var info = new DeleteBuyoInfo(objectCount, comboManager.Value);
            Debug.Log($"objects: {info.ObjectCount}, combo: {info.ComboCount}");
            DeleteBuyo?.Invoke(this, info);
        }
    }
}
