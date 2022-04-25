using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace LWFramework.Step
{
    public class ConverHelp : Singleton<ConverHelp>
    {
        private Assembly m_CurrAssembly;
        public Assembly CurrAssembly
        {
            get
            {
                if (m_CurrAssembly == null)
                {
                    m_CurrAssembly = Assembly.GetExecutingAssembly(); // 获取当前程序集
                }
                return m_CurrAssembly;
            }
        }
        private Assembly m_CallAssembly;
        public Assembly CallAssembly
        {
            get
            {              
                return m_CallAssembly;
            }
            set {
                m_CallAssembly = value;
            }
        }
        public T CreateInstance<T>(string scriptName)
        {
            object o = CurrAssembly.CreateInstance(scriptName);
            if (o == null) {
                o = CallAssembly.CreateInstance(scriptName);
            }
            return (T)o;
        }
    }
}