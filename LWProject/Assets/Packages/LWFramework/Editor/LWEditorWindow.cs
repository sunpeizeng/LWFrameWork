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
using UnityEditor;
using UnityEngine;

public class LWEditorWindow : OdinMenuEditorWindow
{
    public string scriptPath;
    public static LWEditorWindow LWEditor;
    protected override OdinMenuTree BuildMenuTree()
    {
        scriptPath = this.GetType().Assembly.Location;
        scriptPath = scriptPath.Substring(0, scriptPath.LastIndexOf('\\') + 1);

        
        var tree = new OdinMenuTree();
        tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;
        tree.Selection.SupportsMultiSelect = false;

        LWGlobalConfigTool.Instance.ReadConfig();

        tree.Add("配置", LWGlobalConfigTool.Instance);
        tree.Add("DLL管理", new DLLManager());
#if XASSET
        //tree.Add("打包管理", Resources.Load("AssetBuildTool"));   
        tree.Add("打包管理", CreateInstance("LWAsset.Editor.dll", "AssetBuildTool"));  
        //tree.Add("打包规则", AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/Rules.asset"));
        tree.Add("分包打包规则", AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/RulesPatches/RulesPatchesManager.asset"));
        var allAssets = AssetDatabase.GetAllAssetPaths()
          .Where(x => x.StartsWith("Assets/RulesPatches/") && !x.Contains("Manager"))
          .OrderBy(x => x);
        foreach (var path in allAssets)
        {

            string menu = path.Substring("Assets/".Length);
            menu = menu.Replace("RulesPatches", "分包打包规则");
            menu = menu.Replace("RulesPatch", "");
            menu = menu.Replace(".asset", "");
            tree.AddAssetAtPath(menu, path);
        }
#endif
        // tree.Add("配置2", Resources.Load<LWGlobalAsset>("LWGlobalAsset"));     
        tree.Add("UI编辑配置", new UIEditConfig());
        tree.Add("其他", new OtherToolManger());
        tree.AddAssetAtPath("其他/打包规则", "Assets/Rules.asset");



        tree.EnumerateTree().AddThumbnailIcons();
        return tree;
    }
    /// <summary>
    /// 反射创建对象
    /// </summary>
    /// <param name="dllName">dll名称</param>
    /// <param name="typeName">类名</param>
    /// <returns></returns>
    object CreateInstance(string dllName, string typeName)
    {
        var bytes = File.ReadAllBytes(scriptPath + dllName);
        Assembly Assembly = System.Reflection.Assembly.Load(bytes);
        Type type = Assembly.GetType(typeName);
        return Activator.CreateInstance(type);
    }
    [MenuItem("LWFramework/打开工具",priority =1)]
    public static void OpenWindow()
    {
        LWEditor = GetWindow<LWEditorWindow>();
        LWEditor.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
        //GetWindow<LWEditorWindow>().ShowPopup();
    }
    [MenuItem("LWFramework/初始化框架", priority = 6)]
    static void InitFramework()
    {
        FileTool.CheckCreateDirectory(Application.dataPath+ "/@Resources");
        FileTool.CheckCreateDirectory(Application.dataPath + "/Resources");
        FileTool.CheckCreateDirectory(Application.dataPath + "/Scripts");
        FileTool.CheckCreateDirectory(Application.dataPath + "/StreamingAssets");
        FileTool.CheckCreateDirectory(Application.dataPath + "/RulesPatches");
        //if (!FileTool.ExistsFile(Application.dataPath + "/Resources/LWGlobalAsset.asset"))
        //{
        //    var asset = ScriptableObject.CreateInstance(typeof(LWGlobalAsset));
        //    AssetDatabase.CreateAsset(asset, "Assets/Resources/LWGlobalAsset.asset");
        //}
        AssetDatabase.Refresh();
    }
    [MenuItem("LWFramework/Test")]
    private static void OpenWindow222()
    {
        EditorWindow editorWindow = EditorWindow.GetWindow(typeof(SceneView));
        editorWindow.ShowNotification(new GUIContent("aaaaaaaaa"));
        //   GetWindow<TestWindow>()..ShowUtility();
    }



    [MenuItem("Assets/复制路径(Alt+C) &c")]
    static void CopyAssetPath()
    {
        if (EditorApplication.isCompiling)
        {
            return;
        }
        string path = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        GUIUtility.systemCopyBuffer = path;
        Debug.Log(string.Format("systemCopyBuffer: {0}", path));
    }
   
}


    
public class OtherToolManger 
{
    //[Title("查找脚本")]
    [LabelText("脚本"), FoldoutGroup("脚本")]
    public UnityEngine.Object scriptsName;
    [Button("查找脚本"), FoldoutGroup("脚本")]
    public void FildScripts()
    {
        var obj = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //关键代码，获取所有gameobject元素给数组obj
        foreach (GameObject child in obj)    //遍历所有gameobject
        {
            if (child.GetComponent(scriptsName.name))
            {
                Debug.Log(child.gameObject.name);
            }

        }
    }
  

    [TextArea(3,20), LabelText("数据"),FoldoutGroup("加密解密")]
    public string strData;
    [TextArea(1, 20), LabelText("Key"), FoldoutGroup("加密解密")]
    public string strKey;
    [Button("加密"), FoldoutGroup("加密解密")]
    public void AesEncryptOnClick()
    {
        Debug.Log(AESUtility.AesEncrypt(strData, strKey));
    }
    [Button("解密"), FoldoutGroup("加密解密")]
    public void AesDecryptOnClick()
    {
        Debug.Log(AESUtility.AesDecrypt(strData, strKey));
    }
    [Button("生成KEY"), FoldoutGroup("加密解密")]
    public void CreateKey()
    {
        LicenseKey.CreateKeyFile(SystemInfoTool.GetGraphicsDeviceID() + Application.productName);
    }
    [Button("日期"), FoldoutGroup("加密解密")]
    public void ShowDateKey()
    {
        LicenseDate licenseDate = new LicenseDate() { date = "2021-04-01 00:00:00" };
        Debug.Log(LitJson.JsonMapper.ToJson(licenseDate));
    }
    [Title("重命名"), FoldoutGroup("批量重命名")]
    [LabelText("名称")]
    public string strName;
    [Button("设置"), FoldoutGroup("批量重命名")]
    public void SettingName()
    {
        GameObject[]games = Selection.gameObjects;

        for (int i = 0; i < games.Length; i++)
        {
            games[i].name = strName + "_" + games[i].transform.GetSiblingIndex();

        }
    }
   
}