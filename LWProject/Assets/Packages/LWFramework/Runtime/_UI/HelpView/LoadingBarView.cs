using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;

[UIViewData("", FindType.Name, "LWFramework/Canvas/Top")]
public class LoadingBarView : BaseUIView
{
    [UIElement("TxtAppVer")]
    private Text m_TxtAppVer = null;
    [UIElement("TxtAssetVer")]
    private Text m_TxtAssetVer = null;
    [UIElement("TxtLoadMsg")]
    private Text m_TxtLoadMsg = null;
    [UIElement("TxtLoadValue")]
	private Text m_TxtLoadValue = null;
	[UIElement("ImgLoadBar")]
	private Image m_ImgLoadBar = null;
    private float m_BarWidth;
    private float m_BarHeight;

    public override  void CreateView(GameObject go)
	{
		base.CreateView(go);
        m_BarWidth = m_ImgLoadBar.rectTransform.rect.width;
        m_BarHeight = m_ImgLoadBar.rectTransform.rect.height;

    }
    public override void OpenView()
    {
        base.OpenView();
        SetLoadValue(0);
        SetLoadMsg("");
    }
    public override void CloseView()
    {
        base.CloseView();
        SetLoadValue(0);
        SetLoadMsg("");
    }
    public void SetLoadValue(float value) {
        if (m_ImgLoadBar.type == Image.Type.Filled)
        {
            m_ImgLoadBar.fillAmount = value;
        }
        else if (m_ImgLoadBar.type == Image.Type.Sliced) {
            m_ImgLoadBar.rectTransform.sizeDelta = new Vector2(value * m_BarWidth, m_BarHeight);
        }
        
        m_TxtLoadValue.text = (value*100).ToString("0")+ "%";
    }
    public void SetAppVer(int AppVer)
    {
        m_TxtAppVer.text = "应用版本：" + AppVer;
    }
    public void SetAssetVer(string AssetVer)
    {
        m_TxtAssetVer.text = "资源版本：" + AssetVer;
    }
    public void SetLoadMsg(string msg)
    {
        m_TxtLoadMsg.text = msg;
    }
}
