#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public static class ATTPostprocessor
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(
        BuildTarget target, string path)
    {
        if (target != BuildTarget.iOS)
            return;

        string plistPath = Path.Combine(path, "Info.plist");

        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        plist.root.SetString(
            "NSUserTrackingUsageDescription",
            "Your data may be used to show personalized ads in this app.");

        plist.WriteToFile(plistPath);
    }
}
#endif