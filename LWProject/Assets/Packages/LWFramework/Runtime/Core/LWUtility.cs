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
        ///Manifestλ��
        /// </summary>
        public static string AssetsManifestAsset { get; private set; } = "Assets/Manifest.asset";

        /// <summary>
        /// �ȸ�dll������
        /// </summary>
        public static string HotfixFileName { get; private set; } = "hotfix.dll.byte";

        public static LoadDelegate loadDelegate = null;
        public static GetPlatformDelegate getPlatformDelegate = null;
        /// <summary>
        /// ��Ŀ��Ŀ¼
        /// </summary>
        public static string ProjectRoot { get; private set; } = Application.dataPath.Replace("/Assets", "");
        /// <summary>
        /// Library
        /// </summary>
        public static string Library { get; private set; } = ProjectRoot + "/Library";
        /// <summary>
        /// ��Դ·�� ����ʱһ��ΪStreamingAssets �༭���²��Է�ABģʽ��ΪSystem.Environment.CurrentDirectory����ʹ�� AssetDatabase.LoadAssetAtPathģ��AB����
        /// </summary>
        public static string dataPath { get; set; }
        /// <summary>
        /// ����·�� �༭���¶�ȡAssets/Manifest.asset�����Ϊ�ն�ȡab�е�Manifest   
        /// </summary>

        public static string downloadURL { get; set; }
        private static LWGlobalConfig _lwGlobalConfig;

        /// <summary>
        /// Resources�µ�ȫ�������ļ�
        /// </summary>
        public static LWGlobalConfig GlobalConfig
        {
            get
            {
                if (_lwGlobalConfig == null)
                {
                    //���Ȼ�ȡ�ⲿ��������
                    _lwGlobalConfig = ConfigDataTool.ReadData<LWGlobalConfig>("config.cfg",null, false);                 
                    if (_lwGlobalConfig == null)
                    {
                        LWDebug.LogError("������Resources�д��������ļ�");

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
        /// ��ȡ����ʱ��ǰƽ̨������
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
        /// persistentDataPath+ƽ̨���� +/
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