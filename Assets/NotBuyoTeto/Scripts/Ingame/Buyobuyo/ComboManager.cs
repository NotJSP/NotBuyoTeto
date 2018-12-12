using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class ComboManager : MonoBehaviour {
        public float setTime;
        public GameObject[] combos;
        private float timeElapsed;
        private int comboCount = 0;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if (comboCount > 0) {
                timeElapsed += Time.deltaTime;
            }
            if (timeElapsed > setTime) {
                Reset();
            }
        }

        private void Reset() {
            timeElapsed = 0.0f;
            comboCount = 0;
        }

        public void countUp(Vector3 position) {
            comboCount++;
            timeElapsed = 0.0f;
            GameObject combo = Instantiate(combos[comboCount - 1]) as GameObject;
            combo.transform.position = position;
            Destroy(combo, 1.0f);
            Debug.Log(comboCount + "Combo");
        }
    }
    
}