using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.Step
{
    /// <summary>
    /// 步骤控制器，主要用于处理各种步骤中的变化效果
    /// </summary>
    public abstract class BaseStepObjectController : BaseStepController
    {
        [LabelText("控制对象"), LabelWidth(60), HorizontalGroup]
        public string m_ObjName;
       

#if UNITY_EDITOR
        [Button("选中"), HorizontalGroup(50)]
        public virtual void ChooseObj()
        {
            UnityEditor.Selection.activeObject = GameObjectContainer.Instance.FindStepObjByName(m_ObjName);//StepRuntimeData.Instance.FindGameObject(m_ObjName);
                                                                                                           //UnityEditor.SceneView.FrameLastActiveSceneView();
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