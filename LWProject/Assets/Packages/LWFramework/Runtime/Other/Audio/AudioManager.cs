using LWFramework;
using LWFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.Audio {
    /// <summary>
    /// 音频管理器
    /// </summary>
    public class AudioManager : IAudioManager, IManager
    {
        private GameObject m_ManagerEntity;
        private GameObjectPool<AudioChannel> m_Pool;
        private List<AudioChannel> m_PlayList;
        public void Init()
        {
            m_ManagerEntity = new GameObject("AudioManager");
            GameObject audioChannelTemp = new GameObject("AudioChannel");
            audioChannelTemp.transform.parent = m_ManagerEntity.transform;

            m_Pool = new GameObjectPool<AudioChannel>(10, audioChannelTemp);
            m_PlayList = new List<AudioChannel>();
            GameObject.DontDestroyOnLoad(m_ManagerEntity);
        }
        public void Update()
        {
            for (int i = 0; i < m_PlayList.Count; i++)
            {
                //如果没有暂停而停止播放的，则回收
                if (!m_PlayList[i].IsPlay && !m_PlayList[i].IsPause)
                {
                    RemoveChannel(m_PlayList[i]);
                    i--;
                }
            }
        }
        AudioChannel CreateChannel()
        {
            AudioChannel audioChannel = m_Pool.Spawn();
            m_PlayList.Add(audioChannel);
            return audioChannel;
        }
        void RemoveChannel(AudioChannel audioChannel)
        {
            m_Pool.Unspawn(audioChannel);
            if (m_PlayList.Contains(audioChannel))
            {
                m_PlayList.Remove(audioChannel);
            }
        }
        public AudioChannel Play(AudioClip clip, bool loop = false)
        {
            AudioChannel audioChannel = CreateChannel();
            audioChannel.AudioClip = clip;
            audioChannel.Loop = loop;
            audioChannel.Play();
            return audioChannel;
        }
        /// <param name="clip"></param>
        /// <param name="emitter"></param>
        /// <returns></returns>
        public AudioChannel Play(AudioClip clip, Transform emitter, bool loop = false)
        {
            //Create an empty game object
            AudioChannel audioChannel = CreateChannel();
            audioChannel.AudioClip = clip;
            audioChannel.Parent = emitter;
            audioChannel.Loop = loop;
            audioChannel.Play();
            return audioChannel;
        }


        /// <param name="clip"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public AudioChannel Play(AudioClip clip, Vector3 point, bool loop = false)
        {
            AudioChannel audioChannel = CreateChannel();
            audioChannel.AudioClip = clip;
            audioChannel.Posi = point;
            audioChannel.Loop = loop;
            audioChannel.Play();
            return audioChannel;
        }
        public void Stop(AudioChannel audioChannel)
        {
            if (audioChannel != null) {
                RemoveChannel(audioChannel);
            }
            
        }
        public void StopAll()
        {
            for (int i = 0; i < m_PlayList.Count; i++)
            {
                RemoveChannel(m_PlayList[i]);
                i--;
            }
        }

        public void Pause(AudioChannel audioChannel)
        {
            if (audioChannel != null)
            {
                audioChannel.Pause();
            }
        }

        public void Resume(AudioChannel audioChannel)
        {
            if (audioChannel != null)
            {
                audioChannel.Play();
            }
        }
    }

}
