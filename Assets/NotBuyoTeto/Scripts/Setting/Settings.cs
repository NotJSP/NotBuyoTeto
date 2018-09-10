using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NotBuyoTeto.UI;

namespace NotBuyoTeto.Setting {
    [RequireComponent(typeof(SelectableToggler))]
    [DefaultExecutionOrder(1)]
    public class Settings : MonoBehaviour {
        [Header(@"Properties")]
        [SerializeField] RectTransform container;
        [SerializeField] AnimationCurve transitionCurve;
        [SerializeField] float transitionTime;

        [Space(1.0f)]
        [SerializeField] SettingEntry[] entries;

        private new RectTransform transform;
        private SelectableToggler toggler;
        private Coroutine coroutine;
        private bool active;

        private void Awake() {
            this.transform = GetComponent<RectTransform>();
            this.toggler = GetComponent<SelectableToggler>();

            foreach (var entry in entries) {
                entry.Create(container);
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.F1)) {
                GUI.FocusControl(@"");
                toggler.ToggleAll();
                Toggle();
            }
        }

        public void Toggle() {
            if (active) {
                Hide();
            } else {
                Show();
            }
        }

        public void Show() {
            active = true;

            var from_left_x = Camera.main.ViewportToWorldPoint(new Vector3(0, 0)).x;
            var to_left_x = transform.position.x;
            var diff = to_left_x - from_left_x;

            var from_x = Camera.main.transform.position.x;
            var to_x = from_x + diff;

            var camera_pos = Camera.main.transform.position;
            var from = new Vector3(from_x, camera_pos.y, camera_pos.z);
            var to = new Vector3(to_x, camera_pos.y, camera_pos.z);

            if (coroutine != null) {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(cameraMove(from, to));
        }

        public void Hide() {
            active = false;

            var from_x = Camera.main.transform.position.x;
            var to_x = 0.0f;

            var camera_pos = Camera.main.transform.position;
            var from = new Vector3(from_x, camera_pos.y, camera_pos.z);
            var to = new Vector3(to_x, camera_pos.y, camera_pos.z);

            if (coroutine != null) {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(cameraMove(from, to));
        }

        private IEnumerator cameraMove(Vector3 from, Vector3 to) { 
            var startTime = Time.time;

            while (true) {
                var duration = Time.time - startTime;
                var t1 = Mathf.Clamp01(duration / transitionTime);
                var t2 = transitionCurve.Evaluate(t1);
                var p = Vector3.Lerp(from, to, t2);
                Camera.main.transform.position = p;

                if (duration > transitionTime) { break; }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
