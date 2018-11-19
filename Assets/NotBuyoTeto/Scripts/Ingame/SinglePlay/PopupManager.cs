using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.Ingame.SinglePlay{
    public class PopupManager : MonoBehaviour {
        [SerializeField]
        private GameObject tetrinPopup;
        [SerializeField]
        private Button openButton;
        [SerializeField]
        private Button closeButton;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void openPopup() {
            tetrinPopup.GetComponent<Animator>().Play(@"OpenPopup");
        }

        public void closePopup() {
            tetrinPopup.GetComponent<Animator>().Play(@"ClosePopup");
        }
    }
}