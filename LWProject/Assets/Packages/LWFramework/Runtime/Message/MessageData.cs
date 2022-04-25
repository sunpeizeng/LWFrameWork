using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LWFramework.Message {
    /// <summary>
    /// 消息数据
    /// </summary>
    public class MessageData
    {
        // public MessageData data;
        public GameObject sender;
        public float delay = 0;
        public string type;
        private Dictionary<string, object> _param;
        public MessageData()
        {
            _param = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
        }
        public T Get<T>(string name)
        {
            return (T)this[name];
        }
        public T Get<T>(int index)
        {
            List<object> data = _param.Values.ToList();
            return (T)data[index];
        }
        /// <summary>
        /// Find param
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                return _param != null && _param.ContainsKey(name) ? _param[name] : null;
            }
            set
            {
                if (_param != null)
                {
                    _param[name] = value;
                }
            }
        }

        public void Clear()
        {
            sender = null;
            delay = 0;
            type = "";
            _param.Clear();
        }

    }
}

