// VersionData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "VersionData", menuName = "Game/Version Data")]
public class VersionData : ScriptableObject
{
    public string displayVersion = "3.0.0";     // ← you edit this manually
    public int buildNumber = 0;

    public string FullVersionString => $"{displayVersion} (build {buildNumber})";
}