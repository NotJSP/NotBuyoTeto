using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.Ingame.Title {
    [RequireComponent(typeof(Text))]
    public class Versions : MonoBehaviour {
        private Text versionText;

        public string Version {
            get {
                var appVersion = Application.version;
                var separator_pos = appVersion.IndexOf('#');
                return Application.version.Substring(0, separator_pos);
            }
        }

        public int BuildNumber {
            get {
                var appVersion = Application.version;
                var separator_pos = appVersion.IndexOf('#');
                var buildNumberString = appVersion.Substring(separator_pos + 1);
                int buildNumber;
                if (!int.TryParse(buildNumberString, out buildNumber)) {
                    throw new SystemException(@"アプリケーションバージョンの書式が不正です。");
                }
                return buildNumber;
            }
        }

        private void Awake() {
            versionText = GetComponent<Text>();
            versionText.text = $"Version: {Version}, build: {BuildNumber}";
        }
    }
}
