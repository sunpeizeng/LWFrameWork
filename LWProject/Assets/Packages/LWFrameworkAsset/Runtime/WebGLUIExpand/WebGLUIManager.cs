using Cysharp.Threading.Tasks;
using LWFramework.Core;
using LWFramework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///WebGL的UI管理器
/// </summary>
public class WebGLUIManager : UIManager
{
    public override void Init() {
        //UIUtility.Instance.CustomUILoad = new WebGLUILoad();
        base.Init();
    }
    public override async UniTask<T> OpenViewAsync<T>(bool isLastSibling = false)
    {
        IUIView uiViewBase;
        if (!m_UIViewDic.TryGetValue(typeof(T).ToString(), out uiViewBase))
        {
            uiViewBase = await UIUtility.Instance.CreateViewAsync<T>();
            m_UIViewDic.Add(typeof(T).ToString(), uiViewBase);
            m_UIList.Add(uiViewBase);
        }
        await UniTask.WaitUntil(() => uiViewBase != null);
        if (!uiViewBase.IsOpen)
            uiViewBase.OpenView();
        uiViewBase.SetViewLastSibling(isLastSibling);
        return (T)uiViewBase;
    }
   
   
}


