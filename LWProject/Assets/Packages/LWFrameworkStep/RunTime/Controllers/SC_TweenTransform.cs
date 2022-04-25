using Cysharp.Threading.Tasks;
using DG.Tweening;
using LWFramework.Message;
using LWFramework.Step;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step
{
    public class SC_TweenTransform : BaseStepObjectController
    {
        [LabelText("播放时间"), LabelWidth(90)]
        public float m_TweenTime;
        [LabelText("循环时间"), LabelWidth(90)]
        public int m_LoopCount;
        [LabelText("循环类型"), LabelWidth(90)]
        public LoopType m_LoopType;
        [LabelText("播放Tween的阶段"), LabelWidth(90)]
        public SC_State m_SC_State = SC_State.Execute;
        [LabelText("开始的Transform信息,X=10000忽略"), LabelWidth(90)]
        public Vector3[] m_Transform;
        private GameObject m_Target;
        public override void Start()
        {
            m_Target = GameObjectContainer.Instance.FindStepObjByName(m_ObjName); //StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
            if (m_SC_State == SC_State.Start) {
                PlayTween();
            }
        }
        public override void Stop()
        {
           
            m_Target.transform.DOKill();
        }      
        public override void Execute()
        {
            m_ControllerExecuteCompleted?.Invoke();
            if (m_SC_State == SC_State.Execute)
            {
                PlayTween();
            }
        }
        void PlayTween() {
            for (int i = 0; m_Transform!=null && i < m_Transform.Length; i++)
            {
                if (i == 0 && m_Transform[i].x!=10000)
                {
                    m_Target.transform.DOLocalMove(m_Transform[i], m_TweenTime).SetLoops(m_LoopCount, m_LoopType);
                }
                else if (i == 1 && m_Transform[i].x != 10000)
                {
                    m_Target.transform.DOLocalRotate(m_Transform[i], m_TweenTime).SetLoops(m_LoopCount, m_LoopType);
                }
                else if (i == 2 && m_Transform[i].x != 10000)
                {
                    m_Target.transform.DOLocalRotate(m_Transform[i], m_TweenTime).SetLoops(m_LoopCount, m_LoopType);
                }
            }
        }

        public override XElement GetXml()
        {
            XElement control = base.GetXml();
            control.Add(new XAttribute("ObjectName", $"{m_ObjName}"));
            control.Add(new XAttribute("TweenTime", $"{m_TweenTime}"));
            control.Add(new XAttribute("LoopCount", $"{m_LoopCount}"));
            control.Add(new XAttribute("LoopType", $"{(int)m_LoopType}"));
            control.Add(new XAttribute("SC_State", $"{(int)m_SC_State}"));

            XElement datas = new XElement("Datas");
            control.Add(datas);
            for (int i = 0; i < m_Transform.Length; i++)
            {
                XElement Vvector3Xml = VectorUtil.Vector3ToXml("Vector3", m_Transform[i]);
                datas.Add(Vvector3Xml);
            }
           
            control.Add(new XAttribute("Remark", $"{m_Remark}"));
            return control;
        }
        public override void SetXml(XElement xElement)
        {
            m_ObjName = xElement.Attribute("ObjectName").Value;
            m_TweenTime = float.Parse(xElement.Attribute("TweenTime").Value);
            m_LoopCount = int.Parse(xElement.Attribute("LoopCount").Value);
            m_LoopType = (LoopType) int.Parse(xElement.Attribute("LoopType").Value);
            m_SC_State = (SC_State)(int.Parse(xElement.Attribute("SC_State").Value));
            List<XElement> dataList = xElement.Element("Datas").Elements("Vector3").ToList();
            m_Transform = new Vector3[dataList.Count];
            for (int i = 0; i < dataList.Count; i++)
            {
                m_Transform[i] = VectorUtil.XmlToVector3(dataList[i]);
            }


            
        }
    }

}