using Cysharp.Threading.Tasks;
using LWFramework.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step
{
    public class ST_MouseDown : BaseStepObjectTrigger
    {

        CancellationTokenSource cts;
        public override void Start()
        {
            base.Start();
            GameObject gameObject = GameObjectContainer.Instance.FindStepObjByName(m_ObjName); //StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;

            ManagerUtility.HLMgr.AddFlashingHighlighting(gameObject, new Color[] { new Color(0, 0.17f, 1, 1), new Color(0, 0.96f, 0.99f, 1) });
            cts = new CancellationTokenSource();
            _ = WaitUpdate();
        }
        /// <summary>
        //使用Task处理射线点击
        /// </summary>
        async UniTaskVoid WaitUpdate()
        {
            while (true && !m_IsTrigger && cts != null)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: cts.Token);
                if (Input.GetMouseButtonDown(0))
                {
                    //从摄像机发出射线的点
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(ray, 30);
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].collider.gameObject == GameObjectContainer.Instance.FindStepObjByName(m_ObjName))
                        {
                            Finished();
                            break;
                        }
                    }
                }
            }


        }
        public override void Stop()
        {
            base.Stop();
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
                MainManager.Instance.GetManager<IHighlightingManager>().RemoveHighlighting(GameObjectContainer.Instance.FindStepObjByName(m_ObjName));
                cts = null;
            }

        }

        public override XElement GetXml()
        {
            XElement trigger = new XElement("Trigger");
            trigger.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
            trigger.Add(new XAttribute("ObjectName", $"{m_ObjName}"));
            trigger.Add(new XAttribute("ResultIndex", $"{m_NextNum}"));
            return trigger;
        }
        public override void SetXml(XElement xElement)
        {
            m_ObjName = xElement.Attribute("ObjectName").Value;
            m_NextNum = int.Parse(xElement.Attribute("ResultIndex").Value);
        }
    }
}