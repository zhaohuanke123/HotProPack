using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Feif.UIFramework;
using vanko.Util;
using XLua;

namespace Feif.UI
{
    public class UILoadData : UIData
    {
        public string Title;
        public string progressText;
        public float progress;
    }

    [PanelLayer]
    public class UILoad : UIComponent<UILoadData>
    {
        [SerializeField] private Text updateText;
        [SerializeField] private Text progressText;
        [SerializeField] private Slider updateSlider;
        private UpdateManager manager;

        protected override Task OnCreate()
        {
            manager = new UpdateManager();
            manager.OnUpdate += (progress, progressText) =>
            {
                this.Data.Title = progressText;
                // 转化为百分比 0.00 - 100.00
                this.Data.progressText = (progress * 100).ToString("F2") + "%";
                this.Data.progress = progress;
                UIFrame.Refresh(this);
            };
            manager.OnComplete += () =>
            {
                UIFrame.Hide(this);
                UIFrame.Show<UIMain>(new UIMainData());
            };
            return Task.CompletedTask;
        }

        protected override Task OnRefresh()
        {
            updateText.text = this.Data.Title;
            progressText.text = this.Data.progressText;
            updateSlider.value = this.Data.progress;
            return Task.CompletedTask;
        }

        protected override void OnBind()
        {
        }

        protected override void OnUnbind()
        {
        }

        protected override void OnShow()
        {
            // 异步下载
            manager.StartDownLoad();
        }

        protected override void OnHide()
        {
        }

        protected override void OnDied()
        {
        }
    }
}