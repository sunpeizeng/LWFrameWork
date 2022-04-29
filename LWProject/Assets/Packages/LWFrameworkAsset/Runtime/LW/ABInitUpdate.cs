using libx;
using LWFramework.Core;
using LWFramework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// AB初始化更新
/// </summary>
public class ABInitUpdate 
{
    private Action<bool> _onInitUpdateComplete;
    /// <summary>
    /// 初始化更新完成
    /// </summary>
    public Action<bool> OnInitUpdateComplete { get => _onInitUpdateComplete; set => _onInitUpdateComplete = value; }
    /// <summary>
    /// 是否自动更新
    /// </summary>
    private bool m_AutoCheckExists { get; set; } = true;
    /// <summary>
    /// 是否自动更新
    /// </summary>
    private bool m_AutoUpdate { get; set; } = true;
    /// <summary>
    /// 是否自动更新
    /// </summary>
    private bool m_NotNetEnter { get; set; } = true;

    /// <summary>
    /// 设置参数
    /// </summary>
    public void SetConfig() {
        LWGlobalConfig globalConfig = LWUtility.GlobalConfig;
        Assets.development = ((AssetMode)globalConfig.assetMode ==  AssetMode.AssetBundleDev);
        Assets.loggable = globalConfig.loggable;
        Assets.updateAll = globalConfig.updateAll;
        Assets.downloadURL = globalConfig.downloadURL;
        Assets.verifyBy = (VerifyBy)globalConfig.verifyBy;
        Assets.searchPaths = globalConfig.searchPaths;
        Assets.patches4Init = globalConfig.updatePatches4Init;
        m_AutoUpdate = globalConfig.autoCheckUpdate;
        m_NotNetEnter = globalConfig.notNetEnter;
        m_AutoCheckExists = globalConfig.autoCheckExists;
    }
    
    /// <summary>
    /// AB加载初始化
    /// </summary>
    public virtual void AssetsInitialize() {

        Assets.Initialize(error =>
        {
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogWarning(error);
                return;
            }
            else
            {
                Debug.Log("Assets初始化成功");
                // m_LoadAssetBarView = WidgetUIHelp.Instance.OpenLoadingBarView();
                // m_LoadAssetBarView.CloseView();
               
                if ((AssetMode)LWUtility.GlobalConfig.assetMode== AssetMode.AssetBundleLocal || Assets.downloadURL == "" || Assets.downloadURL==null) {

                    OnInitUpdateComplete?.Invoke(true);
                    return;
                }
                //当下载地址为空的话，取本地数据
                if (m_AutoCheckExists)
                {
                    Assets.CheckBundlesExists();
                }
                if (m_AutoUpdate)
                {
                    StartUpdate();
                }
                else {
                    UIWidgetHelp.Instance.OpenMessageBox("是否更新资源?", (flag) =>
                    {
                        if (flag)
                        {
                            StartUpdate();
                        }
                        else
                        {
                            Quit();
                        }
                    });
                }
            }
        });
    }

    public void StartUpdate()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (m_NotNetEnter)
            {
                OnInitComplete();
                LWDebug.LogWarning("无网络");
            }
            else {
                UIWidgetHelp.Instance.OpenMessageBox("请检查网络连接状态", retry =>
                {
                    if (retry)
                    {
                        StartUpdate();
                    }
                    else
                    {
                        Quit();
                    }
                }, "警告提示", "重试", "退出");
            }
            
        }
        else
        {
            Assets.DownloadVersions(error =>
            {
                if (!string.IsNullOrEmpty(error))
                {
                    if (m_NotNetEnter)
                    {
                        LWDebug.LogWarning("网络出现问题" + error);
                        OnInitComplete();
                    }
                    else
                    {
                        UIWidgetHelp.Instance.OpenMessageBox(string.Format("获取服务器版本失败：{0}", error), retry =>
                        {
                            if (retry)
                            {
                                StartUpdate();
                            }
                            else
                            {
                                Quit();
                            }
                        }, "警告提示", "重试", "退出");
                    }
                   
                }
                else
                {
                    UpdateAsset(Assets.patches4Init, "初始化更新", OnInitComplete);                    
                }
            });
        }
    }
    public void UpdateAsset(string []patchNameArray,string title,Action downloadCallback) {
        Downloader handler;
        // 按分包下载版本更新，返回true的时候表示需要下载，false的时候，表示不需要下载
        if (Assets.DownloadAll(patchNameArray, out handler))
        {
            var totalSize = handler.size;
            var tips = string.Format("需要下载 {0} 内容", Downloader.GetDisplaySize(totalSize));
            //自动更新不弹出确认框
            if (m_AutoUpdate)
            {
                DownLoadHandler(handler, downloadCallback);
                
            }
            else {
                UIWidgetHelp.Instance.OpenMessageBox(tips, download =>
                {
                    if (download)
                    {
                        DownLoadHandler(handler, downloadCallback);
                    }
                    else
                    {
                        Quit();
                    }
                }, title, "确认", "退出");
            }            
        }
        else
        {
            downloadCallback?.Invoke();          
        }
    }
    //下载控制
    void DownLoadHandler(Downloader handler, Action downloadCallback) {
        handler.onUpdate += delegate (long progress, long size, float speed)
        {
            //刷新界面
            OnMessage(string.Format("下载中...{0}/{1}, 速度：{2}",
                Downloader.GetDisplaySize(progress),
                Downloader.GetDisplaySize(size),
                Downloader.GetDisplaySpeed(speed)));
            OnProgress(progress * 1f / size);
        };
        handler.onFinished += downloadCallback;
        handler.onFinished += OnUpdateComplete;
        handler.Start();
        UIWidgetHelp.Instance.OpenLoadingBarView();
    }
    private void OnProgress(float progress)
    {
        UIWidgetHelp.Instance.SetAppVer(LWUtility.GlobalConfig.appVer);
        UIWidgetHelp.Instance.SetAssetVer(Assets.currentVersions.ver);
        //Debug.Log("更新进度：" + progress);
        UIWidgetHelp.Instance.SetLoadingBarValue(progress);
    }

    private void OnMessage(string msg)
    {      
        UIWidgetHelp.Instance.SetLoadMsg(msg);
    }

    private void OnInitComplete()
    {
        OnProgress(1);
        if (Assets.currentVersions != null) {
            Debug.Log("资源版本：" + Assets.currentVersions.ver);
        }
        OnMessage("更新完成");
        OnInitUpdateComplete?.Invoke(true);
    }
    //更新结束
    private void OnUpdateComplete()
    {
        UIWidgetHelp.Instance.CloseLoadingBarView();
    }
    //退出程序
    private void Quit()
    {
        OnInitUpdateComplete?.Invoke(false);
#if UNITY_EDITOR && !CREATEDLL
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
