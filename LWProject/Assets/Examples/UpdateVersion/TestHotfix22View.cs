using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;

public class TestHotfix22View : BaseUINode 
{

	[UIElement("Button1")]
	private Button m_Button1 = null;
	public override  void Create(GameObject gameObject)
	{
		base.Create(gameObject);
		m_Button1.onClick.AddListener(() => 		{

		});

	}
	public override void UnSpawn()
	{
		base.UnSpawn();
	}
	public override void Release()
	{
		base.Release();
	}
}
