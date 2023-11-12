using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using LitJson;

namespace vanko.View.Package
{
    public class UIItemCellView : MonoBehaviour
    {
        private JsonData _Data;

        Image Icon;
        Image Frame;
        Text Count;

        public void Init()
        {
            Icon = transform.Find("Icon").GetComponent<Image>();
            Frame = transform.Find("Icon/Frame").GetComponent<Image>();
            Count = transform.Find("Icon/Frame/Count").GetComponent<Text>();
            transform.GetComponent<Button>().onClick.AddListener(CellClick);
        }

        //data是一个cell的数据
        public void Display(JsonData data)
        {
            _Data = data;

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

            //持有数量
            Count.text = data["Count"].ToString();
        }

        public void CellClick()
        {
            GameObject.Find("/Canvas/Package").GetComponent<UIPackageController>().ItemCellClick(_Data);
        }
    }
}