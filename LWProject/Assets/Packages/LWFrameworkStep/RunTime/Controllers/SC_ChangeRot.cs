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
    /// 步骤控制器，处理旋转
    /// </summary>
    public class SC_ChangeRot : BaseStepObjectController
    {
        [LabelText("旋转时间"), LabelWidth(70)]
        public float m_RotTime = 1;
        [LabelText("变化角度"), LabelWidth(70), ListDrawerSettings(CustomAddFunction = "AddEulerValue", OnTitleBarGUI = ("SetValue"))]
        public Vector3[] m_EulerArray;
        private Transform m_Target;
        public override void Start()
        {
            m_Target = GameObjectContainer.Instance.FindStepObjByName(m_ObjName).transform; //StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
            m_Target.localEulerAngles = m_EulerArray[0];
        }

        public override void Stop()
        {
            m_Target.localEulerAngles = m_EulerArray[m_EulerArray.Length - 1];
        }

        public override void Execute()
        {
            //Quaternion oldQ;
            //Vector3 oldE;
            //oldE = m_Target.localEulerAngles;
            //m_Target.localEulerAngles = m_EulerArray[1];
            //oldQ = m_Target.rotation;
            //m_Target.localEulerAngles = oldE;

            //m_Target.DORotateQuaternion(oldQ, m_RotTime).SetEase(Ease.Linear).OnComplete(() =>
            //{
            //    m_ControllerCompleted?.Invoke();
            //});

            Sequence sequence = DOTween.Sequence();
            float changeTimeUnit = m_RotTime / (m_EulerArray.Length - 1);
            for (int i = 1; i < m_EulerArray.Length; i++)
            {
                Quaternion oldQ;
                Vector3 oldE;
                oldE = m_Target.localEulerAngles;
                m_Target.localEulerAngles = m_EulerArray[i];
                oldQ = m_Target.rotation;
                m_Target.localEulerAngles = oldE;

                sequence.Append(m_Target.DORotateQuaternion(oldQ, changeTimeUnit).SetEase(Ease.Linear));
                //  sequence.Append(m_Target.DOScale(m_ScaleArray[i], changeTimeUnit).SetEase(Ease.Linear));
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
                    m_EulerArray[m_EulerArray.Length - 1] = m_Target.localEulerAngles;
                }

            }
        }
        public void AddEulerValue()
        {
            GameObject go = GameObjectContainer.Instance.FindStepObjByName(m_ObjName);
            Array.Resize<Vector3>(ref m_EulerArray, m_EulerArray.Length + 1);
            if (go != null) {
                m_Target = go.transform;          
                m_EulerArray[m_EulerArray.Length - 1] = m_Target.localEulerAngles;
            }
            else
            {
                m_EulerArray[m_EulerArray.Length - 1] = Vector3.zero;
            }

        }
#endif

        public override XElement GetXml()
        {
            XElement control = new XElement("Control");
            control.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
            control.Add(new XAttribute("ObjectName", $"{m_ObjName}"));
            control.Add(new XAttribute("RotTime", $"{m_RotTime}"));

            XElement datas = new XElement("Datas");
            control.Add(datas);
            for (int i = 0; i < m_EulerArray.Length; i++)
            {

                XElement posi = VectorUtil.Vector3ToXml("Euler", m_EulerArray[i]);
                datas.Add(posi);
            }
            control.Add(new XAttribute("Remark", $"{m_Remark}"));
            return control;
        }
        public override void SetXml(XElement xElement)
        {
            m_ObjName = xElement.Attribute("ObjectName").Value;
            m_RotTime = float.Parse(xElement.Attribute("RotTime").Value);
            List<XElement> dataList = xElement.Element("Datas").Elements("Euler").ToList();
            m_EulerArray = new Vector3[dataList.Count];
            for (int i = 0; i < dataList.Count; i++)
            {
                m_EulerArray[i] = VectorUtil.XmlToVector3(dataList[i]);
            }
        }
    }
}