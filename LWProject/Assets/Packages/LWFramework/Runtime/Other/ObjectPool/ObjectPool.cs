using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework
{
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> : BaseObjectPool<T> where T : class, IPoolObject,new()
    {
        protected List<T> m_PoolList = new List<T>();
        protected List<T> m_UseList = new List<T>();
        public int CurrentSize
        {
            get
            {
                return m_PoolList.Count;
            }
        }

        public ObjectPool(int p_PoolMaxSize) : base(p_PoolMaxSize)
        {
        }

        /// <summary>
        /// 改变对象池大小
        /// </summary>
        /// <param name="p_PoolMaxSize"></param>
        public void ChangeSize(int p_PoolMaxSize)
        {
            this.PoolMaxSize = p_PoolMaxSize;
            if (m_PoolList.Count > p_PoolMaxSize)
            {
                for (int i = m_PoolList.Count - 1; i >= p_PoolMaxSize; i--)
                {
                    m_PoolList[i].Release();
                    m_PoolList.RemoveAt(i);
                }
            }
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
                var ins = m_PoolList[0];
                m_PoolList.RemoveAt(0);
                ret = ins;
            }
            ret = new T();
            m_UseList.Add(ret);
            return ret;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="p_Obj"></param>
        public override void Unspawn(T p_Obj)
        {
            if (m_PoolList.Count < PoolMaxSize)
            {
                p_Obj.UnSpawn();
                m_PoolList.Add(p_Obj);
               
            }
            else
            {
                p_Obj.Release();       
            }
            m_UseList.Remove(p_Obj);
        }

        /// <summary>
        /// 是否在对象池中
        /// </summary>
        /// <param name="p_Obj"></param>
        /// <returns></returns>
        public bool IsInPool(T p_Obj)
        {
            return m_PoolList.IndexOf(p_Obj) > -1 ? true : false;
        }
        /// <summary>
        /// 回收所有的对象
        /// </summary>
        public override void UnspawnAll()
        {
            for (int i = 0; i < m_UseList.Count; )
            {
                Unspawn(m_UseList[i]);
            }
        }
        /// <summary>
        /// 清空对象池
        /// </summary>
        public override void Clear()
        {
            foreach (var obj in m_PoolList)
            {
                obj.Release();
            }
            m_PoolList.Clear();
            m_UseList.Clear();
        }
    }
}
   