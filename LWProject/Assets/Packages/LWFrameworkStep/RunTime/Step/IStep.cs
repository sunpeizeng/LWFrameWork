namespace LWFramework.Step
{
    /// <summary>
    /// 节点，控制Step的流程
    /// </summary>
    public interface IStep : IConverXmlGraph
    {
        IStepManager StepManager { get; set; }
        StepNodeState CurrState { get; set; }
        IStep PrevNode { get; set; }
        IStep NextNode { get; set; }
        int Num { get; set; }
        string UniqueId { get; set; }
        int NextIndex { get; }

        bool HaveTrigger { get; }
        /// <summary>
        /// 进入节点,启动所有控制器
        /// </summary>
        void StartControllerList(bool isBack = false);
        /// <summary>
        /// 退出节点，停止所有控制器
        /// </summary>
        void StopControllerList();
        /// <summary>
        /// 进入节点，启动所有触发器
        /// </summary>
        void StartTriggerList();
        /// <summary>
        /// 退出节点，停止触发器
        /// </summary>
        void StopTriggerList();
        /// <summary>
        /// 设置自身未当前运行的节点
        /// </summary>
        void SetSelfCurrent();
    }

    /// <summary>
    /// 步骤节点状态
    /// </summary>
    public enum StepNodeState
    {
        Wait,
        Execute,
        Complete
    }
}