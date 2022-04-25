using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.UI
{
    public class DataBindingAttribute : Attribute
    {
        
        public object Key { get; private set; }
        public BindableEnum Bandable { get; private set; }
        /// <summary>
        /// ViewData的Key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bandable">数据类型,支持控件:(String=>InputField\Text)(Bool=>Toggle\Button)(Float=>InputField\Text\Slider\Scrollbar\Image)</param>
        public DataBindingAttribute(object key, BindableEnum bandable = BindableEnum.String)
        {
            Key = key;
            Bandable = bandable;
        }
    }

}
