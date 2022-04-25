using LWFramework.Core;
using LWFramework.FMS;
using LWFramework.Message;
using LWFramework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIStartup : MonoBehaviour
{
    public static Action OnStart { get; set; }
    public static Action OnUpdate { get; set; }

    void Start()
    {

        DontDestroyOnLoad(gameObject);
        OtherUtility.license = new LicenseKey();
        MainManager.Instance.Init();
        
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(IUIManager).ToString(), new UIManager());
        MainManager.Instance.AddManager(typeof(GlobalMessageManager).ToString(), new GlobalMessageManager());
        MainManager.Instance.AddManager(typeof(IHighlightingManager).ToString(), new HighlightingPlusManager());
        MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), new ResAssetsManger());

       
       
    } 
    // Update is called once per frame
    void Update()
    {
        MainManager.Instance.Update();
        if (OnUpdate != null)
        {
            OnUpdate();
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            MainManager.Instance.GetManager<IUIManager>().OpenView<TestView>();
        }
    }

    void OnDestroy()
    {
    }

    private void OnApplicationQuit()
    {

    }


}
