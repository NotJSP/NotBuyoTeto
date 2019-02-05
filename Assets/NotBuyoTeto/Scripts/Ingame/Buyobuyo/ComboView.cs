using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class ComboView : MonoBehaviour {
        [SerializeField]
        public GameObject comboSprite;
        [SerializeField]
        public GameObject[] numberSprites = new GameObject[10];

        public void Show(Vector2 position, int value) {
            GameObject comboText = new GameObject("comboText");
            comboText.transform.position = position;
            GameObject combo = Instantiate(comboSprite) as GameObject;
            combo.transform.SetParent(comboText.transform, false);

            string count = string.Join("", value.ToString().Reverse());
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
    }
}
