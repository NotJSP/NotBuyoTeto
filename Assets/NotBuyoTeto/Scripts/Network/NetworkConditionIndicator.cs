using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.Network {
    [RequireComponent(typeof(Image))]
    public class NetworkConditionIndicator : MonoBehaviour {
        [SerializeField]
        private Sprite highSprite;
        [SerializeField]
        private Sprite middleSprite;
        [SerializeField]
        private Sprite lowSprite;

        private Image targetImage => GetComponent<Image>();

        public void Set(NetworkConditionType condition) {
            switch (condition) {
                case NetworkConditionType.High:
                    targetImage.sprite = highSprite;
                    break;
                case NetworkConditionType.Middle:
                    targetImage.sprite = middleSprite;
                    break;
                case NetworkConditionType.Low:
                    targetImage.sprite = lowSprite;
                    break;
            }
        }
    }
}
