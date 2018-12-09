using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.Ingame.MultiPlay {
    public class StartingCounter : MonoBehaviour {
        [SerializeField]
        private Text label;

        private int count;
        public int Count {
            get {
                return count;
            }
            private set {
                count = value;
                updateView();
            }
        }

        public bool IsCounting { get; private set; }
        private float time;

        public event EventHandler OnZero;

        private void Reset() {
            this.label = GetComponentInChildren<Text>();
        }

        public void Set(int count) {
            if (count < 0) {
                throw new ArgumentException("カウントが0未満に設定されました。");
            }
            this.Count = count;
        }

        public void CountStart() {
            IsCounting = true;
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        private void Update() {
            if (!IsCounting) { return; }

            time += Time.deltaTime;

            if (time > 1.0f) {
                Count--;
                time -= 1.0f;
            }

            if (Count == 0) {
                IsCounting = false;
                OnZero?.Invoke(this, EventArgs.Empty);
            }
        }

        private void updateView() {
            label.text = Count.ToString();
        }
    }
}
