using LWFramework.Core;
using LWFramework.FMS;
using LWFramework.Message;
using LWFramework.Step;
using LWFramework.UI;
using System;
using UnityEngine;

public class  StartupStep: MonoBehaviour
{
    public static Action OnStart { get; set; }
    public static Action OnUpdate { get; set; }

    public TextAsset t;
    public StepAsset s;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        //LWGlobalConfig config = await ConfigDataTool.ReadDataAsync<LWGlobalConfig>("config");
        //LWUtility.GlobalConfig = config;
        MainManager.Instance.Init();       
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(IUIManager).ToString(), new WebGLUIManager());
        MainManager.Instance.AddManager(typeof(IFSMManager).ToString(), new FSMManager());
        MainManager.Instance.AddManager(typeof(IHotfixManager).ToString(), new HotfixManagerAsync());
        MainManager.Instance.AddManager(typeof(IMessageManager).ToString(), new GlobalMessageManager());
        MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), new ABAssetsManger());
        MainManager.Instance.AddManager(typeof(IStepManager).ToString(), new StepAssetManager());
        MainManager.Instance.AddManager(typeof(IHighlightingManager).ToString(), new HighlightingPlusManager());
        ManagerUtility.AssetsMgr.OnInitUpdateComplete = OnUpdateCallback;
      
        //设置第一个启动的流程
        MainManager.Instance.FirstFSMState = typeof(TestProcedure);
        MainManager.Instance.MonoBehaviour = this;


        ManagerUtility.StepMgr.SetData(s);
        ManagerUtility.StepMgr.StartStep();
    }

   
    /// <summary>
    /// 默认资源更新完成
    /// </summary>
    /// <param name="obj"></param>
    private void OnUpdateCallback(bool obj)
    {
        ManagerUtility.HotfixMgr.LoadScriptAsync((HotfixCodeRunMode)LWUtility.GlobalConfig.hotfixCodeRunMode);

       
    }
    // Update is called once per frame
    void Update()
    {
        MainManager.Instance.Update();
        if (OnUpdate != null)
        {
            OnUpdate();
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            ManagerUtility.StepMgr.MoveNext();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ManagerUtility.StepMgr.MovePrev();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ManagerUtility.StepMgr.JumpStep(1);
        }
    }


    void OnDestroy()
    {
    }

    private void OnApplicationQuit()
    {

    }

}
