using LWFramework.Core;
using LWFramework.Message;
using LWFramework.Step;
using LWFramework.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStepGraphStartup : MonoBehaviour
{

   // public StepGraph m_StepGraph;
    public TextAsset xmlAsset;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(  LWUtility.GlobalConfig);
        MainManager.Instance.Init();
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(IUIManager).ToString(), new UIManager());
        MainManager.Instance.AddManager(typeof(IMessageManager).ToString(), new GlobalMessageManager());
        MainManager.Instance.AddManager(typeof(IHighlightingManager).ToString(), new HighlightingPlusManager());
        MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), new ResAssetsManger());
        MainManager.Instance.AddManager(typeof(IStepManager).ToString(), new StepAssetManager());

        ManagerUtility.StepMgr.SetData(xmlAsset);

       
        ManagerUtility.StepMgr.StartStep();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) {
            ManagerUtility.StepMgr.MoveNext();
        
           // ManagerUtility.MessageMgr.Dispatcher("Jump0");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            ManagerUtility.StepMgr.MovePrev();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ManagerUtility.StepMgr.JumpStep(index);
        }
    }
}


