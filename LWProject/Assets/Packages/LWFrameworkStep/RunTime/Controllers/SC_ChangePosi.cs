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
    public class SC_ChangePosi : BaseStepObjectController
    {
        [LabelText("移动时间"), LabelWidth(70)]
        public float m_MoveTime = 1;
        [LabelText("移动位置"), LabelWidth(70), ListDrawerSettings(CustomAddFunction = "AddPosiValue", OnTitleBarGUI = ("SetValue"))]
        public Vector3[] m_PosiArray;

        private GameObject m_Target;
        public override void Start()
        {
            m_Target = GameObjectContainer.Instance.FindStepObjByName(m_ObjName); //StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
            if (m_Target == null)
            {
                return;
            }

            if (m_PosiArray.Length < 2)
            {
                LWDebug.LogError("当前节点的Controller的移动参数少于2个");
            }
            m_Target.transform.localPosition = m_PosiArray[0];
        }

        public override void Stop()
        {
            if (m_Target == null)
            {
                return;
            }
            m_Target.transform.localPosition = m_PosiArray[m_PosiArray.Length - 1];
        }

        public override void Execute()
        {
            if (m_Target == null)
            {
                m_ControllerExecuteCompleted?.Invoke();
                return;
            }
            m_Target.transform.DOLocalPath(m_PosiArray, m_MoveTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                m_ControllerExecuteCompleted?.Invoke();
            });
        }
        public override XElement GetXml()
        {
            XElement control = new XElement("Control");
            control.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
            control.Add(new XAttribute("ObjectName", $"{m_ObjName}"));
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
            m_MoveTime = float.Parse(xElement.Attribute("MoveTime").Value);
            m_Remark = xElement.Attribute("Remark").Value;
            List<XElement> posiList = xElement.Element("Datas").Elements("Position").ToList();
            m_PosiArray = new Vector3[posiList.Count];
            for (int i = 0; i < posiList.Count; i++)
            {
                m_PosiArray[i] = VectorUtil.XmlToVector3(posiList[i]);
            }
        }



#if UNITY_EDITOR && !CREATEDLL

        public void SetValue()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {

                m_Target = GameObjectContainer.Instance.FindStepObjByName(m_ObjName);
                if (m_Target != null)
                {
                    m_PosiArray[m_PosiArray.Length - 1] = m_Target.transform.localPosition;
                }

            }

        }
        public void AddPosiValue()
        {

            m_Target = GameObjectContainer.Instance.FindStepObjByName(m_ObjName);
            Array.Resize<Vector3>(ref m_PosiArray, m_PosiArray.Length + 1);
            if (m_Target != null)
            {
                m_PosiArray[m_PosiArray.Length - 1] = m_Target.transform.localPosition;
            }
            else
            {
                m_PosiArray[m_PosiArray.Length - 1] = Vector3.zero;
            }

        }
        public override void ChooseObj()
        {
            base.ChooseObj();
            //GameObjectContainer.Instance.GizmosVector3Array = m_PosiArray;
        }
#endif
    }
}