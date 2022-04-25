using Cysharp.Threading.Tasks;
using LWFramework.Core;
using LWFramework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAssetStartup : MonoBehaviour
{
    // Start is called before the first frame update
    void  Start()
    {
        MainManager.Instance.Init();
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(IUIManager).ToString(), new UIManager());
        MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), new ABAssetsManger());
        ManagerUtility.AssetsMgr.OnInitUpdateComplete = OnInitUpdateComplete;
        
    }
   
    private void OnInitUpdateComplete(bool obj)
    {
        MainManager.Instance.GetManager<IUIManager>().OpenView<LoadCtrlView>();
    }
    private void Update()
    {
        MainManager.Instance.Update();
    }
}
