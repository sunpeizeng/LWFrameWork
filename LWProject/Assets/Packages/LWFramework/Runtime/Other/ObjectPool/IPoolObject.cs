using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework {
    public interface IPoolObject
    {

        /// <summary>
        /// 回收进对象池
        /// </summary>
        /// <returns>是否可以进行回收</returns>
        void UnSpawn();
        /// <summary>
        /// 释放掉，完全删除
        /// </summary>
        void Release();
    }
}

