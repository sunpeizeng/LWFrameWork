using System.Collections.Generic;

namespace LWFramework
{
    public abstract class BaseObjectPool<T>
    {
        public int PoolMaxSize { get; protected set; }

        public string m_Name;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_PoolMaxSize">线程池的大小，回收时如果池子大小已满，则多余的对象会被遗弃</param>
        public BaseObjectPool(int p_PoolMaxSize)
        {
            this.PoolMaxSize = p_PoolMaxSize;
        }
        public abstract T Spawn();
        public abstract void Unspawn (T p_Obj);
        public abstract void UnspawnAll();
        public abstract void Clear();
    };
 
}
