using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

namespace vanko.DataModel
{
    public static class UserDataModel
    {
        private static string FilePath = Application.persistentDataPath + "/User.json";

        //创建一个新的课表数据JSON文件
        public static void CreateNew()
        {
            if (!File.Exists(FilePath))
            {
                JsonData d = new JsonData();
                d["Gold"] = 100;
                d["Diamond"] = 90;
                d["Power"] = 100;

                //初始化账号我持有的道具
                JsonData row1 = new JsonData();
                row1["ItemID"] = 1;
                row1["Count"] = 1;

                d["Items"] = new JsonData();
                d["Items"].Add(row1);

                JsonData row2 = new JsonData();
                row2["ItemID"] = 3;
                row2["Count"] = 2;

                d["Items"].Add(row2);

                JsonData row3 = new JsonData();
                row3["ItemID"] = 5;
                row3["Count"] = 1;

                d["Items"].Add(row3);

                File.WriteAllText(FilePath, d.ToJson());
            }
        }

        //读取用户数据
        public static JsonData ReadAllData()
        {
            //将已有的数据读取出来，并显示在UI上
            string json = File.ReadAllText(FilePath);
            return JsonMapper.ToObject(json);
        }

        public static void WriteAllData(JsonData data)
        {
            File.WriteAllText(FilePath, data.ToJson());
        }

        //获得完整的我的道具数据（动态数据和静态数据）
        public static JsonData ReadCompleteMyItemsData()
        {
            //动态数据
            string json = File.ReadAllText(FilePath);
            JsonData dynamic = JsonMapper.ToObject(json);

            //静态数据
            TextAsset asset = Resources.Load<TextAsset>("Json/item");
            JsonData config = JsonMapper.ToObject(asset.text);

            //最终返回的数据
            JsonData d = new JsonData();

            //合并返回（打开两个JSON文件对着写，观察结构）
            for (int i = 0; i < dynamic["Items"].Count; i++)
            {
                int itemid = (int)dynamic["Items"][i]["ItemID"];

                for (int j = 0; j < config.Count; j++)
                {
                    //找到了动态表中和静态表中对应的数据
                    if (itemid == (int)config[j]["ItemID"])
                    {
                        //最终需要的就是静态表加动态表中的数量
                        //拼接放入最终的完整数据中
                        JsonData row = config[j];
                        row["Count"] = dynamic["Items"][i]["Count"];

                        d.Add(row);
                    }
                }
            }

            return d;
        }
    }
}