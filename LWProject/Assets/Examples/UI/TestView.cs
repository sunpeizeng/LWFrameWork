using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;
using LWFramework;
using System.Collections.Generic;

[UIViewData("Assets/ExampleStep/Resources/UI/TestView.prefab", FindType.Name, "LWFramework/Canvas/Normal")]
public class TestView : BaseLogicUIView<TestViewLogic>
{
   

    [UIElement("TxtBind"),DataBinding("Name")]
    private Text m_TxtBind = null;
    [UIElement("IptBind"), DataBinding("Name")]
    private InputField m_IptBind = null;
    [UIElement("SliderBind"), DataBinding("Value",BindableEnum.Float)]
    private Slider m_SliderBind = null;

    [UIElement("ImgBind"), DataBinding("Value", BindableEnum.Float)]
    private Image m_ImgBind = null;

    [UIElement("IptBind2"), DataBinding("Value", BindableEnum.String)]
    private InputField m_IptBind2 = null;
    [UIElement("Btn1")]
    private Button _btn1=null;
	[UIElement("Btn2")]
    private Button _btn2 =null;
	[UIElement("Lyt/TestNodeTemp")]
    private Transform _testNodeTemp = null;
   
    private GameObjectPool<TestNodeTemp> _pool;
	private List<TestNodeTemp> _list;
	private string[] _datas;


	public string[] Datas {
		set {
			_datas = value;
			_list.ForEach(item => _pool.Unspawn(item));
			_list.Clear();
			for (int i = 0; i < _datas.Length; i++)
			{
                int index = i;
				TestNodeTemp node = _pool.Spawn();
				node.Text = _datas[i];
                node.OnClick = () =>
                {
                    ChooseIndex(index);
                };
				_list.Add(node);
			}
		}
	}
    public override void CreateView(GameObject go)
    {
        base.CreateView(go);
        _btn1.onClick.AddListener(() => {
            m_Logic.CreateNode();
        });
        _btn2.onClick.AddListener(() => {
            m_Logic.CreateNode2();
        });
        _pool = new GameObjectPool<TestNodeTemp>(5, _testNodeTemp.gameObject);
        _list = new List<TestNodeTemp>();


    }
    private void ChooseIndex(int index) {
        LWDebug.Log(_datas[index]);
        ManagerUtility.UIMgr.ViewData["Name"] = _datas[index];
        ManagerUtility.UIMgr.ViewData["Value"] = 0.5f;
    }
}