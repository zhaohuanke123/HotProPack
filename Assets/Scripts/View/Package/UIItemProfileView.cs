using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using UnityEngine.U2D;

namespace vanko.View.Package
{
    public class UIItemProfileView : MonoBehaviour
    {
        Image Icon;
        Image Frame;
        Text Name;
        Text Count;
        Text Description;
        Text GoldCount;

        public void Init()
        {
            Icon = transform.Find("Icon").GetComponent<Image>();
            Frame = transform.Find("Icon/Frame").GetComponent<Image>();
            Name = transform.Find("Name").GetComponent<Text>();
            Count = transform.Find("Count").GetComponent<Text>();
            Description = transform.Find("Description/Text").GetComponent<Text>();
            GoldCount = transform.Find("Sell/Count").GetComponent<Text>();
        }

        public void Display(JsonData data)
        {
            //图标
            string[] icon = data["Icon"].ToString().Split('#');
            SpriteAtlas icons = Resources.Load<SpriteAtlas>("UI/" + icon[0]);
            Icon.sprite = icons.GetSprite(icon[1]);

            //品质框
            //加载图集
            SpriteAtlas atlas = Resources.Load<SpriteAtlas>("UI/Package");

            switch ((int)data["Quality"])
            {
                case 1:
                    //根据图集加载精灵（提供精灵的名称）
                    Frame.sprite = atlas.GetSprite("bg_item_frame_white");
                    break;
                case 2:
                    Frame.sprite = atlas.GetSprite("bg_item_frame_green");
                    break;
                case 3:
                    Frame.sprite = atlas.GetSprite("bg_item_frame_blue");
                    break;
                case 4:
                    Frame.sprite = atlas.GetSprite("bg_item_frame_purple");
                    break;
                case 5:
                    Frame.sprite = atlas.GetSprite("bg_item_frame_orange");
                    break;
                case 6:
                    Frame.sprite = atlas.GetSprite("bg_item_frame_white");
                    break;
            }

            //道具名称
            Name.text = data["Name"].ToString();

            //持有数量
            Count.text = "拥有<color=#0748B9FF>" + data["Count"].ToString() + "</color>件";

            //描述
            Description.text = data["Description"].ToString();

            GoldCount.text = data["CellGold"].ToString();
        }
    }
}