
using Cysharp.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace LWFramework.Core
{

    /// <summary>
    /// 热更环境初始化处理
    /// </summary>
    public class HotfixManagerAsync : IManager, IHotfixManager
    {
        public Assembly Assembly { get; private set; }
        public void Init()
        {

        }

        public void LateUpdate()
        {

        }

        public void Update()
        {

        }
        /// <summary>
        /// 加载Dll热更脚本
        /// </summary>
        /// <param name="root">路径</param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public IEnumerator IE_LoadScript(HotfixCodeRunMode mode)
        {
            LWDebug.LogError($"{this} 当前未实现迭代加载方式,请使用其他实现");
            return null;
        }

        public async UniTaskVoid LoadScriptAsync(HotfixCodeRunMode mode)
        {
            string dllPath = "";
            if (Application.isEditor)
            {
                //这里情况比较复杂,Mobile上基本认为Persistent才支持File操作,
                dllPath = Application.dataPath + "/@Resources/Hotfix/" + LWUtility.HotfixFileName;
                //dllPath = Application.persistentDataPath + "/Bundles/@Resources/Hotfix/" + LWUtility.HotfixFileName;
            }
            else if(mode!= HotfixCodeRunMode.ByCode)
            {               
                dllPath = Application.persistentDataPath + "/Bundles/@Resources/Hotfix/" + LWUtility.HotfixFileName;
                //热更文件存放在persistentDataPath，判断如果不存在的话，则从streamingAssetsPath从复制过来
                if (!File.Exists(dllPath))
                {
                    var secondPath = Application.streamingAssetsPath + "/Bundles/@Resources/Hotfix/" + LWUtility.HotfixFileName; ;// Application.streamingAssetsPath + "/" + LWUtility.AssetBundles + "/" + LWUtility.GetPlatform() + "/" + LWUtility.HotfixFileName;
                    var request = UnityWebRequest.Get(secondPath);
                    LWDebug.Log("firstPath:" + dllPath);
                    LWDebug.Log("secondPath:" + secondPath);
                    await request.SendWebRequest();
                    if (request.isDone && request.error == null)
                    {
                        LWDebug.Log("request.downloadHandler.data:" + request.downloadHandler.data.Length);
                        LWDebug.Log("拷贝dll成功:" + dllPath);
                        byte[] results = request.downloadHandler.data;
                        FileTool.WriteByteToFile(LWUtility.HotfixFileName, results, Application.persistentDataPath + "/Bundles/@Resources/Hotfix/");
                    }

                }
            }

            LWDebug.Log("Dll路径:" + dllPath);
            //反射执行
            if (mode == HotfixCodeRunMode.ByReflection)
            {
                var bytes = File.ReadAllBytes(dllPath);
                var mdb = dllPath + ".mdb";
                if (File.Exists(mdb))
                {
                    var bytes2 = File.ReadAllBytes(mdb);
                    Assembly = Assembly.Load(bytes, bytes2);
                }
                else
                {
                    Assembly = Assembly.Load(bytes);
                }

                StartupBridge_Hotfix.StartReflection(Assembly);
            }
#if ILRUNTIME
            //解释执行
            else if (mode == HotfixCodeRunMode.ByILRuntime)
            {
                //解释执行模式
            //    ILRuntimeHelper.LoadHotfix(dllPath);
           //     ILRuntimeHelper.AppDomain.Invoke("StartupBridge_Hotfix", "Start", null, new object[] { true });
            }
#endif
            else if (mode == HotfixCodeRunMode.ByCode)
            {
                LWDebug.Log("内置code模式!", LogColor.green);
                StartupBridge_Hotfix.StartCode();
            }
        }
    }
}