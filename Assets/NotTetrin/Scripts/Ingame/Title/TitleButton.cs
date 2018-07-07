﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace NotTetrin.Ingame.Title {
    [RequireComponent(typeof(Button))]
    public class TitleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        private Vector3 defaultScale;

        private void Awake() {
            this.defaultScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            transform.localScale *= 1.05f;
        }

        public void OnPointerExit(PointerEventData eventData) {
            transform.localScale = defaultScale;
        }
    }
}