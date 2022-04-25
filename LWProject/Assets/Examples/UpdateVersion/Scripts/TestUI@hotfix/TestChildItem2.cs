using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;

//[UIViewData("TestChildItem2", LWFramework.UI.UILayer.local)]
[UIViewData("",FindType.Name,"")]
public class TestChildItem2 : LWFramework.UI.BaseUIView  
{

	[UIElement("HeadImg", "Assets/@Resources/Sprites/log3.png")]
    private Image _HeadImg;
	[UIElement("NameText")]
    private Text _NameText;
	public override  void CreateView(GameObject gameObject)
	{
		base.CreateView(gameObject);
	}
}
