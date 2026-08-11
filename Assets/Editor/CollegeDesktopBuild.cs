using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using System.IO;
using System.Linq;

public static class CollegeDesktopBuild
{
    const string ScenePath =
        "Assets/Scenes/CollegePrototype.unity";

    const string ProductName =
        "Chasse au Tresor du College";

    const string FileName =
        "ChasseAuTresorCollege";

    const string Version =
        "1.0.0";

    static void PrepareProject()
    {
        var scene =
            EditorSceneManager.OpenScene(
                ScenePath
            );

        GameObject profile =
            GameObject.Find(
                "PlatformPerformanceProfile"
            );

        if (profile == null)
        {
            profile =
                new GameObject(
                    "PlatformPerformanceProfile"
                );

            profile.AddComponent<
                PlatformPerformanceProfile
            >();
        }

        PlayerSettings.companyName =
            "College";

        PlayerSettings.productName =
            ProductName;

        PlayerSettings.bundleVersion =
            Version;

        PlayerSettings.defaultScreenWidth =
            1280;

        PlayerSettings.defaultScreenHeight =
            720;

        PlayerSettings.fullScreenMode =
            FullScreenMode.FullScreenWindow;

        PlayerSettings.resizableWindow =
            true;

        PlayerSettings.runInBackground =
            false;

        PlayerSettings.usePlayerLog =
            true;

        // Mono évite d'exiger IL2CPP/Xcode pour
        // cette première version desktop.
        PlayerSettings.SetScriptingBackend(
            NamedBuildTarget.Standalone,
            ScriptingImplementation.Mono2x
        );

        EditorSceneManager.SaveScene(
            scene,
            ScenePath
        );

        // Garantit que la scène principale
        // est bien dans le build.
        bool found = false;

        foreach (
            EditorBuildSettingsScene s
            in EditorBuildSettings.scenes
        )
        {
            if (s.path == ScenePath)
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            var scenes =
                EditorBuildSettings.scenes
                .ToList();

            scenes.Insert(
                0,
                new EditorBuildSettingsScene(
                    ScenePath,
                    true
                )
            );

            EditorBuildSettings.scenes =
                scenes.ToArray();
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "PROJET V1.0 PREPARE"
        );
    }

    static string[] GetScenes()
    {
        string[] scenes =
            EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            scenes =
                new string[]
                {
                    ScenePath
                };
        }

        return scenes;
    }

    static bool Supported(
        BuildTarget target)
    {
        return
            BuildPipeline
            .IsBuildTargetSupported(
                BuildTargetGroup.Standalone,
                target
            );
    }

    static bool Build(
        BuildTarget target,
        string output)
    {
        Directory.CreateDirectory(
            Path.GetDirectoryName(output)
        );

        BuildPlayerOptions options =
            new BuildPlayerOptions();

        options.scenes =
            GetScenes();

        options.target =
            target;

        options.locationPathName =
            output;

        options.options =
            BuildOptions.None;

        Debug.Log(
            "BUILD START : "
            + target
        );

        BuildReport report =
            BuildPipeline.BuildPlayer(
                options
            );

        BuildSummary summary =
            report.summary;

        Debug.Log(
            "BUILD RESULT : "
            + target
            + " -> "
            + summary.result
            + " / "
            + (
                summary.totalSize
                / 1024f
                / 1024f
            ).ToString("0.0")
            + " Mo"
        );

        if (
            summary.result
            != BuildResult.Succeeded
        )
        {
            Debug.LogError(
                "BUILD ECHEC : "
                + target
            );

            return false;
        }

        Debug.Log(
            "BUILD OK : "
            + output
        );

        return true;
    }

    [MenuItem(
        "College Treasure/Build/Linux 64 bits"
    )]
    public static void BuildLinux()
    {
        PrepareProject();

        if (
            !Supported(
                BuildTarget
                    .StandaloneLinux64
            )
        )
        {
            Debug.LogError(
                "MODULE LINUX ABSENT"
            );

            return;
        }

        Build(
            BuildTarget
                .StandaloneLinux64,

            "Builds/Linux/"
            + FileName
            + ".x86_64"
        );
    }

    [MenuItem(
        "College Treasure/Build/Windows 64 bits"
    )]
    public static void BuildWindows()
    {
        PrepareProject();

        if (
            !Supported(
                BuildTarget
                    .StandaloneWindows64
            )
        )
        {
            Debug.LogWarning(
                "MODULE WINDOWS ABSENT - AJOUTER WINDOWS BUILD SUPPORT DANS UNITY HUB"
            );

            return;
        }

        Build(
            BuildTarget
                .StandaloneWindows64,

            "Builds/Windows/"
            + FileName
            + ".exe"
        );
    }

    [MenuItem(
        "College Treasure/Build/macOS Intel"
    )]
    public static void BuildMacIntel()
    {
        PrepareProject();

        if (
            !Supported(
                BuildTarget
                    .StandaloneOSX
            )
        )
        {
            Debug.LogError(
                "MODULE MACOS ABSENT OU PLATEFORME NON DISPONIBLE SUR CET OS"
            );

            return;
        }

        // 0 = architecture Intel x64
        PlayerSettings.SetArchitecture(
            NamedBuildTarget.Standalone,
            0
        );

        Build(
            BuildTarget
                .StandaloneOSX,

            "Builds/macOS/"
            + FileName
            + ".app"
        );
    }
}
