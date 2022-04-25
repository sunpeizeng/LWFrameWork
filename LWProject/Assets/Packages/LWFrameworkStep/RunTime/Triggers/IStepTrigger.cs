using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step
{
    /// <summary>
    /// 步骤触发器，主要用于检测用户的操作，得出对应的结果
    /// </summary>
    public interface IStepTrigger : IConverXmlGraph
    {
        /// <summary>
        ///  控制器执行完成
        /// </summary>
        Action<int> TiggerCompleted { get; set; }
        /// <summary>
        /// 当前的StepManager
        /// </summary>
        IStepManager CurrStepManager { get; set; }
        /// <summary>
        /// 开始触发器
        /// </summary>
        void Start();
        /// <summary>
        /// 结束触发器
        /// </summary>
        void Stop();
        /// <summary>
        /// 触发器完成
        /// </summary>
        void Finished();
    }
}