using LWFramework.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LWGlobalAsset", menuName = "LWFramework/LWGlobalAsset", order = 0)]
//配置资源文件 对外编辑
public class LWGlobalAsset : ScriptableObject
{
    /**
     [ReadOnly]
     public int appVer;
     [LabelText("资源加载方式")]
     public AssetMode assetMode = AssetMode.Resources;
     //热更模式
     [LabelText("代码执行方式")]
     public HotfixCodeRunMode hotfixCodeRunMode = HotfixCodeRunMode.ByCode;
     public bool loggable;
     public int verifyBy = 1 ;
     [InfoBox("结构以（Bundles/）结尾")]
     public string downloadURL;
   
     [HideInInspector]
     //public bool development;//该属性用于适配XASSET 设置时不使用
     public string[] searchPaths;
     [InfoBox("在Init的时候直接更新的分包")]
     public string[] updatePatches4Init;
     public bool updateAll;
     [InfoBox("是否自动检测更新")]
     public bool autoCheckUpdate;


     //public LWGlobalConfig GetLWGlobalConfig() {
     //    LWGlobalConfig globalConfig = new LWGlobalConfig
     //    {
     //        appVer = this.appVer,
     //        assetMode = (int)this.assetMode,
     //        hotfixCodeRunMode = (int)this.hotfixCodeRunMode,
     //        loggable = this.loggable,
     //        verifyBy = this.verifyBy,
     //        downloadURL = this.downloadURL,
     //        searchPaths = this.searchPaths,
     //        updatePatches4Init = this.updatePatches4Init,
     //        updateAll = this.updateAll,
     //        autoCheckUpdate = this.autoCheckUpdate
     //    };
     //    return globalConfig;
     //}

     //[Button("创建外部配置数据")]
     //public void CreateJson() {
     //    ConfigDataTool.Create("config", GetLWGlobalConfig(),false);
     //}
     [Button("删除外部配置数据")]
     public void DeleteJson()
     {
         ConfigDataTool.Delete("config");
     }
     [Button("测试查看")]
     public void TestJson()
     {
       LWDebug.Log(  ConfigDataTool.ReadData<LWGlobalConfig>("config").downloadURL);
     }
     [Button("测试Web查看")]
     public async void TestJson2()
     {
         LWGlobalConfig config = await ConfigDataTool.ReadDataAsync<LWGlobalConfig>("config");
         LWDebug.Log(config);
     }
     */
}

