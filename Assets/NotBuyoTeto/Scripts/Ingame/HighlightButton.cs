using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NotBuyoTeto.Ingame {
    [RequireComponent(typeof(Button))]
    public class HighlightButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField]
        private Button button;

        [SerializeField]
        private Color color1 = Color.white;
        [SerializeField]
        private Color color2 = Color.white;
        [SerializeField]
        private float fadeDuration = 1.0f;

        private float hoverTime = 0.0f;
        private bool isHover = false;

        private bool isButtonHighlighted => button.Equals(EventSystem.current.currentSelectedGameObject);

        public void Reset() {
            this.button = GetComponent<Button>();
        }

        public void Awake() {
            this.button = GetComponent<Button>();
        }

        public void Update() {
            if (isHover) {
                var t = Mathf.PingPong(hoverTime, fadeDuration);
                button.image.color = Color.Lerp(color1, color2, t);
                hoverTime += Time.fixedDeltaTime;
            } else {
                button.image.color = color1;
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            hoverTime = 0.0f;
            isHover = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            isHover = false;
        }
    }
}
