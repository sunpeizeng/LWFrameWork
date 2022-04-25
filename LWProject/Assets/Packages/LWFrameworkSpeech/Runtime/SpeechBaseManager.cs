using Cysharp.Threading.Tasks;
using LWFramework.Audio;
using Microsoft.CognitiveServices.Speech;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;

public abstract class SpeechBaseManager: IManager, ISpeechManager
{
    
    protected SpeechConfig m_SpeechConfig;
    protected SpeechSynthesizer m_Synthesizer;
    protected SpeechRecognizer m_Recognizer;
    protected AudioChannel m_AudioChannel;
    private string m_DefaultPath;
    public virtual void Init()
    {
        //初始化设置
        m_SpeechConfig = SpeechConfig.FromSubscription("3b015dc4c0a9418684f11e54596c63cd", "southeastasia");
        m_SpeechConfig.SpeechSynthesisLanguage = "zh-CN";
        m_SpeechConfig.SpeechRecognitionLanguage = "zh-CN";
       
        //创建语音合成器
        m_Synthesizer = new SpeechSynthesizer(m_SpeechConfig, null);
        //创建语音识别器
        try
        {
            m_Recognizer = new SpeechRecognizer(m_SpeechConfig);
        }
        catch (System.Exception)
        {
            LWDebug.LogError("请检查是否有麦克风,语音识别无法使用！");

        }
        


    }
    public abstract  UniTask<bool> Play(string str, bool isDownload = false);
    public abstract UniTask<bool> PlaySSML(string str, bool isDownload = false);
    public abstract UniTask<bool> PlaySSML(string str, string speaker, string style, string role, bool isDownload = false);

    public bool IsPlay {
        get {
          return m_AudioChannel==null ? false: m_AudioChannel.IsPlay;
        } 
    }

    public AudioChannel AudioChannel => throw new System.NotImplementedException();

    public void Stop() {
        ManagerUtility.AudioMgr.Stop(m_AudioChannel);
    }
    public void Pause()
    {
        ManagerUtility.AudioMgr.Pause(m_AudioChannel);
    }
    public void Resume()
    {
        ManagerUtility.AudioMgr.Resume(m_AudioChannel);
    }
    public async UniTask<string> Recognize() {
        if (m_Recognizer == null) {

            return "无效，请检查是否正常初始化";
        }

        var result = await m_Recognizer.RecognizeOnceAsync().ConfigureAwait(false);

        // Checks result.
        string newMessage = string.Empty;
        if (result.Reason == ResultReason.RecognizedSpeech)
        {
            newMessage = result.Text;
        }
        else if (result.Reason == ResultReason.NoMatch)
        {
            newMessage = "NOMATCH: Speech could not be recognized.";
        }
        else if (result.Reason == ResultReason.Canceled)
        {
            var cancellation = CancellationDetails.FromResult(result);
            newMessage = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";
        }
        return newMessage;
    }
    protected string CreateSSML(string str) {
        XElement speak = new XElement("speak");
        speak.Add(new XAttribute("version", "1.0"));
        speak.Add(new XAttribute(XNamespace.Xml + "lang", "zh-CN"));

        XElement voice = new XElement("voice");
        voice.Add(new XAttribute("name", "zh-CN-XiaoxiaoNeural"));
       

        if (str.Contains("{"))
        {
            Convert(voice, str);
        }
        else
        {
            voice.Add(str);
        }
        speak.Add(voice);
        return speak.ToString();
    }
    protected string CreateSSML(string str,string speaker,string style,string role)
    {
        XElement speak = new XElement("speak");
        speak.Add(new XAttribute("version", "1.0"));
        XNamespace mstts = "https://www.w3.org/2001/mstts";
        speak.Add(new XAttribute(XNamespace.Xmlns + "mstts", "https://www.w3.org/2001/mstts"));
        speak.Add(new XAttribute(XNamespace.Xml + "lang", "zh-CN"));

        XElement voice = new XElement("voice");
        voice.Add(new XAttribute("name", speaker));
        if (style != "" || role != "")
        {
            XElement express = new XElement(mstts + "express-as");
            if (style != "")
            {
                express.Add(new XAttribute("style", style));
            }
            if (role != "")
            {
                express.Add(new XAttribute("role", role));
            }

            if (str.Contains("{"))
            {
                Convert(express,str);
            }
            else {
                express.Add(str);
            }           
            voice.Add(express);
        }
        else {
            if (str.Contains("{"))
            {
                Convert(voice,str);
            }
            else
            {
                voice.Add(str);
            }
            
        }
        LWDebug.Log(voice);
        speak.Add(voice);
        return speak.ToString();
    }
    protected string GetFileName(string str) {
        str = str.Replace("{", "");
        str = str.Replace("}", "");
        str = str.Replace("[", "");
        str = str.Replace("]", "");
        str = str.Replace(":", "");
        if (str.Length < 10)
        {
            return str;
        }
        else {
            return str.Substring(0, 10) + "...";
        }
    }
    void IManager.Update()
    {
    }


    public void Convert(XElement xElement,string str) {
       
        List<string> strList = new List<string>();
        List<StringCustom> dataList = new List<StringCustom>();
        string[] stra1 = str.Split('{');
        for (int i = 0; i < stra1.Length; i++)
        {
            string[] stra2 = stra1[i].Split('}');
            strList.AddRange(stra2);
        }
        for (int i = 0; i < strList.Count; i++)
        {          
            if (strList[i].Contains("["))
            {
                string param = strList[i].Substring(strList[i].IndexOf('[') + 1, strList[i].IndexOf("]") - strList[i].IndexOf('[') - 1);
                string normalStr = strList[i].Substring(strList[i].IndexOf(']'), strList[i].Length- strList[i].IndexOf(']'));
                string[] pv = param.Split(':');
                string type = "";
                string value = "";
                switch (pv[0])
                {
                    case "速度":
                        type = "rate";
                        value = "+" + ((float.Parse(pv[1]) - 1) * 100) + "%";
                        break;
                    default:
                        break;
                }
                dataList.Add(new StringCustom() { hasXml = true, normalStr = normalStr, type = type, value = value });
            }
            else
            {
                dataList.Add(new StringCustom() { hasXml = false, normalStr = strList[i] });
            }

        }

        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].hasXml)
            {
                XElement prosody = new XElement("prosody");
                prosody.Add(new XAttribute(dataList[i].type, dataList[i].value));
                prosody.Add(dataList[i].normalStr);
                xElement.Add(prosody);
            }
            else {
                xElement.Add(dataList[i].normalStr);
            }
        }
    }
}

public class StringCustom
{
    public bool hasXml = false;
    public string normalStr;
    public string type;
    public string value;
}