using System.Collections.Generic;
using UnityEngine;

namespace vanko.Util
{
    public class ABManager
    {
        private static string ABPath = Application.streamingAssetsPath + "/AssetBundles/";

        // 存放所有的AB包
        Dictionary<string, AssetBundle> abDict = new Dictionary<string, AssetBundle>();
    }
}