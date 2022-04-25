
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.Step
{
    [Serializable]
    public class BaseStepObjectTrigger : BaseStepTrigger
    {
        [LabelText("触发对象"), LabelWidth(70), HorizontalGroup]
        public string m_ObjName;
        //[LabelText("触发对象"), LabelWidth(70), ValueDropdown("GetSceneObjectList"), HorizontalGroup]
        //public string m_ObjName;
        //public List<string> GetSceneObjectList()
        //{
            
        //    //if (CurrNode == null)
        //    //{
        //    //    return null;
        //    //}
        //    //DataNode dataNode = (DataNode)CurrNode.graph.nodes.Find((XNode.Node node) =>
        //    //{
        //    //    return node is DataNode;
        //    //});
        //    //return dataNode.m_SceneObjectNameList;
        //    // return StepRuntimeData.Instance.SceneObjectNameList;
        //    return null;
        //}
#if UNITY_EDITOR
        [Button("选中"),  HorizontalGroup(50)]
        public void ChooseObj()
        {
            UnityEditor.Selection.activeObject = GameObjectContainer.Instance.FindStepObjByName(m_ObjName);
            GameObject chooseObj = UnityEditor.Selection.activeObject as GameObject;
            FocusPosition(chooseObj.transform.position);
        }
        void FocusPosition(Vector3 pos)
        {
            UnityEditor.SceneView.lastActiveSceneView.Frame(new Bounds(pos, Vector3.one), false);
        }
#endif
    }
}