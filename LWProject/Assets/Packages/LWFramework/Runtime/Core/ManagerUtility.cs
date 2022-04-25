using LWFramework.Audio;
using LWFramework.Core;
using LWFramework.FMS;
using LWFramework.Message;
using LWFramework.UI;
using LWFramework.WebRequest;


/// <summary>
/// Manager������
/// </summary>
public class ManagerUtility
{

    /// <summary>
    /// ��ȡ��Դ������
    /// </summary>
    public static IAssetsManager AssetsMgr
    {
        get
        {
            return MainManager.Instance.GetManager<IAssetsManager>();
        }
    }
    /// <summary>
    /// ��ȡUI������
    /// </summary>
    public static IUIManager UIMgr {
        get
        {
            return MainManager.Instance.GetManager<IUIManager>();
        }
    }

    /// <summary>
    /// ��ȡFSMManager������ 
    /// </summary>
    public static IFSMManager FSMMgr
    {
        get
        {
            return MainManager.Instance.GetManager<IFSMManager>();
        }
    }
    /// <summary>
    /// ��ȡ�ȸ�����
    /// </summary>
    public static IHotfixManager HotfixMgr {
        get
        {
            return MainManager.Instance.GetManager<IHotfixManager>();
        }
    }
    /// <summary>
    /// ��ȡ��Ϣ������
    /// </summary>
    public static IMessageManager MessageMgr
    {
        get
        {
            return MainManager.Instance.GetManager<IMessageManager>();
        }
    }
    /// <summary>
    /// ��ȡHttp���������
    /// </summary>
    public static IWebRequestManager WebRequestMgr
    {
        get
        {
            return MainManager.Instance.GetManager<IWebRequestManager>();
        }
    }

    /// <summary>
    /// ���������
    /// </summary>
    public static IStepManager StepMgr
    {
        get
        {
            return MainManager.Instance.GetManager<IStepManager>();
        }
    }

    /// <summary>
    /// ����������
    /// </summary>
    public static IHighlightingManager HLMgr
    {
        get
        {
            return MainManager.Instance.GetManager<IHighlightingManager>();
        }
    }

    /// <summary>
    /// ��Ƶ������
    /// </summary>
    public static IAudioManager AudioMgr
    {
        get
        {
            return MainManager.Instance.GetManager<IAudioManager>();
        }
    }
    /// <summary>
    /// ����ת����
    /// </summary>
    public static ISpeechManager SpeechManager
    {
        get
        {
            return MainManager.Instance.GetManager<ISpeechManager>();
        }
    }
    /// <summary>
    /// ��ȡȫ���������
    /// </summary>
    public static TableData TableData {
        get
        {

            return MainManager.Instance.TableData;
        }
    }
   
}
