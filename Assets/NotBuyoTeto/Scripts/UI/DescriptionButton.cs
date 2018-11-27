using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace NotBuyoTeto.Ingame.UI {
    public class DescriptionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField]
        private Text label;
        [SerializeField, TextArea]
        private string description;

        public void OnPointerEnter(PointerEventData data) {
            label.text = description;
        }

        public void OnPointerExit(PointerEventData data) {
        }
    }
}
