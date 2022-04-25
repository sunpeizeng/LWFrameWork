using libx;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "RulesPatchesManager", menuName = "LWFramework/CreateRulesPatchesManager", order = 0)]
public class RulesPatchesManager : ScriptableObject
{
    [LabelText("所有分包管理器"), ListDrawerSettings(CustomAddFunction = "CreateRulesTeam", OnTitleBarGUI = "RefreshBtn")]
    public List<RulesPatch> m_RulesTeamArray = new List<RulesPatch>();

    [LabelText("选中的分包管理器")]
    public RulesPatch m_ChooseRulesPatch;
    [InfoBox("注意：此处修改需要重新打包AB才会生效。。如果编辑器下运行更新Patches In Build的Patches，不会进行下载", InfoMessageType.Warning)]
    public bool AllAssetToBuild;
    [Tooltip("首包包含的分包")]
    public string[] patchesInBuild = new string[0];
    [Tooltip("BuildPlayer的时候被打包的场景")]
    public SceneAsset[] scenesInBuild = new SceneAsset[0];

    /// <summary>
    /// Creates the rules team.
    /// </summary>
    void CreateRulesTeam()
    {

        string path = AssetDatabase.GetAssetPath(this);
        path = path.Substring(0, path.LastIndexOf("/") + 1) + "RulesPatch" + m_RulesTeamArray.Count + ".asset";

        var asset = AssetDatabase.LoadAssetAtPath<RulesPatch>(path);
        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<RulesPatch>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            m_RulesTeamArray.Add(asset);
        }
        m_ChooseRulesPatch = asset;
        AssetDatabase.Refresh();
    }
    /// <summary>
    /// 同步到主Rules按钮
    /// </summary>
    [Button("同步到主Rules")]
    public void BtnSyncRules()
    {
        int returnNumber = WindowsMessageBox.MessageBox(IntPtr.Zero, "是否继续操作", "提示框", 4);
        if (returnNumber == 7)
        {
            return;
        }
        SyncRules();
    }
    /// <summary>
    /// 同步规则
    /// </summary>
    public void SyncRules()
    {
        Refresh();
        var rules = BuildScript.GetBuildRules();
        rules.allAssetsToBuild = AllAssetToBuild;
        rules.patchesInBuild = patchesInBuild;
        rules.scenesInBuild = scenesInBuild;
        for (int i = 0; i < m_RulesTeamArray.Count; i++)
        {
            RulesPatch rulesPatche = m_RulesTeamArray[i];

            PatchBuild findPatchBuild = rules.patches.Find((PatchBuild find) =>
            {
                return find.name == rulesPatche.patchName;
            });
            //判断规则中是否包含当前分包
            if (findPatchBuild == null)
            {
                //当前不存在该分包，则创建新的分包
                PatchBuild patchBuild = new PatchBuild();
                patchBuild.name = rulesPatche.patchName;
                for (int j = 0; j < rulesPatche.assets.Count; j++)
                {
                    patchBuild.assets.Add(rulesPatche.assets[j].name);
                    CheckAssetBuild(rules, rulesPatche.assets[j]);
                }
                rules.patches.Add(patchBuild);
            }
            else
            {
                //当前存在该分包，则直接添加
                for (int j = 0; j < rulesPatche.assets.Count; j++)
                {
                    if (!findPatchBuild.assets.Contains(rulesPatche.assets[j].name))
                    {
                        findPatchBuild.assets.Add(rulesPatche.assets[j].name);
                    }
                    CheckAssetBuild(rules, rulesPatche.assets[j]);
                }
            }



        }
        Debug.Log("同步完成");
    }
    /// <summary>
    /// 刷新按钮
    /// </summary>
    public void RefreshBtn()
    {
        if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
        {
            Refresh();
        }

    }
    /// <summary>
    /// 刷新
    /// </summary>
    void Refresh()
    {
        var allAssets = AssetDatabase.GetAllAssetPaths()
          .Where(x => x.StartsWith("Assets/RulesPatches/") && !x.Contains("Manager"))
          .OrderBy(x => x).ToList();
        for (int i = 0; i < allAssets.Count; i++)
        {
            RulesPatch rulesPatch = AssetDatabase.LoadAssetAtPath<RulesPatch>(allAssets[i]);
            if (rulesPatch != null && !m_RulesTeamArray.Contains(rulesPatch))
            {
                m_RulesTeamArray.Add(rulesPatch);
            }
        }
    }
    void CheckAssetBuild(BuildRules rules, AssetBuild assetBuild)
    {
        AssetBuild findAssetBuild = rules.assets.Find((AssetBuild find) => {
            return find.name == assetBuild.name;
        });
        if (findAssetBuild == null)
        {
            rules.assets.Add(assetBuild);
        }
    }
    #region  分包右键打包代码
    [MenuItem("Assets/Team/GroupBy/None")]
    private static void GroupByNone()
    {
        GroupAssets(GroupBy.None);
    }

    [MenuItem("Assets/Team/GroupBy/Filename")]
    private static void GroupByFilename()
    {
        GroupAssets(GroupBy.Filename);
    }

    [MenuItem("Assets/Team/GroupBy/Directory")]
    private static void GroupByDirectory()
    {
        GroupAssets(GroupBy.Directory);
    }

    [MenuItem("Assets/Team/GroupBy/Explicit/shaders")]
    private static void GroupByExplicitShaders()
    {
        GroupAssets(GroupBy.Explicit, "shaders");
    }
    #endregion
    private static void GroupAssets(GroupBy nameBy, string bundle = null)
    {
        var selection = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets);
        RulesPatchesManager rulesPatchesManager = AssetDatabase.LoadAssetAtPath<RulesPatchesManager>("Assets/RulesPatches/RulesPatchesManager.asset");
        if (rulesPatchesManager == null)
        {
            WindowsMessageBox.MessageBox(IntPtr.Zero, "RulesPatches文件夹内没有RulesPatchesManager", "提示框", 0);
            return;
        }
        if (rulesPatchesManager.m_ChooseRulesPatch == null)
        {
            WindowsMessageBox.MessageBox(IntPtr.Zero, "RulesPatchesManager没有选中的分包资源", "提示框", 0);
            return;
        }
        foreach (var o in selection)
        {
            var path = AssetDatabase.GetAssetPath(o);
            if (string.IsNullOrEmpty(path) || Directory.Exists(path)) continue;
            rulesPatchesManager.m_ChooseRulesPatch.GroupAsset(path, nameBy, bundle);
        }

        EditorUtility.SetDirty(rulesPatchesManager.m_ChooseRulesPatch);
        AssetDatabase.SaveAssets();
    }
    internal void BuildBundlesFinish()
    {
        for (int i = 0; i < m_RulesTeamArray.Count; i++)
        {
            m_RulesTeamArray[i].SyncRules();
        }
    }
}
