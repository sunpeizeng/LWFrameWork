using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Xml.Linq;
using System.Linq;
namespace LWFramework.Step
{
    /// <summary>
    /// 步骤控制器，处理位移，延迟位移
    /// </summary>
    public class SC_ChangePosiDeley : BaseStepObjectController
    {

        [LabelText("延迟时间"), LabelWidth(90)]
        public float m_TimeDeley = 0;
        [LabelText("移动时间"), LabelWidth(70)]
        public float m_MoveTime;
        [LabelText("移动位置"), LabelWidth(70)]
        public Vector3[] m_PosiArray;
        private Transform m_Target;

        public override void Start()
        {
            m_Target = GameObjectContainer.Instance.FindStepObjByName(m_ObjName).transform; //StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
            if (m_PosiArray.Length < 2)
            {
                LWDebug.LogError("当前节点的Controller的移动参数少于2个");
            }
            m_Target.localPosition = m_PosiArray[0];
        }

        public override void Stop()
        {
            m_Target.localPosition = m_PosiArray[m_PosiArray.Length - 1];
        }

        public override void Execute()
        {
            DoPath();

        }
        void DoPath()
        {
            m_Target.DOLocalPath(m_PosiArray, m_MoveTime).SetDelay(m_TimeDeley).SetEase(Ease.Linear).OnComplete(() =>
            {
                m_ControllerExecuteCompleted?.Invoke();
            });
        }

        [Button("设置数据"), LabelWidth(70)]
        public void SetValue()
        {
            m_Target = GameObjectContainer.Instance.FindStepObjByName(m_ObjName).transform;
            m_PosiArray[m_PosiArray.Length - 1] = m_Target.localPosition;
        }
        public override XElement GetXml()
        {
            XElement control = new XElement("Control");
            control.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
            control.Add(new XAttribute("ObjectName", $"{m_ObjName}"));
            control.Add(new XAttribute("TimeDeley", $"{m_TimeDeley}"));
            control.Add(new XAttribute("MoveTime", $"{m_MoveTime}"));

            XElement datas = new XElement("Datas");
            control.Add(datas);
            for (int i = 0; i < m_PosiArray.Length; i++)
            {

                XElement posi = VectorUtil.Vector3ToXml("Position", m_PosiArray[i]);
                datas.Add(posi);
            }
            control.Add(new XAttribute("Remark", $"{m_Remark}"));
            return control;
        }
        public override void SetXml(XElement xElement)
        {
            m_ObjName = xElement.Attribute("ObjectName").Value;
            m_TimeDeley = float.Parse(xElement.Attribute("TimeDeley").Value);
            m_MoveTime = float.Parse(xElement.Attribute("MoveTime").Value);
            List<XElement> dataList = xElement.Element("Datas").Elements("Position").ToList();
            m_PosiArray = new Vector3[dataList.Count];
            for (int i = 0; i < dataList.Count; i++)
            {
                m_PosiArray[i] = VectorUtil.XmlToVector3(dataList[i]);
            }
        }
    }
}