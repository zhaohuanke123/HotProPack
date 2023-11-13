using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vanko.Util;
using vanko.View.LoadMenu;

namespace vanko
{
    public class UILoadMenuController : MonoBehaviour
    {
        UILoadMenuView View;
        UpdateManager _updateManager;

        void Start()
        {
            View = transform.Find("Update").GetComponent<UILoadMenuView>();
            View.Init();

            _updateManager = new UpdateManager();
            if (_updateManager == null)
            {
                Debug.LogError("LoadAB is null");
                return;
            }

            _updateManager.Init();
        }

        void Update()
        {
            if (_updateManager.currentDownloadFile != null)
            {
                View.UpdateProcess(_updateManager.currentDownloadFile, _updateManager.currentDownloadProgress);
            }
        }
    }
}