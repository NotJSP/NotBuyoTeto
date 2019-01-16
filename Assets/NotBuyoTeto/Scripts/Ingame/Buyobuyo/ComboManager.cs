using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class ComboManager : MonoBehaviour {
        public float setTime;
        public GameObject comboSprite;
        public GameObject[] numberSprites;
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

        public void countUp(Vector2 position) {
            comboCount++;
            timeElapsed = 0.0f;
            Debug.Log(comboCount + "Combo");

            GameObject comboText = new GameObject("comboText");
            comboText.transform.position = position;
            GameObject combo = Instantiate(comboSprite) as GameObject;
            combo.transform.SetParent(comboText.transform, false);

            string count = string.Join("", comboCount.ToString().Reverse());
            float f = -1.0f;
            foreach (char c in count) {
                int i = int.Parse(c.ToString());
                GameObject number = Instantiate(numberSprites[i]) as GameObject;
                number.transform.SetParent(comboText.transform, false);
                Vector2 tmp = number.transform.position;
                tmp.x += f;
                f += -0.35f;
                number.transform.position = tmp;
            }
            Destroy(comboText, 1.0f);
        }

        public int getComboCount() {
            return comboCount;
        }
    }
    
}