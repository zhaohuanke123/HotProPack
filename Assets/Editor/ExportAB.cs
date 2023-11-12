using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ExportAB
{
    [MenuItem("AB包导出/Windows")]
    public static void ForWindows()
    {
        Export(BuildTarget.StandaloneWindows);
    }

    [MenuItem("AB包导出/Mac")]
    public static void ForMac()
    {
        Export(BuildTarget.StandaloneOSX);
    }

    [MenuItem("AB包导出/iOS")]
    public static void ForiOS()
    {
        Export(BuildTarget.iOS);
    }

    [MenuItem("AB包导出/Android")]
    public static void ForAndroid()
    {
        Export(BuildTarget.Android);
    }

    private static void Export(BuildTarget platform)
    {
        //项目的Assets目录的路径
        string path = Application.dataPath;
        //保留，D:/HonorZhao/Month3/Week3/Day2/Code/
        //最终路径，D:/HonorZhao/Month3/Week3/Day2/Code/ab
        path = path.Substring(0, path.Length - 7) + "/DataPath/AB/";

        //防止路径不存在
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        //导出ab包的核心代码，生成ab包文件
        //参数1：ab包文件存储路径
        //参数2：导出选项
        //参数3：平台（不同平台的ab包是不一样的）
        BuildPipeline.BuildAssetBundles(
            path,
            BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.ForceRebuildAssetBundle,
            platform
        );

        Debug.Log("导出ab包成功");
    }
}
