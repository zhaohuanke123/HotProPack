using System;
using UnityEngine;
using UnityEngine.UI;

namespace vanko.View.LoadMenu
{
    public class UILoadMenuView : MonoBehaviour
    {
        private Text updateText;
        private Text progressBar;
        private Slider slider;

        public void Init()
        {
            updateText = transform.Find("UpdateText").GetComponent<Text>();
            progressBar = transform.Find("ProgressBar").GetComponent<Text>();
            slider = transform.Find("Slider").GetComponent<Slider>();
        }

        public void UpdateProcess(string text, float value)
        {
            updateText.text = text;
            progressBar.text = Math.Floor(value * 100) + "%";
            slider.value = value;
        }
    }
}