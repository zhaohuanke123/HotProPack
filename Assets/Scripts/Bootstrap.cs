using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vanko.DataModel;


namespace vanko
{
    public class Bootstrap : MonoBehaviour
    {

        void Start()
        {
            UserDataModel.CreateNew();
            //进入MainMenu
            Prefabs.Load("Prefabs/UI/MainMenu");
        }
    }
}
