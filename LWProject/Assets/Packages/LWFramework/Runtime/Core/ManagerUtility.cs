using LWFramework.Audio;
using LWFramework.Core;
using LWFramework.FMS;
using LWFramework.Message;
using LWFramework.UI;
using LWFramework.WebRequest;


/// <summary>
/// Manager工具类
/// </summary>
public class ManagerUtility
{

    /// <summary>
    /// 获取资源管理类
    /// </summary>
    public static IAssetsManager AssetsMgr
    {
        get
        {
            return MainManager.Instance.GetManager<IAssetsManager>();
        }
    }
    /// <summary>
    /// 获取UI管理类
    /// </summary>
    public static IUIManager UIMgr {
        get
        {
            return MainManager.Instance.GetManager<IUIManager>();
        }
    }

    /// <summary>
    /// 获取FSMManager管理类 
    /// </summary>
    public static IFSMManager FSMMgr
    {
        get
        {
            return MainManager.Instance.GetManager<IFSMManager>();
        }
    }
    /// <summary>
    /// 获取热更代码
    /// </summary>
    public static IHotfixManager HotfixMgr {
        get
        {
            return MainManager.Instance.GetManager<IHotfixManager>();
        }
    }
    /// <summary>
    /// 获取消息管理类
    /// </summary>
    public static IMessageManager MessageMgr
    {
        get
        {
            return MainManager.Instance.GetManager<IMessageManager>();
        }
    }
    /// <summary>
    /// 获取Http网络管理类
    /// </summary>
    public static IWebRequestManager WebRequestMgr
    {
        get
        {
            return MainManager.Instance.GetManager<IWebRequestManager>();
        }
    }

    /// <summary>
    /// 步骤管理类
    /// </summary>
    public static IStepManager StepMgr
    {
        get
        {
            return MainManager.Instance.GetManager<IStepManager>();
        }
    }

    /// <summary>
    /// 高亮管理类
    /// </summary>
    public static IHighlightingManager HLMgr
    {
        get
        {
            return MainManager.Instance.GetManager<IHighlightingManager>();
        }
    }

    /// <summary>
    /// 音频管理类
    /// </summary>
    public static IAudioManager AudioMgr
    {
        get
        {
            return MainManager.Instance.GetManager<IAudioManager>();
        }
    }
    /// <summary>
    /// 文字转语音
    /// </summary>
    public static ISpeechManager SpeechManager
    {
        get
        {
            return MainManager.Instance.GetManager<ISpeechManager>();
        }
    }
    /// <summary>
    /// 获取全部表格数据
    /// </summary>
    public static TableData TableData {
        get
        {

            return MainManager.Instance.TableData;
        }
    }
   
}
