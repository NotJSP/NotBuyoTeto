using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NotBuyoTeto.Utility;
using NotBuyoTeto.Utility.Tiling;

namespace NotBuyoTeto.Ingame.Tetrin {
    [RequireComponent(typeof(Renderer), typeof(TileCreator))]
    public class ColliderGroup : MonoBehaviour {
        [SerializeField]
        private GameObject indicatorPrefab;

        public IEnumerable<ColliderBlock> Children { get; private set; }

        private new Renderer renderer;
        private List<GameObject> minos = new List<GameObject>();
        private Instantiator instantiator;
        private DensityIndicator indicator;

        public int EnteredObjectCount => minos.Count();
        private ParticleSystem MinoDeleteEffect;

        protected virtual void Awake() {
            renderer = GetComponent<Renderer>();
            MinoDeleteEffect = GetComponentInChildren<ParticleSystem>();
        }

        public void Create() {
            var tileCreator = GetComponent<TileCreator>();
            var objects = tileCreator.Create();
            Children = objects.Select(o => o.GetComponent<ColliderBlock>());
        }

        public virtual void Initialize(Instantiator instantiator, GameObject wall) {
            this.instantiator = instantiator;
            this.indicator = instantiator.Instantiate(indicatorPrefab, transform.position, Quaternion.identity).GetComponent<DensityIndicator>();
            var rate = gameObject.size().y / indicator.gameObject.size().y;
            var scale = indicator.gameObject.transform.localScale;
            scale.y *= rate;
            indicator.gameObject.transform.localScale = scale;
            indicator.Initialize(wall);

            var objects = GetComponent<TileCreator>().Create();
            Children = objects.Select(o => o.GetComponent<ColliderBlock>());
        }

        protected virtual void Update() {
            var density = (float)EnterCount / Children.Count();
            indicator.UpdateDensity(density);
            renderer.enabled = EnteredAll;
        }

        public virtual void DeleteLine() {
            Debug.Log("ミノ削除");
            MinoDeleteEffect.Play();
            foreach (var mino in minos) {
                Debug.Log(mino + "削除");
                mino.transform.Translate(10000, 10000, 10000);  // OnTriggerExit2Dを呼び出すため範囲外へ移動
                instantiator.Destroy(mino, 1.0f);    // 即消しだと判定が残るから時間差攻撃
            }
            minos.Clear();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision) {
            minos.Add(collision.gameObject);
        }

        protected virtual void OnTriggerExit2D(Collider2D collision) {
            minos.Remove(collision.gameObject);
        }
        
        public bool EnteredAll => Children.All(c => c.IsEntered);
        public int EnterCount => Children.Count(c => c.IsEntered);
    }
}