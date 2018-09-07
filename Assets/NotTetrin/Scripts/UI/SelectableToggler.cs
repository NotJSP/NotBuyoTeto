using UnityEngine;
using UnityEngine.UI;

namespace NotTetrin.UI { 
    public class SelectableToggler : MonoBehaviour {
        [SerializeField] CanvasGroup[] groups;
        [SerializeField] Selectable[] objects;

        public void ToggleAll() {
            foreach (var group in groups) {
                // 入力中のものを強制的に無効化させる
                foreach (var inputField in group.GetComponentsInChildren<InputField>()) {
                    inputField.DeactivateInputField();
                }
                group.interactable = !group.interactable;
            }

            foreach (var obj in objects) {
                if (obj is InputField) {
                    (obj as InputField).DeactivateInputField();
                }
                obj.interactable = !obj.interactable;
            }
        }
    }
}
