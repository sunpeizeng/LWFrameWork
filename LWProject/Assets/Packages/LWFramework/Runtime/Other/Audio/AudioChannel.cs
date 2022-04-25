using LWFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LWFramework.Audio {
    public class AudioChannel : IPoolGameObject
    {
        protected GameObject m_Entity;
        private AudioSource m_Source;
        private Transform m_DefaultParent;
        private bool m_IsPause = false;
        public void Create(GameObject gameObject)
        {
            m_Entity = gameObject;
            m_Source = m_Entity.AddComponent<AudioSource>();
            m_DefaultParent = m_Entity.transform.parent;
            m_Source.playOnAwake = false;
        }

        public bool GetActive()
        {
            return m_Entity||m_Entity.activeInHierarchy;
        }
        public bool IsInScene() {
            return m_Entity;
        }
        public void Release()
        {
            if(m_Entity)
                GameObject.Destroy(m_Entity);
        }

        public void SetActive(bool active)
        {
            if (m_Entity)
                m_Entity.SetActive(active);
        }

        public void UnSpawn()
        {
            if (m_Entity) {
                SetActive(false);
                m_Entity.transform.position = m_DefaultParent.position;
                m_Entity.transform.parent = m_DefaultParent;
                m_Source.Stop();
               
            }
        }
        
        public AudioClip AudioClip
        {
            set => m_Source.clip = value;
        }
        public float Volume
        {
            set => m_Source.volume = value;
        }
        public float Pitch
        {
            set => m_Source.pitch = value;
        }
        public bool Loop
        {
            set => m_Source.loop = value;
        }
        public Transform Parent
        {
            set
            {
                m_Entity.transform.position = value.position;
                m_Entity.transform.parent = value;
            }
        }
        public Vector3 Posi
        {
            set
            {
                m_Entity.transform.position = value;
            }
        }
        public bool IsPlay
        {
            get {
                if (m_Source && m_Source.isPlaying)
                {
                    return true;
                }
                else {
                    return false;
                }
                
            } 
        }
        public bool IsPause
        {
            get {
                return m_IsPause;
            }
        }
        public void Play()
        {
            m_Source.Play();
            m_IsPause = false;
        }
        public void Pause()
        {
            m_Source.Pause();
            m_IsPause = true;
        }
        
    }
}

