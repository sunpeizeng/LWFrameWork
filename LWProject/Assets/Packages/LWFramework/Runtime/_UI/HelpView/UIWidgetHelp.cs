using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWidgetHelp : Singleton<UIWidgetHelp>
{
    /// <summary>
    /// 提示框
    /// </summary>
    /// <param name="msgContent">提示内容</param>
    /// <param name="OnBtnClick">回调Action</param>
    /// <param name="titleStr">标题</param>
    /// <param name="confirmStr">确认按钮文字</param>
    /// <param name="cancelStr">取消按钮文字</param>
    public void OpenMessageBox(string msgContent, Action<bool> OnBtnClick, string titleStr = "提示", string confirmStr = "确定", string cancelStr = "取消")
    {
        MessageBoxView messageBoxView = ManagerUtility.UIMgr.GetView<MessageBoxView>();
        if (messageBoxView == null)
        {
            GameObject msgGo = GameObject.Find("LWFramework/Canvas/Top/MessageBoxView");
            if (msgGo == null)
            {
                LWDebug.LogError($"没有MessageBoxView对象,请在LWFramework/Canvas/Top下增加MessageBoxView,资源在LWFramework/Runtime/Prefabs/WidgetUI");
                return;
            }
            msgGo.SetActive(true);
            messageBoxView = ManagerUtility.UIMgr.OpenView<MessageBoxView>(typeof(MessageBoxView).ToString(), msgGo);
        }
        else
        {
            messageBoxView.OpenView();
        }
        messageBoxView.OnBtnClick = OnBtnClick;
        messageBoxView.MsgStr = msgContent;
        messageBoxView.ConfirmStr = confirmStr;
        messageBoxView.TitleStr = titleStr;
        messageBoxView.CancelStr = cancelStr;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 打开加载进度条页面
    /// </summary>
    /// <returns></returns>
    public void OpenLoadingBarView()
    {
        if (ManagerUtility.UIMgr.GetView<LoadingBarView>() == null)
        {
            GameObject go = GameObject.Find("LWFramework/Canvas/Top/LoadingBarView");
            if (go == null)
            {
                LWDebug.LogError($"没有LoadingBarView对象,请在LWFramework/Canvas/Top下增加LoadingBarView,资源在LWFramework/Runtime/Prefabs/WidgetUI");
                
            }
            go.SetActive(true);
            ManagerUtility.UIMgr.OpenView<LoadingBarView>(typeof(LoadingBarView).ToString(), go);
        }
        else
        {
            ManagerUtility.UIMgr.GetView<LoadingBarView>().OpenView();
        }
       
    }
    /// <summary>
    /// 关闭加载进度条页面
    /// </summary>
    public void CloseLoadingBarView()
    {
        if (ManagerUtility.UIMgr.GetView<LoadingBarView>() != null)
        {
            ManagerUtility.UIMgr.GetView<LoadingBarView>().CloseView();
        }
    }
    /// <summary>
    /// 设置进度数据
    /// </summary>
    /// <param name="value"></param>
    public void SetLoadingBarValue(float value) {
        if (ManagerUtility.UIMgr.GetView<LoadingBarView>() != null)
        {
            ManagerUtility.UIMgr.GetView<LoadingBarView>().SetLoadValue(value);
        }
    }
    /// <summary>
    /// 设置更新内容
    /// </summary>
    /// <param name="msg"></param>
    public void SetLoadMsg(string msg)
    {
        if (ManagerUtility.UIMgr.GetView<LoadingBarView>() != null)
        {
            ManagerUtility.UIMgr.GetView<LoadingBarView>().SetLoadMsg(msg);
        }
    }
    /// <summary>
    /// 设置程序版本
    /// </summary>
    /// <param name="AppVer"></param>
    public void SetAppVer(int AppVer)
    {
        if (ManagerUtility.UIMgr.GetView<LoadingBarView>() != null)
        {
            ManagerUtility.UIMgr.GetView<LoadingBarView>().SetAppVer(AppVer);
        }
    }
    /// <summary>
    /// 设置资源版本
    /// </summary>
    /// <param name="AssetVer"></param>
    public void SetAssetVer(string AssetVer)
    {
        if (ManagerUtility.UIMgr.GetView<LoadingBarView>() != null)
        {
            ManagerUtility.UIMgr.GetView<LoadingBarView>().SetAssetVer(AssetVer);
        }
    }
   

    /// //////////////////////////////////////////////////////////////////////////////////////////
   // private LoadingView m_LoadingView;
    /// <summary>
    /// 打开加载小动画
    /// </summary>
    public void OpenLoadingView()
    {
       
        if (ManagerUtility.UIMgr.GetView<LoadingView>() == null)
        {
            GameObject go = GameObject.Find("LWFramework/Canvas/Top/LoadingView");
            if (go == null)
            {
                LWDebug.LogError($"没有LoadingView对象,请在LWFramework/Canvas/Top下增加LoadingView,资源在LWFramework/Runtime/Prefabs/WidgetUI");
                return;
            }
            go.SetActive(true);
            ManagerUtility.UIMgr.OpenView<LoadingView>(typeof(LoadingView).ToString(), go);
        }
        else
        {
            ManagerUtility.UIMgr.GetView<LoadingView>().OpenView();
        }
        ManagerUtility.UIMgr.GetView<LoadingView>().LoadCount++;
    }
    /// <summary>
    /// 关闭加载小动画
    /// </summary>
    public void CloseLoadingView()
    {
        if (ManagerUtility.UIMgr.GetView<LoadingView>() != null)
        {
            ManagerUtility.UIMgr.GetView<LoadingView>().LoadCount--;
        }
    }
}
