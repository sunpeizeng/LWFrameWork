using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWFramework.FMS;
using LWFramework.UI;
using LWFramework.Core;
using System;
using UnityEngine.SceneManagement;
using libx;


[FSMTypeAttribute(nameof(FSMName.Procedure), true)]
public class TestStartProcedure : BaseFSMState
{
    public async override void OnEnter(BaseFSMState lastState)
    {
        LWDebug.Log("进入流程 状态机OnEnterOnEnterOnEnterOnEnterOnEnter222222222222222");
        MainManager.Instance.GetManager<IAssetsManager>().LoadSceneAsync("Assets/@Resources/Scenes/TestScene.unity", true, LoadSceneComplete);
        GameObject go = await MainManager.Instance.GetManager<IAssetsManager>().LoadAsync<GameObject>("Assets/@Resources/Prefabs/Cube.prefab");
        GameObject cube2 = GameObject.Instantiate(go, Vector3.zero, Quaternion.identity) as GameObject;
       
        //ResourceRequest resourceRequest = MainManager.Instance.GetManager<IAssetsManager>().LoadAsync<ResourceRequest>("Assets/@Resources/Prefabs/Cube.prefab", typeof(GameObject));
        //resourceRequest.completed += (o) =>
        //{
        //    GameObject cube = GameObject.Instantiate(resourceRequest.asset, Vector3.zero, Quaternion.identity) as GameObject;
        //};
    }
  
     void LoadSceneComplete() {
        MainManager.Instance.GetManager<IUIManager>().OpenView<TestHotfixView>();
        GameObject.Find("Canvas").AddComponent(typeof(TestHotfixMono));
        MainManager.Instance.GetManager<IAssetsManager>().LoadSceneAsync("Assets/@Resources/Scenes/HotfixPatch.unity", true);
        GameObject go = MainManager.Instance.GetManager<IAssetsManager>().Load<GameObject>("Assets/@Resources/Prefabs/CubeRigidbody.prefab");
        GameObject cube = GameObject.Instantiate(go, Vector3.zero, Quaternion.identity) as GameObject;
        PhysicsEventListener.Get(cube).onTriggerEnter = OnMonoEventAction2;
        MonoEventListener.Get(cube).onMonoEventAction = OnMonoEventAction;
    }

    private void OnMonoEventAction2(Collider obj)
    {
        LWDebug.Log( obj);
    }

    private void OnMonoEventAction(string monoFun, UnityEngine.Object obj)
    {
       // LWDebug.Log(monoFun +"  "+obj);

    }

    public override void OnInit()
    {
    }

    public override void OnLeave(BaseFSMState nextState)
    {

    }

    public override void OnTermination()
    {

    }

    public override void OnUpdate()
    {

    }
   
}
