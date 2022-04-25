using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.UI {
    public interface IUILogic
    {
        /// <summary>
        /// 创建View  注意OnCreateView是在View中Create的base.Create调用的.所以函数只处理数据等,避免调用view
        /// </summary>
        void OnCreateView();
        /// <summary>
        /// 打开View
        /// </summary>
        void OnOpenView();
        /// <summary>
        /// 更新View
        /// </summary>
        void OnUpdateView();
        /// <summary>
        /// 关闭View
        /// </summary>
        /// <summary>
        /// 关闭View
        /// </summary>
        void OnCloseView();
        /// <summary>
        /// 清空View
        /// </summary>
        void OnClearView();

    }
}

