using libx;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LWFramework.Core;
using UnityEditor;

public class RulesPatch : ScriptableObject
{
    [LabelText("分包名称"),HorizontalGroup()]
    public string patchName;
   
    [Button("修改名称"), HorizontalGroup()]
    public void ChangeName()
    {
        RulesPatchesManager rulesPatchesManager =  AssetDatabase.LoadAssetAtPath<RulesPatchesManager>("Assets/RulesPatches/RulesPatchesManager.asset");
        int oldIndex = rulesPatchesManager.m_RulesTeamArray.IndexOf(this);
        bool isChoose = rulesPatchesManager.m_ChooseRulesPatch == this;
        string filePath = LWUtility.ProjectRoot + "/" + AssetDatabase.GetAssetPath(this);
        string newName = "RulesPatch" + patchName;
        string newPath = filePath.Replace(this.name, newName);
        string newAssetPath = AssetDatabase.GetAssetPath(this).Replace(this.name, newName);
        FileInfo fi = new FileInfo(filePath); //xx/xx/aa.rar
        fi.MoveTo(newPath);
        AssetDatabase.Refresh();

        RulesPatch rulesPatch = AssetDatabase.LoadAssetAtPath<RulesPatch>(newAssetPath);
        rulesPatchesManager.m_RulesTeamArray[oldIndex] = rulesPatch;
        if (isChoose) {
            rulesPatchesManager.m_ChooseRulesPatch = rulesPatch;
        }
        rulesPatch.patchName = patchName;
    }
    [Header("缓存数据"), Tooltip("所有要打包的资源"), TableList(ShowIndexLabels = true, ShowPaging = true, NumberOfItemsPerPage = 15, MaxScrollViewHeight = 200, MinScrollViewHeight = 150)]
    public List<AssetBuild> assets = new List<AssetBuild>();
    
    public AssetBuild GroupAsset(string path, GroupBy groupBy = GroupBy.Filename, string group = null)
    {
        var value = assets.Find(assetBuild => assetBuild.name.Equals(path));
        if (value == null)
        {
            value = new AssetBuild();
            value.name = path;
            assets.Add(value);
        }
        if (groupBy == GroupBy.Explicit)
        {
            value.group = group;
        }
        if (path.EndsWith(".unity"))
        {
            patchName = Path.GetFileNameWithoutExtension(path);
        }
        value.groupBy = groupBy;

        return value;
    }
    [Button("打包AB",ButtonSizes.Medium)]
    public void BtnBuildRules()
    {
        int returnNumber = WindowsMessageBox.MessageBox(IntPtr.Zero, "单独打包仅限Dev及Local", "提示框", 4);
        if (returnNumber == 7)
        {
            return;
        }
        //将分包的数据同步到主包
        GetRulesPatchesManager().SyncRules();
        //处理主包规则
        BuildScript.BuildRules();
        //将主包的BundleBuild同步到当前分包
        SyncRules();
        List<BundleBuild> patchbundles = new List<BundleBuild>();
        List<BundleBuild> bundles = BuildScript.GetBuildRules().bundles;
        for (int i = 0; i < assets.Count; i++)
        {
            BundleBuild find = bundles.Find((BundleBuild check) => {
                return check.assetBundleName == assets[i].bundle;
            });
            if (!patchbundles.Contains(find) && find != null)
            {
                patchbundles.Add(find);
            }
        }
        var builds = patchbundles.ConvertAll(delegate (BundleBuild input) { return input.ToBuild(); }).ToArray();
        BuildScript.BuildAssetBundles(builds);
    }
    [Button("编辑"), ButtonGroup("设置")]
    public void SetChooseFile()
    {
        int returnNumber = WindowsMessageBox.MessageBox(IntPtr.Zero, "是否将当前分包设置为右键操作文件", "提示框", 4);
        if (returnNumber == 7)
        {
            return;
        }
        GetRulesPatchesManager().m_ChooseRulesPatch = this;
    }
    [Button("同步"), ButtonGroup("设置")]
    public void BtnSyncRules()
    {
        int returnNumber = WindowsMessageBox.MessageBox(IntPtr.Zero, "是否将主包数据同步到当前分包", "提示框", 4);
        if (returnNumber == 7)
        {
            return;
        }
        SyncRules();
    }
    public void SyncRules()
    {
        var rules = BuildScript.GetBuildRules();
        PatchBuild findPatchBuild = rules.patches.Find((PatchBuild find) =>
        {
            return find.name == patchName;
        });

        if (findPatchBuild != null)
        {
            assets.Clear();
            for (int i = 0; i < findPatchBuild.assets.Count; i++)
            {
                for (int j = 0; j < rules.assets.Count; j++)
                {
                    if (findPatchBuild.assets[i] == rules.assets[j].name)
                    {
                        assets.Add(rules.assets[j]);
                    }
                }
            }

        }

    }
    [Button("删除"), ButtonGroup("设置")]
    public void DeleteFile()
    {
        int returnNumber = WindowsMessageBox.MessageBox(IntPtr.Zero, "是否删除当前文件", "提示框", 4);
        if (returnNumber == 7)
        {
            return;
        }
        GetRulesPatchesManager().m_RulesTeamArray.Remove(this);
        string filePath = LWUtility.ProjectRoot + "/" + AssetDatabase.GetAssetPath(this);
        FileTool.DeleteFile(filePath);
        AssetDatabase.Refresh();
    }

   

    [Button("剔除"), ButtonGroup("设置")]
    public void FilterFile()
    {
        int returnNumber = WindowsMessageBox.MessageBox(IntPtr.Zero, "是否剔除无效文件", "提示框", 4);
        if (returnNumber == 7)
        {
            return;
        }
        for (int i = 0; i < assets.Count; i++)
        {
            UnityEngine.Object obj =  AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assets[i].name);
            if (!obj) {
                assets.RemoveAt(i);
                i--;
            }
        }
    }
    /// <summary>
    /// 获取本地RulesPatchesManager文件
    /// </summary>
    /// <returns></returns>
    public static RulesPatchesManager GetRulesPatchesManager()
    {
        return AssetDatabase.LoadAssetAtPath<RulesPatchesManager>("Assets/RulesPatches/RulesPatchesManager.asset");
    }
}