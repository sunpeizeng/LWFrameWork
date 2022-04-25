using Cysharp.Threading.Tasks;
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
    public class SC_ChangeTransform : BaseStepObjectController
    {
        [LabelText("Transform改变的方式"), LabelWidth(90)]
        public ChangeTransformType m_ChangeTransformType;
        [LabelText("开始的Transform信息,X=10000忽略"), LabelWidth(90)]
        public Vector3[] m_StartTransform;
        [LabelText("结束的Transform信息,X=10000忽略"), LabelWidth(90)]
        public Vector3[] m_StopTransform;
        private GameObject m_Target;
        public override void Start()
        {
            m_Target = GameObjectContainer.Instance.FindStepObjByName(m_ObjName); //StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
            for (int i = 0; m_StartTransform!=null &&i < m_StartTransform.Length; i++)
            {
                if (i == 0 && m_StartTransform[i].x != 10000) {
                    m_Target.transform.localPosition = m_StartTransform[0];
                }
                else if (i == 1 && m_StartTransform[i].x != 10000)
                {
                    m_Target.transform.localEulerAngles = m_StartTransform[1];
                }
                else if (i == 2 && m_StartTransform[i].x != 10000)
                {
                    m_Target.transform.localScale = m_StartTransform[2];
                }
            }
        }
        public override void Stop()
        {
            if (m_ChangeTransformType == ChangeTransformType.Change)
            {
                for (int i = 0; m_StopTransform != null && i < m_StopTransform.Length; i++)
                {
                    if (i == 0 && m_StopTransform[i].x != 10000)
                    {
                        m_Target.transform.localPosition = m_StopTransform[0];
                    }
                    else if (i == 1 && m_StopTransform[i].x != 10000)
                    {
                        m_Target.transform.localEulerAngles = m_StopTransform[1];
                    }
                    else if (i == 2 && m_StopTransform[i].x != 10000)
                    {
                        m_Target.transform.localScale = m_StopTransform[2];
                    }
                }
            }
            else {
                for (int i = 0; m_StopTransform != null && i < m_StopTransform.Length; i++)
                {
                    if (i == 0 && m_StopTransform[i].x != 10000)
                    {
                        m_Target.transform.localPosition += m_StopTransform[0];
                    }
                    else if (i == 1 && m_StopTransform[i].x != 10000)
                    {
                        m_Target.transform.localEulerAngles += m_StopTransform[1];
                    }
                    else if (i == 2 && m_StopTransform[i].x != 10000)
                    {
                        m_Target.transform.localScale += m_StopTransform[2];
                    }
                }
            }
            
        }
        
        public override void Execute()
        {
            m_ControllerExecuteCompleted?.Invoke();
        }

       

        public override XElement GetXml()
        {
            XElement control = base.GetXml();
            control.Add(new XAttribute("ObjectName", $"{m_ObjName}"));
            control.Add(new XAttribute("ChangeTransformType", $"{(int)m_ChangeTransformType}"));

            XElement datas = new XElement("Datas");
            control.Add(datas);
            XElement data = new XElement("StartTransform");
            for (int i = 0; i < m_StartTransform.Length; i++)
            {
                
                XElement Vvector3Xml = VectorUtil.Vector3ToXml("Vector3", m_StartTransform[i]);
                data.Add(Vvector3Xml);

            }
            datas.Add(data);
            XElement data2 = new XElement("StopTransform");
            for (int i = 0; i < m_StopTransform.Length; i++)
            {
               
                XElement Vvector3Xml = VectorUtil.Vector3ToXml("Vector3", m_StopTransform[i]);
                data2.Add(Vvector3Xml);

            }
            datas.Add(data2);
            control.Add(new XAttribute("Remark", $"{m_Remark}"));
            return control;
        }
        public override void SetXml(XElement xElement)
        {
            m_ObjName = xElement.Attribute("ObjectName").Value;
            m_ChangeTransformType = (ChangeTransformType)(int.Parse(xElement.Attribute("ChangeTransformType").Value));
            List<XElement> dataList = xElement.Element("Datas").Element("StartTransform").Elements("Vector3").ToList();
            m_StartTransform = new Vector3[dataList.Count];
            for (int i = 0; i < dataList.Count; i++)
            {
                m_StartTransform[i] = VectorUtil.XmlToVector3(dataList[i]);
            }


            List<XElement> dataList2 = xElement.Element("Datas").Element("StopTransform").Elements("Vector3").ToList();
            m_StopTransform = new Vector3[dataList2.Count];
            for (int i = 0; i < dataList2.Count; i++)
            {
                m_StopTransform[i] = VectorUtil.XmlToVector3(dataList2[i]);
            }
        }
    }
    public enum ChangeTransformType { 
        Change=0,Add=1
    }
}