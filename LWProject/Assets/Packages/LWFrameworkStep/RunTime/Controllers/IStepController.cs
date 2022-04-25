using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step
{
    /// <summary>
    /// 步骤控制器，主要用于处理各种步骤中的变化效果
    /// </summary>
    public interface IStepController : IConverXmlGraph
    {
        /// <summary>
        /// 当前的Graph
        /// </summary>
        // IStepManager CurrStepManager { get; set; }
        /// <summary>
        /// 当前控制器执行完成的回调
        /// </summary>
        Action ControllerCompleted { get; set; }
        /// <summary>
        /// 控制器备注
        /// </summary>
        string Remark { get; }
        /// <summary>
        ///开始控制器
        /// </summary>
        void Start();
        /// <summary>
        /// 结束控制器
        /// </summary>
        void Stop();
        /// <summary>
        /// 执行控制器
        /// </summary>
        void Execute();

    }

    public enum SC_State
    {
        Start = 0, Execute = 1, Stop = 2
    }
}