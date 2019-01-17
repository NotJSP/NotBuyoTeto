using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotBuyoTeto.Constants;

namespace NotBuyoTeto.Ingame.Title {
    public class TitleManager : MonoBehaviour {
        [SerializeField]
        private InputField nameField;
        [SerializeField]
        private CanvasGroup buttonGroup;

        private void Awake() {
            // アプリケーションのFPSを60に固定
            Application.targetFrameRate = 60;

            if (PlayerPrefs.HasKey(PlayerPrefsKey.PlayerName)) {
                nameField.text = PlayerPrefs.GetString(PlayerPrefsKey.PlayerName);
            }
            nameField.onValueChanged.AddListener(onNameFieldTextChanged);
        }

        private void Update() {
            if (Input.GetButton(@"Escape")) {
                Application.Quit();
            }
            // デバッグ用 (Ctrl+F12)
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F12)) {
                PlayerPrefs.DeleteAll();
                Debug.Log(@"ローカルデータを削除しました。");
            }
        }
        
        private void onNameFieldTextChanged(string text) {
            if (string.IsNullOrWhiteSpace(text)) {
                buttonGroup.interactable = false;
                return;
            }

            PlayerPrefs.SetString(PlayerPrefsKey.PlayerName, text);
            buttonGroup.interactable = true;
        }
    }
}