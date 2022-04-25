
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework {
    public class GameObjectPool<T> : ObjectPool<T> where T : class, IPoolGameObject,new()
    {
        private GameObject m_Template;
        public GameObjectPool(int poolMaxSize) : base(poolMaxSize)
        {

        }
        public GameObjectPool(int p_PooMaxSize, GameObject p_Template) : base(p_PooMaxSize)
        {
            m_Template = p_Template;
            m_Template.SetActive(false);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        public override T Spawn()
        {
            T ret;
            if (m_PoolList.Count > 0)
            {
                ret = m_PoolList[0];
                m_PoolList.RemoveAt(0);
                //处理一些跨场景等原因 对象池中的对象丢失的清空
                if (!ret.IsInScene()) {
                    ret = (T)Activator.CreateInstance(typeof(T));
                    GameObject go = GameObject.Instantiate(m_Template, m_Template.transform.parent, false);
                    ret.Create(go);
                }
            }
            else {
                ret = (T)Activator.CreateInstance(typeof(T));
                GameObject go = GameObject.Instantiate(m_Template, m_Template.transform.parent, false);
                ret.Create(go);
               
            }
            m_UseList.Add(ret);
            ret.SetActive(true);
            return ret;
        }
        public override void Unspawn(T obj)
        {
            base.Unspawn(obj);
            obj.SetActive (false);
        }
    }

}
