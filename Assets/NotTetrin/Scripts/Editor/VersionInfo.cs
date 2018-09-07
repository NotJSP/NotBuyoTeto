using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class VersionInfo : IPreprocessBuildWithReport {
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report) {
        var appVersion = PlayerSettings.bundleVersion;
        var versions = appVersion.Split('#');
        var version = versions[0];
        var buildNumberString = versions[1];
        int buildNumber;
        if (int.TryParse(buildNumberString, out buildNumber)) {
            buildNumber++;
            PlayerSettings.bundleVersion = version + '#' + buildNumber.ToString();
        } else {
            throw new SystemException(@"バージョンの書式が不正です。");
        }
    }
}
