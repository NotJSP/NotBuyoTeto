using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.UI {
    public class SettingGroup : MonoBehaviour {
        [SerializeField] private Text _header;
        [SerializeField] private RectTransform _container;

        public string title {
            get { return _header.text; }
            set { _header.text = value; }
        }

        public RectTransform container => _container;
    }
}