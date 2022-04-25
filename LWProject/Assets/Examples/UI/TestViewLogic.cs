using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;

public class TestViewLogic : BaseUILogic<TestView>  
{
	public TestViewLogic(TestView view): base(view)
	{
    }
    public override void OnCreateView()
    {
        base.OnCreateView();      
    }
	public override void OnOpenView()
	{
		base.OnOpenView();
		CreateNode();
	}
	public void CreateNode() {
		string[] values = new string[] { "aaaa", "bbbbb", "ccccc", "ddddd" };
		m_View.Datas = values;
	}
	public void CreateNode2()
	{
		string[] values = new string[] { "11111", "bbb222bb", "ccc3333cc", "dddd4444d" };
		m_View.Datas = values;
	}
}
