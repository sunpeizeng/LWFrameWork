using LWFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExportFramwork
{
    [MenuItem("Assets/导出框架")]
    static void ExportFramework()
    {       
        string path = EditorUtility.SaveFolderPanel("导出框架", LWUtility.ProjectRoot, "Framework");
        string lw_SavePath = path + "/LWFramework";
        string lwAsset_SavePath = path + "/LWFrameworkAsset";
        string lwSpeech_SavePath = path + "/LWFrameworkSpeech";
        string lwStepGraph_SavePath = path + "/LWFrameworkStep";
        string lwWebRequest_SavePath = path + "/LWFrameworkWebRequest";

        string lw_Path = Application.dataPath + "/Packages/LWFramework";
        string lwAsset_Path = Application.dataPath + "/Packages/LWFrameworkAsset";
        string lwSpeech_Path = Application.dataPath + "/Packages/LWFrameworkSpeech";
        string lwStepGraph_Path = Application.dataPath + "/Packages/LWFrameworkStep";
        string lwWebRequest_Path = Application.dataPath + "/Packages/LWFrameworkWebRequest";
        string uniTask_Path = Application.dataPath + "/Packages/UniTask";

        //删除所有老版本
        FileTool.DeleteDir(path);
        //创建新版的文件夹
        FileTool.CheckCreateDirectory(path);

        FileTool.CopyDir(lw_Path+ "/Editor", lw_SavePath);
        FileTool.CopyDir(lw_Path + "/Components", lw_SavePath);
        FileTool.CopyDir(lw_Path + "/Plugins", lw_SavePath);
        FileTool.CopyDir(lw_Path + "/Runtime/Prefabs", lw_SavePath);       
        FileTool.CopyDir(lwAsset_Path + "/Editor", lwAsset_SavePath);

        FileTool.CopyDir(lwSpeech_Path + "/Plugins", lwSpeech_SavePath);
        FileTool.CopyDir(lwStepGraph_Path + "/Editor", lwStepGraph_SavePath);
        FileTool.CheckCreateDirectory(lwWebRequest_SavePath);
        FileTool.CopyDir(uniTask_Path, path);

        UpFramework(lw_Path);
        UpFramework(lwAsset_Path);
        UpFramework(lwStepGraph_Path);
        UpFramework(lwWebRequest_Path);
        FileTool.CopyFile(lw_Path + "/package.json", lw_SavePath + "/package.json", true);
        FileTool.CopyFile(lwAsset_Path + "/package.json", lwAsset_SavePath + "/package.json", true);
        FileTool.CopyFile(lwSpeech_Path + "/package.json", lwSpeech_SavePath + "/package.json", true);
        FileTool.CopyFile(lwStepGraph_Path + "/package.json", lwStepGraph_SavePath + "/package.json", true);
        FileTool.CopyFile(lwWebRequest_Path + "/package.json", lwWebRequest_SavePath + "/package.json", true);


        FileTool.CopyFile(LWUtility.ProjectRoot+ "/Library/ScriptAssemblies/LW.Runtime.dll" , lw_SavePath + "/LW.Runtime.dll", true);
        FileTool.CopyFile(LWUtility.ProjectRoot + "/Library/ScriptAssemblies/LWAsset.Runtime.dll", lwAsset_SavePath + "/LWAsset.Runtime.dll", true);
        FileTool.CopyFile(LWUtility.ProjectRoot + "/Library/ScriptAssemblies/LWSpeech.Runtime.dll", lwSpeech_SavePath + "/LWSpeech.Runtime.dll", true);
        FileTool.CopyFile(LWUtility.ProjectRoot + "/Library/ScriptAssemblies/LWStep.Runtime.dll", lwStepGraph_SavePath + "/LWStep.Runtime.dll", true);
        FileTool.CopyFile(LWUtility.ProjectRoot + "/Library/ScriptAssemblies/LWWebRequest.dll", lwWebRequest_SavePath + "/LWWebRequest.dll", true);
    }

    //[MenuItem("Assets/修改框架")]
    static void UpFramework(string path)
    {
        //ExportPackage packageInfo = ConfigDataTool.ReadData<ExportPackage>("package.json", false, path);
        //string[] versions = packageInfo.version.Split('.');
        //int PATCH = int.Parse(versions[2]);
        //PATCH++;
        //packageInfo.version = versions[0]+"." + versions[1] + "." + PATCH;
        //ConfigDataTool.Create("package.json", packageInfo, false, path);
        //LWDebug.Log(packageInfo.name);
    }
}
public class ExportPackage
{
    /// <summary>
    /// 
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string displayName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string version { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string description { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string author { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string unity { get; set; }
}