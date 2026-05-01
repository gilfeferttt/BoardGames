// AutoVersionIncrement.cs → put inside Editor folder
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class AutoVersionIncrement : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        // 1. Find your VersionData asset (adjust path if needed)
        var versionData = AssetDatabase.LoadAssetAtPath<VersionData>("Assets/Resources/VersionData.asset");

        if (versionData == null)
        {
            Debug.LogWarning("VersionData not found → skipping auto-increment");
            return;
        }

        // 2. Increment
        versionData.buildNumber++;
        EditorUtility.SetDirty(versionData);
        AssetDatabase.SaveAssets();

        // 3. Optional: semantic-like auto-update of display version (uncomment if wanted)
        // versionData.displayVersion = $"1.0.{versionData.buildNumber / 100}"; // example

        // 4. Apply to Unity PlayerSettings
        PlayerSettings.bundleVersion = versionData.displayVersion;

        // For Android / iOS stores
        PlayerSettings.Android.bundleVersionCode = versionData.buildNumber;
        PlayerSettings.iOS.buildNumber = versionData.buildNumber.ToString();

        Debug.Log($"Version updated → {versionData.FullVersionString}");
    }
}