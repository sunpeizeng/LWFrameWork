using Cysharp.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class SpeechCacheManager : SpeechBaseManager,IManager
{
    private string m_DefaultPath;

    public override void Init() {
        base.Init();
        m_SpeechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Audio24Khz96KBitRateMonoMp3);
        //创建语音合成器
        m_Synthesizer = new SpeechSynthesizer(m_SpeechConfig, null);
        SetPath(Application.persistentDataPath + "/Speech");
    }
 
    public void SetPath(string path) {
        path = path.Trim();
        if (path == "")
        {
            m_DefaultPath = Application.persistentDataPath + "/Speech";
        }
        else {
            m_DefaultPath = path;            
        }
        FileTool.CheckCreateDirectory(m_DefaultPath);
    }
    public override async UniTask<bool> Play(string str, bool isDownload = false)
    {
        bool ret = false;
        string fileName = GetFileName(str);
        string filePath = $"{m_DefaultPath}/{fileName}.mp3";
        if (!FileTool.ExistsFile(filePath)) {
            var result = await m_Synthesizer.SpeakTextAsync(str);
            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                var stream = AudioDataStream.FromResult(result);
                await stream.SaveToWaveFileAsync(filePath);
                ret = true;
            }
            else
            {
                ret = false;
                LWDebug.LogError("转换出错了" + result.Reason);
            }
        }
        UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG);
        await uwr.SendWebRequest();
        AudioClip ac = DownloadHandlerAudioClip.GetContent(uwr);
        ManagerUtility.AudioMgr.Stop(m_AudioChannel);
        m_AudioChannel = ManagerUtility.AudioMgr.Play(ac);
        if (!isDownload)
        {
            FileTool.DeleteFile(filePath);
        }
        return ret;
    }
    public override async UniTask<bool> PlaySSML(string speakText, bool isDownload = false)
    {
        bool ret = await PlayLoadSSML(speakText, "", "", "", isDownload);
        return ret;
    }
    public override async UniTask<bool> PlaySSML(string speakText, string speaker, string style, string role, bool isDownload = false)
    {
        bool ret = await PlayLoadSSML(speakText, speaker, style, role, isDownload);
        return ret;
    }

    async UniTask<bool> PlayLoadSSML(string speakText, string speaker, string style, string role, bool isDownload = false) {
        bool ret = false;
        string fileName = GetFileName(speakText);
        string filePath = $"{m_DefaultPath}/{fileName}.mp3";
        if (!isDownload)
        {
            FileTool.DeleteFile(filePath);
        }

        if (!FileTool.ExistsFile(filePath))
        {
            SpeechSynthesisResult result = null;
            if (speaker != null || speaker != "")
            {
                result = await m_Synthesizer.SpeakSsmlAsync(CreateSSML(speakText, speaker, style, role));
               
            }
            else {
                result = await m_Synthesizer.SpeakSsmlAsync(CreateSSML(speakText));
                ret = false;
            }
           
            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                var stream = AudioDataStream.FromResult(result);
                await stream.SaveToWaveFileAsync(filePath);
                ret = true;
            }
            else
            {
                LWDebug.LogError("转换出错了" + result.Reason);
            }
        }
        UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG);
        await uwr.SendWebRequest();
        AudioClip ac = DownloadHandlerAudioClip.GetContent(uwr);
        ManagerUtility.AudioMgr.Stop(m_AudioChannel);
        m_AudioChannel = ManagerUtility.AudioMgr.Play(ac);
        if (!isDownload)
        {
            FileTool.DeleteFile(filePath);
        }
        return ret;
    }
    async UniTask<object> AA() {
        return 0;
    }
    void IManager.Update()
    {
    }
}
