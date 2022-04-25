using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
namespace LWFramework.Step
{
    public class VectorUtil
    {
        public static Vector3 XmlToVector3(XElement xElement)
        {
            Vector3 ret = Vector3.zero;

            if (xElement != null)
            {

                ret.x = float.Parse(xElement.Attribute("x").Value);
                ret.y = float.Parse(xElement.Attribute("y").Value);
                ret.z = float.Parse(xElement.Attribute("z").Value);
            }
            return ret;
        }

        public static XElement Vector3ToXml(string xmlName, Vector3 vector3)
        {
            XElement xElement = new XElement(xmlName);
            xElement.SetAttributeValue("x", vector3.x);
            xElement.SetAttributeValue("y", vector3.y);
            xElement.SetAttributeValue("z", vector3.z);
            return xElement;
        }
    }
}