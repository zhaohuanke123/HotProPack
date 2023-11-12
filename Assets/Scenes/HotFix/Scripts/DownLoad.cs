﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using XLua;
using UnityEngine.UI;

//-----------------------------【下载资源】-----------------------------
public class DownLoad : MonoBehaviour
{
    // private string path = @"file://E:\WorkSpaces\Unity\XLuaFix\AssetBundles\newobj.u3d";
    // private string verPath = @"http://localhost/AssetBundles/version.txt";
    private string path = @"http://localhost/AssetBundles/newobj.u3d";

    public AssetBundle assetBundle;

    // 开始下载更新
    public void StartDownLoad()
    {
        Debug.Log("开始下载更新！");
        // 启动协程
        StartCoroutine(GetAssetBundle(ExecuteHotFix));
    }

    //-----------------------------【从服务器下载热更资源】-----------------------------
    public Slider slider;
    public Text progressText; //进度显示

    IEnumerator GetAssetBundle(Action callBack)
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path);
        request.SendWebRequest();
        while (!request.isDone)
        {
            Debug.Log(request.downloadProgress);
            slider.value = request.downloadProgress; //下载进度
            progressText.text = Math.Floor(request.downloadProgress * 100) + "%";
            yield return 1;
        }

        // 下载完成
        if (request.isDone)
        {
            progressText.text = 100 + "%";
            slider.value = 1;
            // 隐藏UI(等待1s)
            yield return new WaitForSeconds(1);
            GameObject.Find("Canvas").SetActive(false);
        }

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("DownLoad Err: " + request.error);
        }
        else
        {
            assetBundle = DownloadHandlerAssetBundle.GetContent(request);
            TextAsset hot = assetBundle.LoadAsset<TextAsset>("luaScript.lua.txt");

            string newPath = Application.persistentDataPath + @"/luaScript.lua.txt";
            if (!File.Exists(newPath))
            {
                // Create后如果不主动释放资源就会被占用,下次打开会报错，所以一定要加上 .Dispose()
                File.Create(newPath).Dispose();
            }

            // 写入文件
            File.WriteAllText(newPath, hot.text);

            Debug.Log("下载资源成功！new Path : " + newPath);
            // 下载成功后 读取执行lua脚本
            callBack();
        }
    }

    //-----------------------------【执行热更脚本】-----------------------------
    public void ExecuteHotFix()
    {
        Debug.Log("开始执行热更脚本 luaScript");
        LuaEnv luaEnv = new LuaEnv();
        luaEnv.AddLoader(MyLoader);
        luaEnv.DoString("require 'luaScript'");
    }

    // 自定义Loader
    public byte[] MyLoader(ref string filePath)
    {
        // 读取下载的脚本资源
        string newPath = Application.persistentDataPath + @"/" + filePath + ".lua.txt";

        Debug.Log("执行脚本路径：" + newPath);
        string txtString = File.ReadAllText(newPath);
        return System.Text.Encoding.UTF8.GetBytes(txtString);
    }
}