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
using UnityEngine.PlayerLoop;
using vanko.Config;

namespace vanko.Util
{
    public class UpdateManager
    {
        public string currentDownloadFile;
        public float currentDownloadProgress;
        public static string remoteUrl = "http://localhost/downloads/";

        public event Action<float, string> OnUpdate;

        // 更新完成事件
        public event Action OnComplete;

        // 存放待下载文件列表
        private List<UpdateData> updateDataList = new List<UpdateData>();
        private float totalSize = 0;
        private float currentDownloadSize = 0;

        public async Task Init()
        {
            await Task.Run(() => { StartDownLoad(); });
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

            // 从本地读取md5文件
            OnUpdate?.Invoke(0, "正在计算需要下载的文件...");
            var localMd5Path = LoadConfig.dataPath + "/md5.json";
            if (!File.Exists(localMd5Path))
            {
                File.WriteAllText(localMd5Path, md5DataText);
                md5Data.md5Dict.ForEach(p =>
                {
                    updateDataList.Add(p);
                    totalSize += p.size;
                });
            }
            else
            {
                var localMd5DataText = File.ReadAllText(localMd5Path);
                var localMd5Data = JsonUtility.FromJson<MD5DataList>(localMd5DataText);
                // 比较md5，获取需要更新的文件列表
                for (var i = 0; i < md5Data.md5Dict.Count; i++)
                {
                    var p = md5Data.md5Dict[i];
                    var localP = localMd5Data.md5Dict.Find(x => x.file == p.file);
                    if (localP == null || localP.md5 != p.md5)
                    {
                        updateDataList.Add(p);
                        totalSize += p.size;
                    }

                    OnUpdate?.Invoke((float)i / md5Data.md5Dict.Count, "正在计算需要下载的文件...");
                }

                // 删除在localmd5列表中且不在md5列表中的文件
                for (var i = 0; i < localMd5Data.md5Dict.Count; i++)
                {
                    var p = localMd5Data.md5Dict[i];
                    var md5P = md5Data.md5Dict.Find(x => x.file == p.file);
                    if (md5P == null)
                    {
                        File.Delete(LoadConfig.dataPath + p.file);
                    }
                }
            }

            OnUpdate?.Invoke(1, "正在计算需要下载的文件...");

            Debug.Log("需要下载文件数量：" + updateDataList.Count);
            Debug.Log("需要下载文件总大小：" + totalSize);
            for (int i = 0; i < updateDataList.Count; i++)
            {
                var p = updateDataList[i];
                if (!Directory.Exists(LoadConfig.dataPath + p.path))
                {
                    Directory.CreateDirectory(LoadConfig.dataPath + p.path);
                }

                await DownLoadFile(p.file, p.size);
            }

            OnUpdate?.Invoke(1, "下载完成！");
            await Task.Delay(TimeSpan.FromSeconds(1));
            OnComplete?.Invoke();
        }

        async Task DownLoadFile(string file, float size)
        {
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

        async Task SaveFileAsync(string filePath, byte[] data)
        {
            await Task.Run(() => { File.WriteAllBytes(filePath, data); });
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