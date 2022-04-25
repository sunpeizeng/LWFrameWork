using Cysharp.Threading.Tasks;
using LWFramework.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpeechManager 
{
    /// <summary>
    /// 获取当前音频播放的AudioChannel
    /// </summary>
    AudioChannel AudioChannel { get; }
    /// <summary>
    /// 直接播放文字
    /// </summary>
    /// <param name="str">需要转换的文字</param>
    /// <param name="isDownload">是否下载，默认是false不下载</param>
    /// <returns></returns>
    UniTask<bool> Play(string str, bool isDownload = false);
    /// <summary>
    /// 播放SSML文字
    /// </summary>
    /// <param name="speakText">需要转换的文字</param>
    /// <param name="isDownload">是否下载，默认是false不下载</param>
    /// <returns></returns>
    UniTask<bool> PlaySSML(string speakText, bool isDownload = false);
    /// <summary>
    /// 播放SSML文字
    /// </summary>
    /// <param name="speakText">需要转换的文字</param>
    /// <param name="speaker">人物</param>
    /// <param name="style">风格</param>
    /// <param name="role">角色</param>
    /// <param name="num">编号可为空字符串</param>
    /// <param name="isDownload">是否下载，默认是false不下载</param>
    /// <returns></returns>
    UniTask<bool> PlaySSML(string speakText, string speaker= "zh-CN-XiaoxiaoNeural", string style="Default", string role = "Default", bool isDownload=false);
    /// <summary>
    /// 是否在播放
    /// </summary>
    bool IsPlay { get; }
    /// <summary>
    /// 暂停播放
    /// </summary>
    void Pause();
    /// <summary>
    /// 恢复播放
    /// </summary>
    void Resume();
    /// <summary>
    /// 停止播放
    /// </summary>
    void Stop();
    /// <summary>
    /// 识别语音
    /// </summary>
    /// <returns>返回识别的文字</returns>
    UniTask<string> Recognize();
}
