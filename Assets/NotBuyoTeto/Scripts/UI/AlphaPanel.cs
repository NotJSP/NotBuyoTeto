using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.UI {
    [RequireComponent(typeof(Image))]
    public class AlphaPanel : MonoBehaviour {
        private void Awake() {
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        }
    }
}
