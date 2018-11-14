using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.UI {
    public class Window : MonoBehaviour {
        [SerializeField]
        private Image image;
        [SerializeField]
        private Material originalMat;
        [SerializeField]
        private Color mainColor = Color.white;
        [SerializeField]
        private Color subColor = Color.black;

        void Start() {
            var mat = Instantiate(originalMat);
            mat.SetColor("_MainColor", mainColor);
            mat.SetColor("_SubColor", subColor);
            image.material = mat;
        }
    }
}