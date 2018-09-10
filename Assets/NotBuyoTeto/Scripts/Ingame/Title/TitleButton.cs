using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace NotBuyoTeto.Ingame.Title {
    [RequireComponent(typeof(Button))]
    public class TitleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
        private Vector3 defaultScale;

        private void Awake() {
            this.defaultScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            CommonSfxManager.Instance.Play(CommonSfxType.Select);
            transform.localScale *= 1.05f;
        }

        public void OnPointerExit(PointerEventData eventData) {
            transform.localScale = defaultScale;
        }

        public void OnPointerClick(PointerEventData eventData) {
            CommonSfxManager.Instance.Play(CommonSfxType.Decide);
        }
    }
}