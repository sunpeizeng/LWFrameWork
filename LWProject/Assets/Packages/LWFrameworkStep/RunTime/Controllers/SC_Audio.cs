using Cysharp.Threading.Tasks;
using LWFramework.Audio;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
namespace LWFramework.Step {
    public class SC_Audio : BaseStepController
    {
    
        [LabelText("播放Tween的阶段"), LabelWidth(90)]
        public SC_State m_SC_State = SC_State.Execute;
        [LabelText("执行的音频路径")]
        public string m_AudioPath;
        [LabelText("延迟播放")]
        public float m_DeleyTime = 0;
        private AudioClip m_AudioClip;
        private AudioChannel m_AudioChannel;

       
        private CancellationTokenSource m_Cts;
        private float m_Time;
        private bool m_ExecuteCompleted = false;
        public override void Start()
        {
            m_ExecuteCompleted = false;
            m_Time = m_DeleyTime;
          

            if (m_AudioPath != null && m_AudioPath != "") {
                m_AudioClip = ManagerUtility.AssetsMgr.Load<AudioClip>(m_AudioPath);
            }
            if (m_SC_State == SC_State.Start) {
                m_AudioChannel = ManagerUtility.AudioMgr.Play(m_AudioClip);
            }

       
        }


        public override void Stop()
        {
            if (m_Cts != null)
            {
                m_Cts.Cancel();
                m_Cts.Dispose();
                m_Cts = null;
            }
            if (m_AudioChannel != null) {
                ManagerUtility.AudioMgr.Stop(m_AudioChannel);
                m_AudioChannel = null;
            }

        }

        public override void Execute()
        {
            m_Cts = new CancellationTokenSource();
            if (m_SC_State == SC_State.Execute)
            {
                WaitUpdate().Forget();
            }
            else {
                m_ControllerExecuteCompleted?.Invoke();
            }
           
        }
  
        /// <summary>
        //计算等待时间
        /// </summary>
        async UniTaskVoid WaitUpdate()
        {
            while (true && m_Cts != null)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: m_Cts.Token);               
                if (m_Time > 0) {
                    m_Time -= Time.deltaTime;
                }
                if (m_Time <= 0 && m_AudioChannel == null) {
                    m_AudioChannel = ManagerUtility.AudioMgr.Play(m_AudioClip);
                }
                if (m_AudioChannel!=null&&!m_AudioChannel.IsPlay &&!m_ExecuteCompleted)
                {
                    m_ControllerExecuteCompleted?.Invoke();
                    m_ExecuteCompleted = true;
                }
            }


        }
        public override XElement GetXml()
        {
            XElement control = base.GetXml();
            control.Add(new XAttribute("SC_State", $"{(int)m_SC_State}"));
            control.Add(new XAttribute("AudioPath", $"{m_AudioPath}"));
            control.Add(new XAttribute("DeleyTime", $"{m_DeleyTime}"));
            control.Add(new XAttribute("Remark", $"{m_Remark}"));

            return control;
        }
        public override void SetXml(XElement xElement)
        {
            m_SC_State = (SC_State)(int.Parse(xElement.Attribute("SC_State").Value));
            m_AudioPath = xElement.Attribute("AudioPath").Value;
           
            m_DeleyTime = float.Parse( xElement.Attribute("DeleyTime").Value);
            m_Remark = xElement.Attribute("Remark").Value;
        }
    }
    
}
