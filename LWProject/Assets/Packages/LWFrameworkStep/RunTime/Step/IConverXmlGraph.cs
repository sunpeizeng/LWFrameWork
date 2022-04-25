using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
namespace LWFramework.Step
{
    public interface IConverXmlGraph
    {
        /// <summary>
        /// 转换成xml文件
        /// </summary>
        /// <returns></returns>
        XElement GetXml();
        /// <summary>
        /// 导入xml文件初始化
        /// </summary>
        /// <param name="xElement"></param>
        void SetXml(XElement xElement);
    }
}