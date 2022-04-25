using LWFramework.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DLLManager
{
   
    [LabelText("过滤的dll名称或文件夹名称")]
    [OnValueChanged("SetFilterStrArray")]
    public string[] filterStrArray;


    public DLLManager()
    {
        filterStrArray = ConfigDataTool.ReadData<string[]>("filterString~", Application.dataPath, false);
    }
    [Button("编译dll", ButtonSizes.Medium)]
    public void BuildButton()
    {       
        var cmd = new DllBuild(Application.dataPath + "/@Resources/Hotfix");
        cmd.onFinished = ((DllBuild self, bool isSuccess) =>
        {
            var tip = isSuccess ? "Dll生成成功!" : "Dll生成失败!";
            Debug.Log(tip);
        });
        cmd.filterStrArray = filterStrArray;
        cmd.Execute();
        AssetDatabase.Refresh();
        SetFilterStrArray();
    }
    
    public void SetFilterStrArray()
    {
        ConfigDataTool.Create("filterString~", filterStrArray,false,Application.dataPath);
    }
    //   [HorizontalGroup("Split",0.5f)]
    //[Button("编译dll(Release)", ButtonSizes.Medium)]
    //public void Button()
    //{
    //    BuildDllScript.RoslynBuild(BuildDllTools.BuildMode.Release);
    //}
    ////   [HorizontalGroup("Split", 0.5f)]
    //[Button("编译dll(Debug)", ButtonSizes.Medium)]
    //public void Button2()
    //{
    //    BuildDllScript.RoslynBuild(BuildDllTools.BuildMode.Debug);
    //}
#if ILRUNTIME
    [Button("分析DLL生成绑定", ButtonSizes.Medium)]
    public void Button3()
    {
        BuildDllScript.GenCLRBindingByAnalysis(); ;
    }
    [Button("手动绑定生成", ButtonSizes.Medium)]
    public void Button4()
    {
        BuildDllScript.GenCLRBindingBySelf();
    }
    [Button("生成跨域Adapter")]
    public void Button5()
    {
        BuildDllScript.GenCrossBindAdapter();
    }
    [Button("生成Link.xml")]
    public void Button6()
    {
        StripCode.GenLinkXml();

    }
#endif
    [Button("添加IL包")]
    public void Button7()
    {
        bool exitsIL = FileTool.ExistsFile("Assets/LWFramework/ILRuntime/Ilr/CLRBinding.zip", LWUtility.ProjectRoot);
        if (!exitsIL)
        {
            DefineSymbolsTool.AddDefine("ILRUNTIME");
            FileTool.CopyDir(LWUtility.ProjectRoot + "/ILFile/ILRuntime", LWUtility.ProjectRoot + "/Assets/LWFramework");

        }
        AssetDatabase.Refresh();
    }
    [Button("删除IL包")]
    public void Button8()
    {
        bool exitsIL = FileTool.ExistsFile("Assets/LWFramework/ILRuntime/Ilr/CLRBinding.zip", LWUtility.ProjectRoot);
        if (exitsIL)
        {
            DefineSymbolsTool.DeleteDefine("ILRUNTIME");
            FileTool.DeleteDir(LWUtility.ProjectRoot + "/Assets/LWFramework/ILRuntime");
        }
        AssetDatabase.Refresh();
    }
    [Button("添加打包框架宏")]
    public void Button9()
    {
        DefineSymbolsTool.AddDefine("CREATEDLL");
        AssetDatabase.Refresh();
    }
    [Button("删除打包框架宏")]
    public void Button10()
    {
        DefineSymbolsTool.DeleteDefine("CREATEDLL");
        AssetDatabase.Refresh();
    }
    [Button("添加AB宏")]
    public void Button11()
    {
        DefineSymbolsTool.AddDefine("XASSET");
        AssetDatabase.Refresh();
    }
    [Button("删除AB宏")]
    public void Button12()
    {
        DefineSymbolsTool.DeleteDefine("XASSET");
        AssetDatabase.Refresh();
    }
}
public class FilterDll {
    public List<string> StrList;
}