using System;
namespace LWFramework.UI
{
    public class UIElementAttribute : Attribute
    {
        public readonly string m_RootPath;
        public readonly string m_ResPath;
        /// <summary>
        /// 界面对象
        /// </summary>
        /// <param name="p_RootPath">查找的路径</param>
        /// <param name="p_ResPath">默认的资源主要是图片</param>
        public UIElementAttribute(string p_RootPath, string p_ResPath = "")
        {
            this.m_RootPath = p_RootPath;
            this.m_ResPath = p_ResPath;
        }
    }

}
