using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using vanko.DataModel;
using vanko.View.MainMenu;

namespace vanko.Controller
{
    public class UIMainMenuController : MonoBehaviour
    {
        UIUserNumericalView View;

        void Start()
        {
            View = transform.Find("HeaderCount").GetComponent<UIUserNumericalView>();
            View.Init();

            transform.Find("Package").GetComponent<Button>().onClick.AddListener(PackageClick);

            View.Refresh(UserDataModel.ReadAllData());
        }

        public void PackageClick()
        {
            //进入背包页面
            Prefabs.Load("Prefabs/UI/Package");
        }
    }
}