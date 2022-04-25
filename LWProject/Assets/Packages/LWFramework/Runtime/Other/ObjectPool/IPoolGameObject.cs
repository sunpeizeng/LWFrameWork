using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework {
    public interface IPoolGameObject: IPoolObject
    {
        /// <summary>
        /// 是否为Active
        /// </summary>
        bool GetActive();
        /// <summary>
        /// 是否存在场景中
        /// </summary>
        /// <returns>是否存在场景中</returns>
        bool IsInScene();
        void SetActive(bool active);
        void Create(GameObject gameObject);
      
    }
}

