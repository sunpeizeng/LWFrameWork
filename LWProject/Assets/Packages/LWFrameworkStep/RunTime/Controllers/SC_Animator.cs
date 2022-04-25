using Cysharp.Threading.Tasks;
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
    public class SC_Animator : BaseStepObjectController
    {
        [LabelText("播放的百分百进度")]
        public float m_AnimaProgress;
        [LabelText("播放的动画片段数据")]
        public AnimatorStepData[] m_AnimatorStepDataArray;
        private GameObject m_Object;
        private Animator m_Animator;
        private CancellationTokenSource m_Cts;
        private bool m_ExecuteCompleted = false;
        public override void Start()
        {
            m_Object = GameObjectContainer.Instance.FindStepObjByName(m_ObjName);
            if (!m_Object.activeInHierarchy)
            {
                m_Object.SetActive(true);
            }
            m_Animator = m_Object.GetComponent<Animator>();
            m_ExecuteCompleted = false;
            for (int i = 0; i < m_AnimatorStepDataArray.Length; i++)
            {

            }
            PlayAni(SC_State.Start);
           // m_Animator.Play(m_AnimatorStepDataArray[0].aniName, 0, m_AnimatorStepDataArray[0].aniProgress);
        }
        bool PlayAni(SC_State sc_State) {
            for (int i = 0; i < m_AnimatorStepDataArray.Length; i++)
            {
                AnimatorStepData data = m_AnimatorStepDataArray[i];
                if (data.playState == sc_State) {
                    m_Animator.Play(data.aniName, 0, data.aniProgress);
                    return true;
                }
            }
            return false;
        }

        public override void Stop()
        {
            if (m_Cts != null)
            {
                m_Cts.Cancel();
                m_Cts.Dispose();
                m_Cts = null;
            }
            PlayAni(SC_State.Stop);
           // m_Animator.Play(m_AnimatorStepDataArray[m_AnimatorStepDataArray.Length - 1].aniName, 0, m_AnimatorStepDataArray[m_AnimatorStepDataArray.Length - 1].aniProgress);
        }

        public override void Execute()
        {
          
            PlayAni(SC_State.Execute); 
            m_Cts = new CancellationTokenSource();
            WaitUpdate().Forget();
           
            //   m_Animator.Play(m_AnimatorStepDataArray[m_AnimatorStepDataArray.Length - 1].aniName, 0, m_AnimatorStepDataArray[m_AnimatorStepDataArray.Length - 1].aniProgress);
           
        }
        /// <summary>
        //计算等待时间
        /// </summary>
        async UniTaskVoid WaitUpdate()
        {
            while (true && m_Cts != null)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: m_Cts.Token);
               
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > m_AnimaProgress && !m_ExecuteCompleted)
                {
                    m_ControllerExecuteCompleted?.Invoke();
                    m_ExecuteCompleted = true;
                }
            }


        }
        public override XElement GetXml()
        {
            XElement control = new XElement("Control");
            control.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
            control.Add(new XAttribute("ObjectName", $"{m_ObjName}"));
            control.Add(new XAttribute("AnimaProgress", $"{m_AnimaProgress}"));
            XElement datas = new XElement("Datas");
            for (int i = 0; i < m_AnimatorStepDataArray.Length; i++)
            {
                XElement data = new XElement("Data");
                data.Add(new XAttribute("AniName", $"{m_AnimatorStepDataArray[i].aniName}"));
                data.Add(new XAttribute("AniProgress", $"{m_AnimatorStepDataArray[i].aniProgress}"));
                data.Add(new XAttribute("PlayState", $"{(int)m_AnimatorStepDataArray[i].playState}"));
                datas.Add(data);
            }
            control.Add(new XAttribute("Remark", $"{m_Remark}"));
            control.Add(datas);

            return control;
        }
        public override void SetXml(XElement xElement)
        {
            m_ObjName = xElement.Attribute("ObjectName").Value;
            m_Remark = xElement.Attribute("Remark").Value;
            m_AnimaProgress = float.Parse(xElement.Attribute("AnimaProgress").Value) ;
            List<XElement> dataList = xElement.Element("Datas").Elements("Data").ToList();
            m_AnimatorStepDataArray = new AnimatorStepData[dataList.Count];
            for (int i = 0; i < dataList.Count; i++)
            {
                AnimatorStepData data = new AnimatorStepData
                {
                    aniName = dataList[i].Attribute("AniName").Value,
                    aniProgress = float.Parse(dataList[i].Attribute("AniProgress").Value),
                    playState = (SC_State)(int.Parse(dataList[i].Attribute("PlayState").Value))
                };
                m_AnimatorStepDataArray[i] = data;
            }
        }
    }
    public class AnimatorStepData
    {
        [HorizontalGroup(LabelWidth = 100)]
        public string aniName;
        [HorizontalGroup(LabelWidth = 100)]
        public float aniProgress;
        [HorizontalGroup(LabelWidth = 20)]
        public SC_State playState = SC_State.Start;
    }
}
