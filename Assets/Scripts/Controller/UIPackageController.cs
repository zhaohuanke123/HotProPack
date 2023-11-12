using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using LitJson;
using vanko.DataModel;
using vanko.View.Package;

public class UIPackageController : MonoBehaviour
{
    UIItemProfileView Profile;

    void Start()
    {
        //左侧视图初始化
        Profile = transform.Find("Detail").GetComponent<UIItemProfileView>();
        Profile.Init();

        transform.Find("GoBack").GetComponent<Button>().onClick.AddListener(GoBackClick);

        JsonData list = UserDataModel.ReadCompleteMyItemsData();

        Transform grid = transform.Find("Items/List/Viewport/Content");

        for(int i = 0; i < list.Count; i++)
        {
            Object prefab = Resources.Load("Prefabs/UI/ItemCell");
            GameObject cell = Object.Instantiate(prefab) as GameObject;
            cell.name = prefab.name;

            cell.transform.SetParent(grid);
            cell.transform.localScale = Vector3.one;
            cell.transform.localRotation = Quaternion.identity;

            UIItemCellView view = cell.transform.GetComponent<UIItemCellView>();
            view.Init();
            view.Display(list[i]);
        }
    }

    public void GoBackClick()
    {
        Destroy(gameObject);
    }

    public void ItemCellClick(JsonData data)
    {
        Profile.Display(data);
    }
}
