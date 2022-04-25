using Cysharp.Threading.Tasks;
using LWFramework.Audio;
using LWFramework.Core;
using LWFramework.FMS;
using LWFramework.Message;
using LWFramework.UI;
using LWFramework.WebRequest;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class  SpeechStartup: MonoBehaviour
{
    public static Action OnStart { get; set; }
    public static Action OnUpdate { get; set; }

    async void Start()
    {
        DontDestroyOnLoad(gameObject);
        OtherUtility.license = new LWFramework.Other.LicenseNone();
        LWGlobalConfig config = await ConfigDataTool.ReadDataAsync<LWGlobalConfig>("config.cfg", null, false);
        LWUtility.GlobalConfig = config;
        MainManager.Instance.Init();       
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(IUIManager).ToString(), new WebGLUIManager());
        MainManager.Instance.AddManager(typeof(IFSMManager).ToString(), new FSMManager());
        MainManager.Instance.AddManager(typeof(IHotfixManager).ToString(), new HotfixManagerAsync());
        MainManager.Instance.AddManager(typeof(IMessageManager).ToString(), new GlobalMessageManager());
        MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), new ABAssetsManger());
        MainManager.Instance.AddManager(typeof(IAudioManager).ToString(), new AudioManager());
        MainManager.Instance.AddManager(typeof(ISpeechManager).ToString(), new SpeechCacheManager());
        ManagerUtility.AssetsMgr.OnInitUpdateComplete = OnUpdateCallback;
      
        //设置第一个启动的流程
        //MainManager.Instance.FirstFSMState = typeof(StartProcedure);
        MainManager.Instance.MonoBehaviour = this;            
    }

  
    /// <summary>
    /// 默认资源更新完成
    /// </summary>
    /// <param name="obj"></param>
    private void OnUpdateCallback(bool obj)
    {
        //  ManagerUtility.HotfixMgr.LoadScriptAsync((HotfixCodeRunMode)LWUtility.GlobalConfig.hotfixCodeRunMode);      
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
            testc();
        }
    }

    private async void testc()
    {
        string ret = await ManagerUtility.SpeechManager.Recognize();
        LWDebug.Log(ret);
    }

    void OnDestroy()
    {
    }

    private void OnApplicationQuit()
    {

    }

}
