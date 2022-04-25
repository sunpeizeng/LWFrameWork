using LWFramework.UI;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
public class TestChildNode : BaseUINode
{
    [UIElement("HeadImg")]
    private Image _HeadImg = null;
    [UIElement("NameText")]
    private Text _NameText = null;
    public string NameText {
        set {
            _NameText.text = value;
        }
    }
    public string HeadImgName { 
        set => _HeadImg.sprite = UIUtility.Instance.GetSprite(value);
    }
  

    public override void UnSpawn()
    {
        base.UnSpawn();
        LWDebug.Log("回收了！！！！");
    }
    public override void Release()
    {
        base.Release();
        LWDebug.Log("释放掉了！！！！");
    }
}
