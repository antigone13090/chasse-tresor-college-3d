using System;
using System.IO;
using System.Linq;
using CollegeTreasure.Campaign;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class CampaignV2DesktopBuild
{
    const string Version = "2.0.0";
    const string FileName = "ChasseAuTresorCollegeV2";

    [MenuItem("College Treasure/V2/Build/Linux 64 bits")]
    public static void BuildLinux()
    {
        Build(BuildTarget.StandaloneLinux64, "Builds/V2/Linux/" + FileName + ".x86_64");
    }

    [MenuItem("College Treasure/V2/Build/Windows 64 bits")]
    public static void BuildWindows()
    {
        Build(BuildTarget.StandaloneWindows64, "Builds/V2/Windows/" + FileName + ".exe");
    }

    [MenuItem("College Treasure/V2/Build/macOS Universal")]
    public static void BuildMac()
    {
        Build(BuildTarget.StandaloneOSX, "Builds/V2/macOS/" + FileName + ".app");
    }

    static void Build(BuildTarget target, string output)
    {
        string[] scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();

        if (scenes.Length != 8
            || scenes[0] != "Assets/CampaignV2/Scenes/CampaignMenu.unity"
            || CampaignCatalog.All.Length != 7)
        {
            throw new InvalidOperationException(
                "La campagne V2 doit etre generee et validee avant le build.");
        }

        if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, target))
            throw new InvalidOperationException("Module de build absent pour " + target + ".");

        Directory.CreateDirectory(Path.GetDirectoryName(output));
        string previousVersion = PlayerSettings.bundleVersion;
        string previousProductName = PlayerSettings.productName;

        try
        {
            PlayerSettings.bundleVersion = Version;
            PlayerSettings.productName = "Chasse au Tresor du College V2";
            PlayerSettings.SetScriptingBackend(
                NamedBuildTarget.Standalone,
                ScriptingImplementation.Mono2x);

            BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = scenes,
                target = target,
                locationPathName = output,
                options = BuildOptions.None
            });

            BuildSummary summary = report.summary;
            Debug.Log("BUILD V2 RESULT : " + target + " -> " + summary.result
                + " / " + (summary.totalSize / 1024f / 1024f).ToString("0.0") + " Mo");

            if (summary.result != BuildResult.Succeeded)
                throw new InvalidOperationException("Echec du build V2 pour " + target + ".");

            Debug.Log("BUILD V2 OK : " + output);
        }
        finally
        {
            PlayerSettings.bundleVersion = previousVersion;
            PlayerSettings.productName = previousProductName;
        }
    }
}
