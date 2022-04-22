using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public static class BuildAssetBundles
{
    [MenuItem("Asset Bundles/Build All")]
    public static void BuildAllAssetBundles()
    {
        BuildIOSBundles();
        BuildAndroidBundles();
        BuildWinBundles();
        BuildOSXBundles();
    }
    
    [MenuItem("Asset Bundles/Build iOS")]
    public static void BuildIOSBundles()
    {
        string dir = Application.dataPath + "/../_assetBundles/" + BuildTarget.iOS.ToString();

        Directory.CreateDirectory(dir);

        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.iOS);
    }
    
    [MenuItem("Asset Bundles/Build Win")]

    public static void BuildWinBundles()
    {
        string dir = Application.dataPath + "/../_assetBundles/" + BuildTarget.StandaloneWindows.ToString();

        Directory.CreateDirectory(dir);

        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
    
    [MenuItem("Asset Bundles/Build OSX")]

    public static void BuildOSXBundles()
    {
        string dir = Application.dataPath + "/../_assetBundles/" + BuildTarget.StandaloneOSX.ToString();

        Directory.CreateDirectory(dir);

        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
    }
    
    [MenuItem("Asset Bundles/Build Android")]

    public static void BuildAndroidBundles()
    {
        string dir = Application.dataPath + "/../_assetBundles/" + BuildTarget.Android.ToString();

        Directory.CreateDirectory(dir);

        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.Android);
    }
}