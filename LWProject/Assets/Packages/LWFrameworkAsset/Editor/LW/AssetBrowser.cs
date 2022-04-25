using libx;
using LWFramework.Core;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
//[Searchable]
public class AssetBrowser : OdinEditorWindow
{
    public static AssetBrowser window;
    private static BuildRules m_BuildRules;
    //[ TableList(ShowPaging = true, NumberOfItemsPerPage = 20,ShowIndexLabels = true)]
    [Searchable, TableList(ShowPaging = true, NumberOfItemsPerPage = 20, ShowIndexLabels = true)]
    public List<AssetBundleData> AssetBundleDataList = new List<AssetBundleData>();
    public string PatcheName;


  

    [MenuItem("LWFramework/资源浏览器", priority = 5)]
    public static void OpenWindow()
    {
        window = GetWindow<AssetBrowser>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
      
    }
    
    
    //[Button("查找")]
    //public void Find(string value) {
    //    if (value == "" || value ==null)
    //    {
    //        for (int i = 0; i < AssetBundleDataList.Count; i++)
    //        {
    //            AssetBundleDataList[i].UnChoose(); 

    //        }
    //    }
    //    else {
    //        for (int i = 0; i < AssetBundleDataList.Count; i++)
    //        {

    //            bool isFind = false;
    //            if (AssetBundleDataList[i].AssetBundleName.Contains(value))
    //            {
    //                AssetBundleDataList[i].Choose();
    //                continue;
    //            }
    //            else
    //            {
    //                for (int j = 0; j < AssetBundleDataList[i].AssetDataList.Count; j++)
    //                {
    //                    if (AssetBundleDataList[i].AssetDataList[j].AssetPath.Contains(value))
    //                    {
    //                        AssetBundleDataList[i].Choose();
    //                        isFind = true;
    //                        break;
    //                    }
    //                }
    //            }
    //            if (!isFind)
    //            {
    //                AssetBundleDataList[i].UnChoose();
    //            }
    //            else
    //            {
    //                continue;
    //            }
    //        }
    //    }
        
    //}
  

    [Button("刷新")]
    public void Refresh() {
        AssetBundleDataList.Clear();
        var dir = LWUtility.ProjectRoot+ "/"+  Assets.Bundles+"/"+ BuildScript.GetPlatformName()+ "/versions.bundle" ;
        m_BuildRules = AssetDatabase.LoadAssetAtPath<BuildRules>("Assets/Rules.asset");
        var versions = Assets.LoadVersions(dir);
       
        List<PatchesData> PatcherList = new List<PatchesData>();
        for (int i = 0; i < versions.patches.Count; i++)
        {
            PatchesData patchesData = new PatchesData();
            //分包名称
            patchesData.PatcheName = versions.patches[i].name;
            for (int j = 0; j < versions.bundles.Count; j++)
            {
                if (versions.patches[i].files.Contains(versions.bundles[j].id)){

                    patchesData.BundleNames.Add(versions.bundles[j].name);
                }
            }
            PatcherList.Add(patchesData);
        }
     
        for (int i = 0; i < PatcherList.Count; i++)
        {          
            for (int m = 0; m < m_BuildRules.bundles.Count; m++)
            {
                if (PatcherList[i].BundleNames.Contains(m_BuildRules.bundles[m].assetBundleName)){
                    if (PatcheName != "" && PatcheName == PatcherList[i].PatcheName)
                    {
                        AssetBundleData assetBundleData = new AssetBundleData();
                        //设置分包名
                        assetBundleData.PatcheName = PatcherList[i].PatcheName;
                        BundleBuild bundleBuild = m_BuildRules.bundles[m];
                        //设置Bundle名
                        assetBundleData.AssetBundleName = bundleBuild.assetBundleName;
                        assetBundleData.AssetDataList = new List<AssetData>();
                        //处理具体资源
                        for (int n = 0; n < bundleBuild.assetNames.Count; n++)
                        {
                            AssetData assetData = new AssetData();
                            assetData.Asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(bundleBuild.assetNames[n]);
                            assetData.AssetPath = bundleBuild.assetNames[n];
                            assetBundleData.AssetDataList.Add(assetData);
                        }
                        //计算Bundle大小
                        var fileInfo = new FileInfo(Environment.CurrentDirectory + "/" + BuildScript.outputPath + "/" + assetBundleData.AssetBundleName);
                        var fileSize = fileInfo.Length;
                        assetBundleData.Size = GetFormatSizeString((int)fileSize, 1024, "#,##0.##");

                        AssetBundleDataList.Add(assetBundleData);
                    }
                    else if (PatcheName==null ||PatcheName == ""){
                        AssetBundleData assetBundleData = new AssetBundleData();
                        //设置分包名
                        assetBundleData.PatcheName = PatcherList[i].PatcheName;
                        BundleBuild bundleBuild = m_BuildRules.bundles[m];
                        //设置Bundle名
                        assetBundleData.AssetBundleName = bundleBuild.assetBundleName;
                        assetBundleData.AssetDataList = new List<AssetData>();
                        //处理具体资源
                        for (int n = 0; n < bundleBuild.assetNames.Count; n++)
                        {
                            AssetData assetData = new AssetData();
                            assetData.Asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(bundleBuild.assetNames[n]);
                            assetData.AssetPath = bundleBuild.assetNames[n];
                            assetBundleData.AssetDataList.Add(assetData);
                        }
                        //计算Bundle大小
                        var fileInfo = new FileInfo(Environment.CurrentDirectory + "/" + BuildScript.outputPath + "/" + assetBundleData.AssetBundleName);
                        var fileSize = fileInfo.Length;
                        assetBundleData.Size = GetFormatSizeString((int)fileSize, 1024, "#,##0.##");

                        AssetBundleDataList.Add(assetBundleData);
                    }
                   
                }
               
            }
        }


       
    }
   
