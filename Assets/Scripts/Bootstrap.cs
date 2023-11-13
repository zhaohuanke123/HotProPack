using System;
using Feif.UI;
using Feif.UIFramework;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using vanko.DataModel;
using vanko.Util;


namespace vanko
{
    public class Bootstrap : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad(this);

            // 注册资源请求释放事件
            UIFrame.OnAssetRequest += OnAssetRequest;
            UIFrame.OnAssetRelease += OnAssetRelease;

            var data = new UILoadData();
            data.Title = "This is UILoad";
            UIFrame.Show<UILoad>(data);
        }

        // 资源请求事件，type为UI脚本的类型
        // 可以使用Addressables，YooAssets等第三方资源管理系统
        private Task<GameObject> OnAssetRequest(Type type)
        {
            var layer = UIFrame.GetLayer(type);
            return Task.FromResult(Resources.Load<GameObject>($"Test/{layer.GetName()}/{type.Name}"));
        }

        // 资源释放事件
        private void OnAssetRelease(Type type)
        {
            // TODO
        }
    }
}