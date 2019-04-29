#if UNITY_EDITOR

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class Exporter
{
    [MenuItem("Tools/Export Unitypackage")]
    public static void Export()
    {
        // configure
        var root = "Scripts/RandomFixtureKit";
        var exportPath = "./RandomFixtureKit.unitypackage";

        var path = Path.Combine(Application.dataPath, root);
        var assets = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)
            .Where(x => Path.GetExtension(x) == ".cs" || Path.GetExtension(x) == ".asmdef")
            .Select(x => "Assets" + x.Replace(Application.dataPath, "").Replace(@"\", "/"))
            .ToArray();

        UnityEngine.Debug.Log("Export below files" + Environment.NewLine + string.Join(Environment.NewLine, assets));

        AssetDatabase.ExportPackage(
            assets,
            exportPath,
            ExportPackageOptions.Default);

        UnityEngine.Debug.Log("Export complete: " + Path.GetFullPath(exportPath));
    }

    [MenuItem("Tools/Build CliTest Windows")]
    public static void BuildCliTestWindows()
    {
        BuildCliTest("win");
    }

    [MenuItem("Tools/Build CliTest Linux")]
    public static void BuildCliTestLinux()
    {
        BuildCliTest("linux");
    }

    [MenuItem("Tools/Build CliTest Mac")]
    public static void BuildCliTestMac()
    {
        BuildCliTest("mac");
    }

    public static void BuildCliTest(string buildTargetString)
    {
        var buildTarget = BuildTarget.StandaloneWindows64;
        if (buildTargetString.StartsWith("win", StringComparison.OrdinalIgnoreCase))
        {
            buildTarget = BuildTarget.StandaloneWindows64;
        }
        else if (buildTargetString.StartsWith("mac", StringComparison.OrdinalIgnoreCase) || buildTargetString.StartsWith("osx", StringComparison.OrdinalIgnoreCase))
        {
            buildTarget = BuildTarget.StandaloneOSX;
        }
        else if (buildTargetString.StartsWith("linux", StringComparison.OrdinalIgnoreCase))
        {
            buildTarget = BuildTarget.StandaloneLinux64;
        }
        else
        {
            throw new Exception("Unknown BuildTarget, please use [win] or [mac] or [linux]");
        }

        // always use IL2CPP.
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP);

        UnityEngine.Debug.Log("Start to Build " + buildTargetString + " binary");
        BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            targetGroup = BuildTargetGroup.Standalone,
            target = buildTarget,
            options = BuildOptions.BuildScriptsOnly
                    | BuildOptions.EnableHeadlessMode
                    | BuildOptions.IncludeTestAssemblies,
            scenes = new[] { "Assets/Scripts/RuntimeUnitTestToolkit/UnitTest.unity" },
            locationPathName = "bin/tests" + ((buildTarget == BuildTarget.StandaloneWindows64) ? ".exe" : "")
        });
        UnityEngine.Debug.Log("Build Completed, files under bin dir.");
    }
}

#endif