    public static string GetFormatSizeString(int size, int p, string specifier)
    {
        var suffix = new[] { "", "K", "M", "G", "T", "P", "E", "Z", "Y" };
        int index = 0;

        while (size >= p)
        {
            size /= p;
            index++;
        }

        return string.Format(
            "{0}{1}B",
            size.ToString(specifier),
            index < suffix.Length ? suffix[index] : "-"
        );
    }

    //[MenuItem("1/1")]
    public static void menu()
    {
        Texture target = Selection.activeObject as Texture;
        System.Type type = System.Reflection.Assembly.Load("UnityEditor.dll").GetType("UnityEditor.TextureUtil");
        System.Reflection.MethodInfo methodInfo = type.GetMethod("GetStorageMemorySize", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        Debug.Log("内存占用：" + EditorUtility.FormatBytes(UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(target)));
        Debug.Log("硬盘占用：" + EditorUtility.FormatBytes((int)methodInfo.Invoke(null, new object[] { target })));


    }
}

/// <summary>
/// 一行列表数据
/// </summary>
[Serializable]
public class AssetBundleData
{
    [TableColumnWidth(120, false), ReadOnly]
    public string PatcheName;
    [GUIColor("SetColor"), ReadOnly]
    public string AssetBundleName;
    [TableColumnWidth(50,false),ReadOnly]
    public string Size;
    [TableList]
    public List<AssetData> AssetDataList;
    [HideInInspector]
    public bool isChoose;
    public void Choose() {
        isChoose = true;
    }
    Color SetColor() {
        if(isChoose)
            return Color.red;
        else
            return Color.white;
    }
    public void UnChoose() {
        isChoose = false;
    }
    [Button("Size"), TableColumnWidth(50, false)]
    public void SetSize()
    {

        for (int i = 0; i < AssetDataList.Count; i++)
        {
            var fileInfo = new FileInfo(Environment.CurrentDirectory + "/" + AssetDataList[i].AssetPath);
            var fileSize = fileInfo.Length;
            AssetDataList[i].Size = AssetBrowser.GetFormatSizeString((int)fileSize, 1024, "#,##0.##");

        }

      
    }

   
}
/// <summary>
/// 一行资源数据
/// </summary>
[Serializable]
public class AssetData {
    [TableColumnWidth(150, false)]
    public UnityEngine.Object Asset;
    [TableColumnWidth(50, false), ReadOnly]
    public string Size;
    [HideInInspector]
    public string AssetPath;
}

[Serializable]
public class PatchesData
{
    public string PatcheName;
    public List<string> BundleNames = new List<string> ();
}