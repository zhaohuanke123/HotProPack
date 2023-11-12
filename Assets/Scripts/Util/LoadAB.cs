using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Networking;
using vanko.Config;

namespace vanko.Util
{
    public class LoadAB : MonoBehaviour
    {
        public static string remoteUrl = "http://localhost/downloads/";

        void Start()
        {
            StartCoroutine(StartDownLoad());
        }

        IEnumerator StartDownLoad()
        {
            UnityWebRequest request =
                UnityWebRequest.Get(remoteUrl + "md5.json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("DownLoad Err: " + request.error);
                yield break;
            }

            var md5DataText = request.downloadHandler.text;
            Debug.Log("下载到文件列表：\n" + md5DataText);
            // Json保存到字典
            var md5Data = JsonUtility.FromJson<MD5DataList>(md5DataText);
            Debug.Log("下载到文件列表：\n" + md5Data.md5Dict.Count);

            foreach (var p in md5Data.md5Dict)
            {
                string file = p.file;
                string md5 = p.md5;
                string path = p.path;
                Debug.Log("文件：" + file + "，MD5：" + md5);

                // 获取本地相同路径文件，如果不存在或者md5不同则下载
                string localPath = LoadConfig.dataPath + file;
                var localMd5 = FileUtil.GetFileMD5(localPath);
                if (!File.Exists(localPath))
                {
                    Debug.Log("本地不存在文件：" + localPath);
                    yield return DownLoadFile(path, file);
                }
                else if (localMd5 != md5)
                {
                    Debug.Log("本地文件MD5不同：" + localPath);
                    Debug.Log("服务器文件MD5：" + md5);
                    Debug.Log("本地文件MD5：" + localMd5);
                    yield return DownLoadFile(path, file);
                }
                else
                {
                    Debug.Log("本地文件MD5相同：" + localPath);
                }
            }

            Debug.Log("下载完成！");
        }

        IEnumerator DownLoadFile(string path, string file)
        {
            
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(remoteUrl + path + file);
            request.SendWebRequest();
            while (!request.isDone)
            {
                Debug.Log("正在下载：" + file + "，进度：" + request.downloadProgress);
                yield return 1;
            }

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("DownLoad Err: " + request.error);
                yield break;
            }

            Debug.Log("下载完成：" + remoteUrl + path + file);
        }
    }

    [Serializable]
    public class MD5Data
    {
        public string file;
        public string md5;
        public string path;
    }

    [Serializable]
    public class MD5DataList
    {
        public List<MD5Data> md5Dict;
    }
}