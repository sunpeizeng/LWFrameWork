using LWFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//配置文件 用于读取
public class LWGlobalConfig
{
    public int appVer;
    public int assetMode;
    public int hotfixCodeRunMode;
    public bool lwGuiLog;
    public int logLevel;
    public bool writeLog;
    public bool loggable;
    public int verifyBy = 1;
    public string downloadURL;
    public string[] searchPaths;
    public string[] updatePatches4Init;
    public bool updateAll;
    public bool autoCheckUpdate;
    public bool autoCheckExists;
    public bool notNetEnter;
    public override string ToString()
    {
        return $"appVer:{appVer}---    assetMode:{(HotfixCodeRunMode)assetMode}---" +
            $"hotfixCodeRunMode:{(HotfixCodeRunMode)hotfixCodeRunMode}---" +
            $"downloadURL:{downloadURL}---" +
            $"updateAll:{updateAll}---" +
            $"autoUpdate:{autoCheckUpdate}";

    }
}

public enum AssetMode
{
    Resources = 0,
    AssetBundleServer = 1,
    AssetBundleLocal = 2,
    AssetBundleDev = 3
}