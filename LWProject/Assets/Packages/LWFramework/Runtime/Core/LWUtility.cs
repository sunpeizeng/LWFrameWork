using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LWFramework.Core
{
    public delegate Object LoadDelegate(string path, Type type);

    public delegate string GetPlatformDelegate();

    public static class LWUtility
    {
        public static string AssetBundles { get; private set; } = "AssetBundles";
        /// <summary>
        ///Manifest位置
        /// </summary>
        public static string AssetsManifestAsset { get; private set; } = "Assets/Manifest.asset";

        /// <summary>
        /// 热更dll的名称
        /// </summary>
        public static string HotfixFileName { get; private set; } = "hotfix.dll.byte";

        public static LoadDelegate loadDelegate = null;
        public static GetPlatformDelegate getPlatformDelegate = null;
        /// <summary>
        /// 项目根目录
        /// </summary>
        public static string ProjectRoot { get; private set; } = Application.dataPath.Replace("/Assets", "");
        /// <summary>
        /// Library
        /// </summary>
        public static string Library { get; private set; } = ProjectRoot + "/Library";
        /// <summary>
        /// 资源路径 运行时一般为StreamingAssets 编辑器下测试非AB模式则为System.Environment.CurrentDirectory便于使用 AssetDatabase.LoadAssetAtPath模拟AB加载
        /// </summary>
        public static string dataPath { get; set; }
        /// <summary>
        /// 下载路径 编辑器下读取Assets/Manifest.asset，如果为空读取ab中的Manifest   
        /// </summary>

        public static string downloadURL { get; set; }
        private static LWGlobalConfig _lwGlobalConfig;

        /// <summary>
        /// Resources下的全局配置文件
        /// </summary>
        public static LWGlobalConfig GlobalConfig
        {
            get
            {
                if (_lwGlobalConfig == null)
                {
                    //优先获取外部配置数据
                    _lwGlobalConfig = ConfigDataTool.ReadData<LWGlobalConfig>("config.cfg",null, false);                 
                    if (_lwGlobalConfig == null)
                    {
                        LWDebug.LogError("请先在Resources中创建配置文件");

//#if UNITY_EDITOR && !CREATEDLL
//                        if (lwGlobalAsset == null)
//                        {
//                            FileTool.CheckCreateDirectory(Application.dataPath + "/Resources");
//                            lwGlobalAsset = (LWGlobalAsset)ScriptableObject.CreateInstance(typeof(LWGlobalAsset));
//                            UnityEditor.AssetDatabase.CreateAsset(lwGlobalAsset, "Assets/Resources/LWGlobalAsset.asset");
//                            UnityEditor.AssetDatabase.Refresh();
//                        }
//#endif
//                        _lwGlobalConfig = lwGlobalAsset.GetLWGlobalConfig();
                    }
                    LWDebug.SetLogConfig(_lwGlobalConfig.lwGuiLog,_lwGlobalConfig.logLevel, _lwGlobalConfig.writeLog);

                }

                return _lwGlobalConfig;
            }
            set {
                _lwGlobalConfig = value;
                LWDebug.SetLogConfig(_lwGlobalConfig.lwGuiLog, _lwGlobalConfig.logLevel, _lwGlobalConfig.writeLog);
            }
        }
        /// <summary>
        /// 获取运行时当前平台的名称
        /// </summary>
        /// <returns></returns>
        public static string GetRuntimePlatform()
        {
            return getPlatformDelegate != null
                ? getPlatformDelegate()
                : GetPlatformForAssetBundles(Application.platform);
        }

        private static string GetPlatformForAssetBundles(RuntimePlatform platform)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXEditor:
                    return "OSX";
                default:
                    return null;
            }
        }
        /// <summary>
        /// persistentDataPath+平台名称 +/
        /// </summary>
        public static string updatePath
        {
            get
            {
                return Path.Combine(Application.persistentDataPath, Path.Combine(AssetBundles, GetRuntimePlatform())) +
                       Path.DirectorySeparatorChar;
            }
        }
    }
}