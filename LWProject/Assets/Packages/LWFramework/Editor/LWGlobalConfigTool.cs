using LWFramework.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class LWGlobalConfigTool:Singleton<LWGlobalConfigTool>
{
    [InfoBox("config文件会通过ConfigDataTool去获取的，在不同的平台需要设置不同的ConfigFilePath，最好在Starup中主动设置。默认是Application.streamingAssetsPath。", InfoMessageType.Warning)]
    [InfoBox("例如Android-Application.persistentDataPath PC-LWUtility.ProjectRoot/Application.streamingAssetsPath  WebGL-Application.streamingAssetsPath", InfoMessageType.Warning)]
    [LabelText("程序版本号")]
    public int appVer;
    [LabelText("资源加载方式"), OnValueChanged("ChangeAssetMode")]
    public AssetMode assetMode;
    //[HideInInspector]
    //public bool development;//该属性用于适配XASSET 设置时不使用
    [LabelText("代码执行方式")]
    public HotfixCodeRunMode hotfixCodeRunMode;
    
    private int verifyBy = 1;
    [InfoBox("结构以（Bundles/）结尾"), ShowIf("assetMode", AssetMode.AssetBundleServer)]
    public string downloadURL;
    private string prefsKey = "urlbak";

    public string[] searchPaths;
    [InfoBox("在Init的时候直接更新的分包")]
    public string[] updatePatches4Init;
    [LabelText("全部更新"),Tooltip("更新所有的分包")]
    public bool updateAll;
    [LabelText("自动更新"), Tooltip("是否自动检测更新")]
    public bool autoCheckUpdate = true;
    [LabelText("检查AB文件"), Tooltip("是否自动检测AB是否存在")]
    public bool autoCheckExists = true;
    [LabelText("无网络进入"), Tooltip("无网络自动进入")]
    public bool notNetEnter = true;
    [LabelText("圆点log"), FoldoutGroup("Log")]
    public bool lwGuiLog = true;
    [LabelText("显示log等级"), FoldoutGroup("Log")]
    public LWLogLevel logLevel = LWLogLevel.All;
    [LabelText("是否生成日志"), FoldoutGroup("Log")]
    public bool writeLog = true;
    [LabelText("开启ABLog"), FoldoutGroup("Log")]
    public bool loggable = true;

    private bool isRead = false;


    public void CreateConfig()
    {
        if (!isRead)
            return;
        
        ConfigDataTool.Create("config.cfg", GetLWGlobalConfig(),false);
       // LWDebug.Log("创建LWGlobalConfigTool");
        
    }

    public void ChangeAssetMode()
    {
       // EditorPrefs.SetString(prefsKey, downloadURL);
        switch (assetMode)
        {
            case AssetMode.Resources:
                hotfixCodeRunMode = HotfixCodeRunMode.ByCode;
                downloadURL = "";
                break;
            case AssetMode.AssetBundleServer:
                downloadURL = EditorPrefs.GetString(prefsKey);
                EditorPrefs.SetString(prefsKey, downloadURL);
                break;
            case AssetMode.AssetBundleLocal:
                downloadURL = "";
                break;
            case AssetMode.AssetBundleDev:
                downloadURL = "";
                break;
            default:
                break;
        }
      
    }

    public void ReadConfig()
    {
        LWGlobalConfig config = ConfigDataTool.ReadData<LWGlobalConfig>("config.cfg",null,false);
        if (config != null)
        {
            this.appVer = config.appVer;
            this.assetMode = (AssetMode)config.assetMode;
            this.hotfixCodeRunMode = (HotfixCodeRunMode)config.hotfixCodeRunMode;
            this.lwGuiLog = config.lwGuiLog;
            this.logLevel = (LWLogLevel)config.logLevel;
            this.writeLog = config.writeLog;
            this.loggable = config.loggable;
            this.verifyBy = config.verifyBy;
            this.downloadURL = config.downloadURL;
            this.searchPaths = config.searchPaths;
            this.updatePatches4Init = config.updatePatches4Init;
            this.updateAll = config.updateAll;
            this.autoCheckExists = config.autoCheckExists;
            this.autoCheckUpdate = config.autoCheckUpdate;
            this.notNetEnter  = config.notNetEnter;

        }
        isRead = true;
    }
    public LWGlobalConfig GetLWGlobalConfig()
    {


        LWGlobalConfig globalConfig = new LWGlobalConfig
        {
            appVer = this.appVer,
            assetMode = (int)this.assetMode,
            hotfixCodeRunMode = (int)this.hotfixCodeRunMode,
            lwGuiLog = this.lwGuiLog,
            logLevel = (int)this.logLevel,
            writeLog = this.writeLog,
            loggable = this.loggable,
            verifyBy = this.verifyBy,
            downloadURL = this.downloadURL,
            searchPaths = this.searchPaths,
            updatePatches4Init = this.updatePatches4Init,
            updateAll = this.updateAll,
            autoCheckExists = this.autoCheckExists,
            autoCheckUpdate = this.autoCheckUpdate,
            notNetEnter = this.notNetEnter
        };
        return globalConfig;
    }
    [Button("生成配置", ButtonSizes.Medium)]
    private void BuildConfig()
    {
        //LWGlobalAsset configTool = Resources.Load<LWGlobalAsset>("LWGlobalAsset");
        //configTool.CreateJson();
        LWGlobalConfigTool.Instance.CreateConfig();
        string dataStr = LitJson.JsonMapper.ToJson(GetLWGlobalConfig());
        EditorPrefs.SetString("ConfigBak", dataStr);
    }
   
}