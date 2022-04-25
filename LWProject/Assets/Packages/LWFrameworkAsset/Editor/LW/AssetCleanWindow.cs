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
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

public class AssetCleanWindow : OdinEditorWindow
{
    public static AssetCleanWindow window;
    private static BuildRules m_BuildRules;


    [LabelText("清理文件夹"), LabelWidth(90), FolderPath(ParentFolder = "Assets"), OnValueChanged("SavePath"), HorizontalGroup("A")]
    public string CleanPath;
    [LabelText("显示使用的文件"), LabelWidth(90), HorizontalGroup("B", Width = 150)]
    public bool IsShowUseAsset;

    [LabelText("多选资源"), LabelWidth(60), HorizontalGroup("工具", Width = 150), OnValueChanged("ChooseAssetsAction")]
    public bool ChooseAssets;

    [TableList(ShowPaging = true, NumberOfItemsPerPage = 20, ShowIndexLabels = true)]
    public List<AssetCleanData> UnUseAssetList = new List<AssetCleanData>();
    [TableList(ShowPaging = true, NumberOfItemsPerPage = 20, ShowIndexLabels = true)]
    public List<AssetCleanData> UseAssetList = new List<AssetCleanData>();

    protected override void OnEnable()
    {        
        CleanPath = EditorPrefs.GetString("AssetCleanPath", "");
    }
    void SavePath() {
        EditorPrefs.SetString("AssetCleanPath", CleanPath);
    }

    [MenuItem("LWFramework/资源清理器", priority = 5)]
    public static void OpenWindow()
    {
        window = GetWindow<AssetCleanWindow>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);

    }


    [Button("刷新"), HorizontalGroup("B")]
    public void Refresh() {
       
        UnUseAssetList.Clear();
        UseAssetList.Clear();
        var dir = LWUtility.ProjectRoot + "/" + Assets.Bundles + "/" + BuildScript.GetPlatformName() + "/versions.bundle";
        m_BuildRules = AssetDatabase.LoadAssetAtPath<BuildRules>("Assets/Rules.asset");
        var versions = Assets.LoadVersions(dir);
        List<string> useAssetPath = new List<string>();
        for (int i = 0; i < m_BuildRules.bundles.Count; i++)
        {
            BundleBuild bundleBuild = m_BuildRules.bundles[i];
            //处理具体资源
            for (int n = 0; n < bundleBuild.assetNames.Count; n++)
            {


                useAssetPath.Add(bundleBuild.assetNames[n]);
            }
        }
        // Find assets
        var files = Directory.GetFiles("Assets/" + CleanPath, "*.*", SearchOption.AllDirectories)
            .Where(item => Path.GetExtension(item) != ".meta")
            .Where(item => Path.GetExtension(item) != ".js")
            .Where(item => Path.GetExtension(item) != ".cs")
            .Where(item => Path.GetExtension(item) != ".dll")
            .Where(item => Regex.IsMatch(item, "[\\/\\\\]Gizmos[\\/\\\\]") == false)
            .Where(item => Regex.IsMatch(item, "[\\/\\\\]Plugins[\\/\\\\]Android[\\/\\\\]") == false)
            .Where(item => Regex.IsMatch(item, "[\\/\\\\]Plugins[\\/\\\\]iOS[\\/\\\\]") == false)
            .Where(item => Regex.IsMatch(item, "[\\/\\\\]Resources[\\/\\\\]") == false);

        foreach (var path in files)
        {
            //var guid = AssetDatabase.AssetPathToGUID(path);
            string path_rep = path.Replace("\\", "/");
            if (!useAssetPath.Contains(path_rep))
            {
                AssetCleanData assetData = new AssetCleanData();
                assetData.Asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path_rep);
                assetData.AssetPath = path_rep;
                UnUseAssetList.Add(assetData);
            }
            else if (useAssetPath.Contains(path_rep) && IsShowUseAsset) {
                AssetCleanData assetData = new AssetCleanData();
                assetData.Asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path_rep);
                assetData.AssetPath = path_rep;
                UseAssetList.Add(assetData);
            }
        }
    }
    [Button("删除"), HorizontalGroup("工具")]
    public void Delete() {
        int returnNumber = WindowsMessageBox.MessageBox(IntPtr.Zero, "是否继续操作", "提示框", 4);
        if (returnNumber == 7)
        {
            return;
        }
        for (int i = 0; i < UnUseAssetList.Count; i++)
        {
            if (UnUseAssetList[i].O) {
                string oldPath = LWUtility.ProjectRoot + "/" + UnUseAssetList[i].AssetPath;
                FileTool.DeleteFile(oldPath);
            }
           

        }
        AssetDatabase.Refresh();
    }
    [Button("移至备份"), HorizontalGroup("工具")]
    public void Backup()
    {
        int returnNumber = WindowsMessageBox.MessageBox(IntPtr.Zero, "是否继续操作", "提示框", 4);
        if (returnNumber == 7)
        {
            return;
        }
        for (int i = 0; i < UnUseAssetList.Count; i++)
        {
            if (UnUseAssetList[i].O)
            {
                string oldPath = LWUtility.ProjectRoot + "/" + UnUseAssetList[i].AssetPath;
                string newPath = oldPath.Replace("Assets", "Backups");
                FileInfo fi = new FileInfo(oldPath);
                string dir = newPath.Substring(0, newPath.LastIndexOf("/"));
                FileTool.CheckCreateDirectory(dir);
                if (FileTool.ExistsFile(newPath))
                {
                    FileTool.DeleteFile(oldPath);
                }
                else
                {
                    fi.MoveTo(newPath);
                }
            }
        }
        AssetDatabase.Refresh();

    }

    void ChooseAssetsAction(){
        for (int i = 0; i < UnUseAssetList.Count; i++)
        {
            UnUseAssetList[i].O = ChooseAssets;
        }
    }
}
/// <summary>
/// 一行资源数据
/// </summary>
[Serializable]
public class AssetCleanData
{
    [TableColumnWidth(20, false)]
    public bool O;
    [TableColumnWidth(150, false)]
    public UnityEngine.Object Asset;
    public string AssetPath;
    [Button("删除"),TableColumnWidth(100, false)]
    public void Delete() {
        string filePath = LWUtility.ProjectRoot + "/" + AssetPath;
        FileTool.DeleteFile(filePath);
        AssetDatabase.Refresh();
    }
    [Button("移至备份"), TableColumnWidth(100, false)]
    public void Backup() {
        string oldPath = LWUtility.ProjectRoot + "/" + AssetPath;
        string newPath = oldPath.Replace("Assets", "Backups");
        FileInfo fi = new FileInfo(oldPath);
        string dir = newPath.Substring(0, newPath.LastIndexOf("/"));
        FileTool.CheckCreateDirectory(dir);
        if (FileTool.ExistsFile(newPath))
        {
            FileTool.DeleteFile(oldPath);
        }
        else
        {
            fi.MoveTo(newPath);
        }
        AssetDatabase.Refresh();
    }
}