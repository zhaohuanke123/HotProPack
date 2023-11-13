using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Feif.UIFramework;
using LitJson;
using vanko;
using vanko.DataModel;

namespace Feif.UI
{
    public class UIMainData : UIData
    {
        public int GoldCount;
        public int DiamondCount;
        public int PowerCount;
    }

    [PanelLayer]
    public class UIMain : UIComponent<UIMainData>
    {
        [SerializeField] private Text goldCount;
        [SerializeField] private Button btnGoldAdd;
        [SerializeField] private Button btnPackage;
        [SerializeField] private Text diamondCount;
        [SerializeField] private Button btnDiamondAdd;
        [SerializeField] private Text powerCount;
        [SerializeField] private Button btnPowerAdd;
        JsonData data;

        protected override Task OnCreate()
        {
            UserDataModel.CreateNew();
            data = UserDataModel.ReadAllData();
            Data.GoldCount = (int)data["Gold"];
            Data.DiamondCount = (int)data["Diamond"];
            Data.PowerCount = (int)data["Power"];
            goldCount.text = Data.GoldCount.ToString();
            diamondCount.text = Data.DiamondCount.ToString();
            powerCount.text = Data.PowerCount.ToString();
            btnGoldAdd.onClick.AddListener(() =>
            {
                Data.GoldCount += 10;
                goldCount.text = Data.GoldCount.ToString();
            });
            btnDiamondAdd.onClick.AddListener(() =>
            {
                Data.DiamondCount += 10;
                diamondCount.text = Data.DiamondCount.ToString();
            });
            btnPowerAdd.onClick.AddListener(() =>
            {
                Data.PowerCount += 10;
                powerCount.text = Data.PowerCount.ToString();
            });
            btnPackage.onClick.AddListener(() =>
            {
                UIFrame.Hide(this);
            });

            return Task.CompletedTask;
        }

        protected override Task OnRefresh()
        {
            return Task.CompletedTask;
        }

        protected override void OnBind()
        {
            Debug.Log("OnBind");
        }

        protected override void OnUnbind()
        {
            Debug.Log("OnUnbind");
        }

        protected override void OnShow()
        {
            Debug.Log("OnShow");
        }

        protected override void OnHide()
        {
            Debug.Log("OnHide");
        }

        protected override void OnDied()
        {
            Debug.Log("OnDied");
            data["Gold"] = Data.GoldCount;
            data["Diamond"] = Data.DiamondCount;
            data["Power"] = Data.PowerCount;
            UserDataModel.WriteAllData(data);
        }
    }
}