using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NotBuyoTeto.Constants;

namespace NotBuyoTeto.Setting {
    public class AccountSettings : SettingEntry {
        public override void Create(RectTransform container) {
            var group = Instantiate(templates.SettingGroup, container);
            group.title = @"アカウント設定";

            var label1 = Instantiate(templates.Label, group.container);
            label1.fontSize = 18;
            label1.text = @"お名前";
            
            var label2 = Instantiate(templates.Label, group.container);
            label2.fontSize = 11;
            label2.color = Color.red;
            label2.text = @"以下の名前でランキングに登録されます";

            var textField = Instantiate(templates.InputField, group.container);
            textField.characterLimit = 10;
            textField.text = PlayerPrefs.HasKey(PlayerPrefsKey.PlayerName) ? PlayerPrefs.GetString(PlayerPrefsKey.PlayerName) : @"";
            textField.onValueChanged.AddListener(onValueChanged);
        }

        private void onValueChanged(string value) {
            PlayerPrefs.SetString(PlayerPrefsKey.PlayerName, value);
        }
    }
}
