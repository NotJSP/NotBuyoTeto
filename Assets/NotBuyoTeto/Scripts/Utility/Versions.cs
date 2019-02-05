using System;
using System.Collections;
using UnityEngine;

namespace NotBuyoTeto.Utility {
    public static class Versions {
        public static string Version {
            get {
                var appVersion = Application.version;
                var separator_pos = appVersion.IndexOf('#');
                return Application.version.Substring(0, separator_pos);
            }
        }

        public static int BuildNumber {
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
    }
}
