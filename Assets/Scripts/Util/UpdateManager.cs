using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Feif.Extensions;
using Feif.UI;
using Feif.UIFramework;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Networking;
using vanko.Config;

namespace vanko.Util
{
    public class UpdateManager
    {
        public string currentDownloadFile;
        public float currentDownloadProgress;
        public static string remoteUrl = "http://localhost/downloads/";
        public event Action<float, string> OnUpdate;

        // 存放待下载文件列表
        private List<UpdateData> updateDataList = new List<UpdateData>();
        private float totalSize = 0;
        private float currentDownloadSize = 0;

        public void Init()
        {
        }

        public async Task StartDownLoad()
        {
            UnityWebRequest request = UnityWebRequest.Get(remoteUrl + "md5.json");
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("DownLoad Err: " + request.error);
                return;
            }

            var md5DataText = request.downloadHandler.text;
            // Json保存到字典
            var md5Data = JsonUtility.FromJson<MD5DataList>(md5DataText);

            foreach (var p in md5Data.md5Dict)
            {
                string file = p.file;
                string md5 = p.md5;
                string path = p.path;
                Debug.Log("文件：" + file + "，MD5：" + md5);

                // 获取本地相同路径文件，如果不存在或者md5不同则下载
                string localPath = LoadConfig.dataPath + file;
                var localMd5 = FileUtil.GetFileMD5(localPath);
                if (localMd5 == "")
                {
                    totalSize += p.size;
                    updateDataList.Add(p);
                }
                else if (localMd5 != md5)
                {
                    totalSize += p.size;
                    updateDataList.Add(p);
                }
            }

            Debug.Log("需要下载文件数量：" + updateDataList.Count);
            Debug.Log("需要下载文件总大小：" + totalSize);
            for (int i = 0; i < updateDataList.Count; i++)
            {
                var p = updateDataList[i];
                await DownLoadFile(p.path, p.file, p.size);
            }

            OnUpdate?.Invoke(1, "下载完成！");
        }

        public async Task DownLoadFile(string path, string file, float size)
        {
            if (!Directory.Exists(LoadConfig.dataPath + path))
            {
                Directory.CreateDirectory(LoadConfig.dataPath + path);
            }

            UnityWebRequest request = UnityWebRequest.Get(remoteUrl + file);

            request.SendWebRequest();
            currentDownloadFile = file;

            float lastDownloadSize = 0;
            while (!request.isDone)
            {
                currentDownloadSize = request.downloadProgress * size - lastDownloadSize + currentDownloadSize;
                lastDownloadSize = request.downloadProgress * size;
                currentDownloadProgress = currentDownloadSize / totalSize;
                OnUpdate?.Invoke(currentDownloadProgress, "正在下载：" + file);
                await Task.Delay(TimeSpan.FromSeconds(0.01));
            }

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"DownLoad {remoteUrl + file} Err: " + request.error);
                return;
            }
            else
            {
                Debug.Log("DownLoad " + remoteUrl + file + " Success!");
                SaveFileAsync(LoadConfig.dataPath + file, request.downloadHandler.data);
            }
        }
        
        public async Task SaveFileAsync(string filePath, byte[] data)
        {
            await Task.Run(() =>
            {
                File.WriteAllBytes(filePath, data);
            });
        }
    }

    [Serializable]
    public class UpdateData
    {
        public string file;
        public string md5;
        public string path;
        public float size;
    }

    public class MD5DataList
    {
        public List<UpdateData> md5Dict;
    }
}