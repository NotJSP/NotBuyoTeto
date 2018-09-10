using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using NotBuyoTeto.Ingame;

namespace NotBuyoTeto.Setting {
    public class BGMSettings : SettingEntry {
        [SerializeField]
        private BgmManager bgmManager;

        public override void Create(RectTransform container) {
            var group = Instantiate(templates.SettingGroup, container);
            group.title = @"BGM設定";

            foreach (var clip in bgmManager.Clips) {
                var key = $"bgm_{clip.name}";
                var enabled = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) == 1 : true;
                PlayerPrefs.SetInt(key, enabled ? 1 : 0);

                if (enabled) {
                    bgmManager.Add(clip);
                }

                var toggle = Instantiate(templates.Toggle, group.container);
                toggle.isOn = enabled;
                toggle.onValueChanged.AddListener(c => onValueChanged(clip, c));
                var toggleLabel = toggle.GetComponentInChildren<Text>();
                toggleLabel.text = clip.name;
            }

            var button = Instantiate(templates.Button, group.container);
            button.onClick.AddListener(() => bgmManager.RandomPlay());
            var buttonLabel = button.GetComponentInChildren<Text>();
            buttonLabel.text = @"ランダム再生";
        }

        private void onValueChanged(AudioClip clip, bool enabled) {
            var key = $"bgm_{clip.name}";

            if (enabled) {
                bgmManager.Add(clip);
                PlayerPrefs.SetInt(key, 1);
            } else {
                bgmManager.Remove(clip);
                PlayerPrefs.SetInt(key, 0);
            }
        }
    }
}
