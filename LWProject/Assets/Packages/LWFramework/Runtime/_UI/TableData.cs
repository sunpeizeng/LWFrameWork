using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.Core
{
    
    public class TableData 
    {           
        private Hashtable m_Data = new Hashtable();
        public Hashtable Data {
            get => m_Data;
        }
        /// <summary>
        /// 数据发生变化的处理
        /// </summary>
        public event Action<object> OnDataChange;
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="key">数据的key</param>
        /// <returns></returns>
        public T Get<T>(object key)
        {
            object obj = this[key];
            if (obj == null) {
                return default(T);
            }
            return (T)this[key];
        }
        /// <summary>
        /// 获取数据 外部转换
        /// </summary>
        /// <param name="key">数据的key</param>
        /// <returns></returns>
        public object Get(object key)
        {
            return this[key];
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[object key]
        {
            get
            {
                return m_Data != null && m_Data.ContainsKey(key) ? m_Data[key] : null;
            }
            set
            {
                if (m_Data != null)
                {
                    var oldValue = m_Data[key];
                    if (oldValue == value|| value.Equals(oldValue))
                        return;
                    m_Data[key] = value;
                    OnDataChange?.Invoke(key);
                }
            }
        }
        public void Remove(object key) {
            var value = this[key];
            if (value != null) {
                m_Data.Remove(key);
                value = null;
            }
           

        }
        /// <summary>
        /// 清空数据
        /// </summary>
        public void Clear() {
            m_Data.Clear();
        }
       
    }

}
