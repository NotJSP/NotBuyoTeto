using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.UI {
    [RequireComponent(typeof(Animator))]
    public class MessageWindow : MonoBehaviour {
        [SerializeField] Text messageText;
        [SerializeField] Text statusText;

        public string Message {
            get {
                return messageText.text;
            }
            set {
                messageText.text = value;
            }
        }

        public string Status {
            get {
                return statusText.text;
            }
            set {
                statusText.text = value;
            }
        }

        private Animator animator;

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        public void Show() {
            enableObject();
            animator.Play(@"OpenWindow", 0);
        }

        public void Hide() {
            animator.Play(@"CloseWindow", 0);
            var state = animator.GetCurrentAnimatorStateInfo(0);
            Invoke(@"disableObject", state.length);
        }

        private void enableObject() {
            gameObject.SetActive(true);
        }

        private void disableObject() {
            gameObject.SetActive(false);
        }
    }
}
