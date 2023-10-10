using UnityEngine;
using System.Collections;

namespace NotBuyoTeto.Ingame {
    public class GameTimer : MonoBehaviour {
        public float ElapsedTime { get; private set; }
        public bool Watching { get; private set; }

        public void Start() {
            this.ElapsedTime = 0.0f;
            this.Watching = false;
        }

        public void Start(bool restart) {
            if (restart) {
                this.ElapsedTime = 0.0f;
            }
            this.Watching = true;
        }
        public void Restart() => this.Start(true);
        public void Resume() => this.Start(false);

        public void Stop() {
            this.Watching = false;
        }

        public void Update() {
            if (!this.Watching) { return; }
            this.ElapsedTime += Time.deltaTime;
        }
    }
}