using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step {
    public class SC_LoadObject : BaseStepController
    {
        [LabelText("文件夹路径")]
        public string m_DirPath;
        [LabelText("加载物体的数据"), ListDrawerSettings(CustomAddFunction = "CustomAddFunction")]
        public List<ObjectLoadData> m_LoadDataList;
        public override void Start()
        {
            for (int i = 0; i < m_LoadDataList.Count; i++)
            {
                string objName = OtherUtility.GetFileNameFromPath(m_LoadDataList[i].loadPath);
                GameObject go = GameObjectContainer.Instance.FindStepObjByName(objName);
                GameObjectContainer.Instance.DestroyStepObject(go);
            }
        }

        public override void Stop()
        {
            for (int i = 0; i < m_LoadDataList.Count; i++)
            {               
                string objName = OtherUtility.GetFileNameFromPath(m_LoadDataList[i].loadPath);//m_ObjectPathArray[i].Substring(startIndex, length);
                GameObject go = ManagerUtility.AssetsMgr.InstanceGameObject(m_DirPath + m_LoadDataList[i].loadPath);
                go.name = objName;
                go.transform.position = m_LoadDataList[i].posi;
                go.transform.eulerAngles = m_LoadDataList[i].euler;
                go.transform.localScale = m_LoadDataList[i].scale;
                GameObjectContainer.Instance.AddStepObject(go);
            }
        }

        public override void Execute()
        {
            m_ControllerExecuteCompleted?.Invoke();
        }
        public override XElement GetXml()
        {
            XElement control =  base.GetXml();
            control.Add(new XAttribute("DirPath", $"{m_DirPath}"));
            XElement datas = new XElement("Datas");
            
            for (int i = 0; i < m_LoadDataList.Count; i++)
            {
                XElement data = new XElement("Data");
                data.Add(new XAttribute("LoadPath", $"{m_LoadDataList[i].loadPath}"));
                XElement posi = VectorUtil.Vector3ToXml("Posi", m_LoadDataList[i].posi);
                XElement euler = VectorUtil.Vector3ToXml("Euler", m_LoadDataList[i].euler);
                XElement scale = VectorUtil.Vector3ToXml("Scale", m_LoadDataList[i].scale);
                data.Add(posi);
                data.Add(euler);
                data.Add(scale);
                datas.Add(data);
            }
            control.Add(datas);
            control.Add(new XAttribute("Remark", $"{m_Remark}"));
            return control;
        }
        public override void SetXml(XElement xElement)
        {
            m_Remark = xElement.Attribute("Remark").Value;
            m_DirPath = xElement.Attribute("DirPath").Value;
            List<XElement> loadDataList = xElement.Element("Datas").Elements("Data").ToList();
            m_LoadDataList = new List<ObjectLoadData>();
            for (int i = 0; i < loadDataList.Count; i++)
            {
                ObjectLoadData data = new ObjectLoadData
                {
                    loadPath = loadDataList[i].Attribute("LoadPath").Value,
                    posi = VectorUtil.XmlToVector3(loadDataList[i].Element("Posi")),
                    euler = VectorUtil.XmlToVector3(loadDataList[i].Element("Euler")),
                    scale = VectorUtil.XmlToVector3(loadDataList[i].Element("Scale"))

                };
                m_LoadDataList.Add(data);
            }
        }
#if UNITY_EDITOR
        void CustomAddFunction() {
            ObjectLoadData data = new ObjectLoadData();
            GameObject go = UnityEditor.Selection.activeGameObject;
            if (go != null)
            {
                data.loadPath = go.name + ".prefab";
                data.posi = go.transform.position;
                data.euler = go.transform.eulerAngles;
                data.scale = go.transform.localScale;
            }
            else {
                data.scale = Vector3.one;
            }
            
            m_LoadDataList.Add(data);
        }
#endif
    }
    public class ObjectLoadData {
        public string loadPath;
        public Vector3 posi;
        public Vector3 euler;
        public Vector3 scale;
    }
}
