using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.Audio {
    public interface IAudioManager
    {
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="clip">音频文件</param>
        /// <param name="loop">是否循环 默认-false</param>
        /// <returns></returns>
        AudioChannel Play(AudioClip clip, bool loop = false);
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="clip">音频文件</param>
        /// <param name="emitter">播放跟随的Transform</param>
        /// <param name="loop">是否循环 默认-false</param>
        /// <returns></returns>
        AudioChannel Play(AudioClip clip, Transform emitter, bool loop = false);
        /// <summary>
        ///  播放音频
        /// </summary>
        /// <param name="clip">音频文件</param>
        /// <param name="point">声音播放的位置</param>
        /// <param name="loop">是否循环 默认-false</param>
        /// <returns></returns>
        AudioChannel Play(AudioClip clip, Vector3 point, bool loop = false);

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="audioChannel"></param>
        void Stop(AudioChannel audioChannel);

        /// <summary>
        /// 暂停播放
        /// </summary>
        /// <param name="audioChannel"></param>
        void Pause(AudioChannel audioChannel);
        /// <summary>
        /// 恢复播放
        /// </summary>
        /// <param name="audioChannel"></param>
        void Resume(AudioChannel audioChannel);
        /// <summary>
        /// 停止所有的音频
        /// </summary>
        void StopAll();
    }

}
