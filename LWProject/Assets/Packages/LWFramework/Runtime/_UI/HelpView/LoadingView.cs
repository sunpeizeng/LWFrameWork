using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;

[UIViewData("",FindType.Name, "LWFramework/Canvas/Top")]
public class LoadingView : BaseUIView 
{
    private int m_LoadCount;

    public int LoadCount {
        get => m_LoadCount;
        set => m_LoadCount = value;
    }
	[UIElement("RimgAni")]
	private Transform m_RimgAni = null;
	public override  void CreateView(GameObject gameObject)
	{
		base.CreateView(gameObject);
	}
	public override void UpdateView()
	{
		base.UpdateView();
		m_RimgAni.Rotate(m_RimgAni.forward * Time.deltaTime * -300);
        if (m_LoadCount <= 0) {
            ManagerUtility.UIMgr.CloseView<LoadingView>();
        }
	}
    public override void CloseView()
    {
        base.CloseView();
        m_LoadCount = 0;
    }
}
