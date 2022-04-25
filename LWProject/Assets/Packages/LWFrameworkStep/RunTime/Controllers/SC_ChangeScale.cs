using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Xml.Linq;
using System.Linq;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif
namespace LWFramework.Step
{
    /// <summary>
    /// 步骤控制器，处理位移
    /// </summary>
    public class SC_ChangeScale : BaseStepObjectController
    {

        [LabelText("变化时间"), LabelWidth(70)]
        public float m_ChangeTime;
        [LabelText("变化大小"), LabelWidth(70), ListDrawerSettings(CustomAddFunction = "AddScaleValue", OnTitleBarGUI = ("SetValue"))]
        public Vector3[] m_ScaleArray;


        private Transform m_Target;
        public override void Start()
        {
            m_Target = GameObjectContainer.Instance.FindStepObjByName(m_ObjName).transform; //StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;


            m_Target.localScale = m_ScaleArray[0];
        }

        public override void Stop()
        {
            m_Target.localScale = m_ScaleArray[m_ScaleArray.Length - 1];
        }

        public override void Execute()
        {
            Sequence sequence = DOTween.Sequence();
            float changeTimeUnit = m_ChangeTime / (m_ScaleArray.Length - 1);
            for (int i = 1; i < m_ScaleArray.Length; i++)
            {
                sequence.Append(m_Target.DOScale(m_ScaleArray[i], changeTimeUnit).SetEase(Ease.Linear));
            }
            sequence.OnComplete(() =>
            {
                m_ControllerExecuteCompleted?.Invoke();
            });


        }
#if UNITY_EDITOR

        public void SetValue()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                m_Target = GameObjectContainer.Instance.FindStepObjByName(m_ObjName).transform;
                if (m_Target != null)
                {
                    m_ScaleArray[m_ScaleArray.Length - 1] = m_Target.localScale;
                }

            }
        }
        public void AddScaleValue()
        {
            m_Target = GameObjectContainer.Instance.FindStepObjByName(m_ObjName).transform;
            Array.Resize<Vector3>(ref m_ScaleArray, m_ScaleArray.Length + 1);
            if (m_Target != null)
            {
                m_ScaleArray[m_ScaleArray.Length - 1] = m_Target.localScale;
            }
            else
            {
                m_ScaleArray[m_ScaleArray.Length - 1] = Vector3.one;
            }

        }
#endif
        public override XElement GetXml()
        {
            XElement control = new XElement("Control");
            control.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
            control.Add(new XAttribute("ObjectName", $"{m_ObjName}"));
            control.Add(new XAttribute("ChangeTime", $"{m_ChangeTime}"));

            XElement datas = new XElement("Datas");
            control.Add(datas);
            for (int i = 0; i < m_ScaleArray.Length; i++)
            {

                XElement posi = VectorUtil.Vector3ToXml("Scale", m_ScaleArray[i]);
                datas.Add(posi);
            }
            control.Add(new XAttribute("Remark", $"{m_Remark}"));
            return control;
        }
        public override void SetXml(XElement xElement)
        {
            m_ObjName = xElement.Attribute("ObjectName").Value;
            m_ChangeTime = float.Parse(xElement.Attribute("ChangeTime").Value);
            List<XElement> dataList = xElement.Element("Datas").Elements("Scale").ToList();
            m_ScaleArray = new Vector3[dataList.Count];
            for (int i = 0; i < dataList.Count; i++)
            {
                m_ScaleArray[i] = VectorUtil.XmlToVector3(dataList[i]);
            }
        }
    }
}