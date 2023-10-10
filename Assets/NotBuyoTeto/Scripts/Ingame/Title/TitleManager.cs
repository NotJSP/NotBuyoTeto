using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using NotBuyoTeto.Constants;
using NotBuyoTeto.Utility;
using System;
using NotBuyoTeto.Ingame.SinglePlay;

namespace NotBuyoTeto.Ingame.Title {
    public class TitleManager : MonoBehaviour {
        [SerializeField]
        private InputField nameField;
        [SerializeField]
        private CanvasGroup buttonGroup;

        private Coroutine nameSavingCoroutine;

        private void Awake() {
            // アプリケーションのFPSを60に固定
            Application.targetFrameRate = 60;

            if (!PlayerPrefs.HasKey(PlayerPrefsKey.PlayerId)) {
                initializePlayerData();
            }
            nameField.text = PlayerPrefs.GetString(PlayerPrefsKey.PlayerName);
            nameField.onValueChanged.AddListener(onNameFieldTextChanged);
            nameField.onEndEdit.AddListener(onNameFieldEndEdit);
        }

        private void Update() {
            if (Input.GetButton(@"Escape")) {
                Application.Quit();
            }
            // デバッグ用 (Ctrl+F12)
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F12)) {
                initializePlayerData();
                Debug.Log(@"ローカルデータを削除しました。");
            }
        }

        private void initializePlayerData() {
            PlayerPrefs.DeleteAll();
            nameField.text = "Player";
            this.onNameFieldEndEdit("Player");
        }

        private void onNameFieldEndEdit(string name) {
            if (this.nameSavingCoroutine != null) {
                StopCoroutine(this.nameSavingCoroutine);
            }

            try {
                this.nameSavingCoroutine = StartCoroutine(savePlayerName(name));
            } catch (Exception e) {
                Debug.LogError(e.Message);
            } finally {
                this.nameSavingCoroutine = null;
            }

            PlayerPrefs.SetString(PlayerPrefsKey.PlayerName, name);
        }

        private void onNameFieldTextChanged(string name) {
            if (string.IsNullOrWhiteSpace(name)) {
                buttonGroup.interactable = false;
                return;
            }
            buttonGroup.interactable = true;
        }

        private IEnumerator savePlayerName(string name) {
            var ncmbObj = new NCMBObject(NCMBClassName.Users);
            ncmbObj[@"name"] = name;

            if (PlayerPrefs.HasKey(PlayerPrefsKey.PlayerId)) {
                ncmbObj.ObjectId = PlayerPrefs.GetString(PlayerPrefsKey.PlayerId);
            }

            var watcher = new ASyncValue<bool, NCMBException>();

            while (true) {
                ncmbObj.SaveAsync(e => {
                    if (e != null) {
                        Debug.LogError(e);
                        watcher.Exception = e;
                        return;
                    }
                    watcher.Value = true;
                    PlayerPrefs.SetString(PlayerPrefsKey.PlayerId, ncmbObj.ObjectId);
                });
                yield return new WaitUntil(watcher.TakeOrFailure);

                if (!watcher.Failure) { break; }
                yield return new WaitForSeconds(3.0f);
            }
        }
    }
}