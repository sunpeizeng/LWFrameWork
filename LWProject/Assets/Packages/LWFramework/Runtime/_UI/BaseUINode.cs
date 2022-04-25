using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LWFramework.UI
{
    public abstract class BaseUINode: IPoolGameObject
    {
        protected GameObject m_Entity;              
        /// <summary>
        /// 创建GameObject实体
        /// </summary>
        /// <param name="gameObject"></param>
        public virtual void Create(GameObject gameObject)
        {
            m_Entity = gameObject;
            //view上的组件
            UIUtility.Instance.SetViewElement(this, gameObject);
        }
        public virtual void UnSpawn() {          
            SetActive(false);
            m_Entity.transform.SetAsLastSibling();
        }
        public bool IsInScene()
        {
            return m_Entity;
        }
        //public abstract void OnUnSpawn();
        /// <summary>
        /// 释放引用，删除gameobject
        /// </summary>
        public virtual void Release()
        {
            GameObject.Destroy(m_Entity);
        }

        public bool GetActive()
        {
            return m_Entity.activeInHierarchy;
        }

        public void SetActive(bool active)
        {
            m_Entity.SetActive(active);
        }
        public virtual void ResetNode() { 
        
        }
    }
}