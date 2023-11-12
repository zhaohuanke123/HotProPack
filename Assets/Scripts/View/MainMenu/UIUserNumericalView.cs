using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

namespace vanko.View.MainMenu
{
    public class UIUserNumericalView : MonoBehaviour
    {
        private Text GoldCount;

        public void Init()
        {
            //找到Text子物体
            GoldCount = transform.Find("Gold/Count").GetComponent<Text>();
        }

        public void Refresh(JsonData data)
        {
            GoldCount.text = data["Gold"].ToString();
        }
    }
}