using Cysharp.Threading.Tasks;
using LWFramework.Audio;
using LWFramework.Core;
using LWFramework.FMS;
using LWFramework.Message;
using LWFramework.Other;
using LWFramework.UI;
using System;
using UnityEngine;

public class StartupAB : MonoBehaviour
{
    public static Action OnStart { get; set; }
    public static Action OnUpdate { get; set; }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        OtherUtility.license = new LicenseNone();
        MainManager.Instance.Init();
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(IUIManager).ToString(), new UIManager());    
        MainManager.Instance.AddManager(typeof(IFSMManager).ToString(), new FSMManager());
        MainManager.Instance.AddManager(typeof(IHotfixManager).ToString(), new HotfixManager());
        MainManager.Instance.AddManager(typeof(IMessageManager).ToString(), new GlobalMessageManager());
        MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), new ABAssetsManger());
        MainManager.Instance.AddManager(typeof(IAudioManager).ToString(), new AudioManager());

        MainManager.Instance.GetManager<IAssetsManager>().OnInitUpdateComplete = OnUpdateCallback;
        MainManager.Instance.FirstFSMState = typeof(TestStartProcedure);
        MainManager.Instance.MonoBehaviour = this;
        
    }
   
    /// <summary>
    /// 默认资源更新完成
    /// </summary>
    /// <param name="obj"></param>
    private void OnUpdateCallback(bool obj)
    {
        StartCoroutine(ManagerUtility.HotfixMgr.IE_LoadScript((HotfixCodeRunMode)LWUtility.GlobalConfig.hotfixCodeRunMode));
       // ManagerUtility.HotfixMgr.LoadScriptAsync((HotfixCodeRunMode)LWUtility.GlobalConfig.hotfixCodeRunMode));
    } 
    // Update is called once per frame
    void Update()
    {
        MainManager.Instance.Update();
        if (OnUpdate != null)
        {
            OnUpdate();
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            Test();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            AudioClip audioClip = ManagerUtility.AssetsMgr.Load<AudioClip>("Assets/@Resources/Audio/Clip.mp3");
            AudioChannel audioChannel =  ManagerUtility.AudioMgr.Play(audioClip);
            audioChannel.Loop = true;
            ManagerUtility.AudioMgr.Stop(audioChannel);
        }
    }

    async void Test() {
        UIWidgetHelp.Instance.OpenLoadingView();
        await UniTask.Delay(3000);
        UIWidgetHelp.Instance.CloseLoadingView();
    }
    void OnDestroy()
    {
    }

    private void OnApplicationQuit()
    {

    }



}
