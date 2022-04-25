using Cysharp.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SpeechManager : SpeechBaseManager, IManager
{
   
    public override async UniTask<bool> Play(string str, bool isDownload = false)
    {
        bool ret = false;
        var result = await m_Synthesizer.SpeakTextAsync(str);
        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
        {
            
            var audioClip = ResultToAudioClip(result);
            ManagerUtility.AudioMgr.Stop(m_AudioChannel);
            m_AudioChannel = ManagerUtility.AudioMgr.Play(audioClip);
            ret = true;
        }
        else
        {
            LWDebug.LogError("转换出错了" + result.Reason);
        }
        return ret;
    }
    public override async UniTask<bool> PlaySSML(string speakText, bool isDownload = false)
    {
        bool ret = false;
        var result = await m_Synthesizer.SpeakSsmlAsync(CreateSSML(speakText));
        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
        {

            var audioClip = ResultToAudioClip(result);
            ManagerUtility.AudioMgr.Stop(m_AudioChannel);
            m_AudioChannel = ManagerUtility.AudioMgr.Play(audioClip);
            ret = true;
        }
        else
        {
            LWDebug.LogError("转换出错了" + result.Reason);
        }
        return ret;
    }
    public override async UniTask<bool> PlaySSML(string speakText, string speaker, string style, string role, bool isDownload = false)
    {
        bool ret = false;
        var result = await m_Synthesizer.SpeakSsmlAsync(CreateSSML(speakText, speaker, style, role));
        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
        {

            var audioClip = ResultToAudioClip(result);
            ManagerUtility.AudioMgr.Stop(m_AudioChannel);
            m_AudioChannel = ManagerUtility.AudioMgr.Play(audioClip);
            ret = true;
        }
        else
        {
            LWDebug.LogError("转换出错了" + result.Reason);
        }
        return ret;
    }
    //讲语音文件转换成AudioClip
    AudioClip ResultToAudioClip(SpeechSynthesisResult result) {
      
        var sampleCount = result.AudioData.Length / 2;
        var audioData = new float[sampleCount];
        for (var i = 0; i < sampleCount; ++i)
        {
            audioData[i] = (short)(result.AudioData[i * 2 + 1] << 8 | result.AudioData[i * 2]) / 32768.0F;
        }
        var audioClip = AudioClip.Create("SynthesizedAudio", sampleCount, 1, 16000, false);
        audioClip.SetData(audioData, 0);
        return audioClip;
    }
    async UniTaskVoid SaveFile(SpeechSynthesisResult result,string fileName)
    {
        var stream = AudioDataStream.FromResult(result);
        await stream.SaveToWaveFileAsync($"./{fileName}.mp3");
    }
    void IManager.Update()
    {
    }
}
