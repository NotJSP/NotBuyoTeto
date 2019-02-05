using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.Title {
    [RequireComponent(typeof(Text))]
    public class VersionText : MonoBehaviour {
        private Text versionText;

        private void Awake() {
            versionText = GetComponent<Text>();
            versionText.text = $"Version: {Versions.Version}, build: {Versions.BuildNumber}";
        }
    }
}
