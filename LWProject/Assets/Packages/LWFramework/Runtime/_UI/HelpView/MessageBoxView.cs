using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;
using System;

[UIViewData("",FindType.Name, "LWFramework/Canvas/Top")]
public class MessageBoxView : BaseUIView 
{
	[UIElement("ImgBox/LytBtns/BtnCancel/TxtCancel")]
	private Text _txtCancel = null;
	[UIElement("ImgBox/LytBtns/BtnConfirm/TxtConfirm")]
    private Text _txtConfirm = null;
	[UIElement("ImgBox/LytBtns/BtnCancel")]
    private Button _btnCancel = null;
	[UIElement("ImgBox/LytBtns/BtnConfirm")]
    private Button _btnConfirm = null;
	[UIElement("ImgBox/TxtMsg")]
    private Text _txtMsg = null;
    [UIElement("ImgBox/TxtTitle")]
    private Text _txtTitle = null;
    /// <summary>
    /// 按钮点击操作
    /// </summary>
    public Action<bool> OnBtnClick { get; set; }
   
  
    /// <summary>
    /// 提示内容
    /// </summary>
    public string MsgStr
    {
        set => _txtMsg.text = value;
    }
    /// <summary>
    /// 提示标题
    /// </summary>
    public string TitleStr
    {
        set => _txtTitle.text = value;
    }
    /// <summary>
    /// 确认按钮文字
    /// </summary>
    public string ConfirmStr
    {
        set => _txtConfirm.text = value;
    }
    /// <summary>
    /// 放弃按钮文字
    /// </summary>
    public string CancelStr
    {
        set {
            if (value == "" || value == null)
            {
                _btnCancel.gameObject.SetActive(false);
            }
            else {
                _txtCancel.text = value;
            }
            
        }
    }
    public override  void CreateView(GameObject go)
	{
        base.CreateView(go);
        _btnConfirm.onClick.AddListener(() => 		{
            HandleEvent(true);
        });

        _btnCancel.onClick.AddListener(() => 		{
            HandleEvent(false);
        });
	}
    public override void OpenView()
    {
        base.OpenView();
       
    }
    public override void CloseView()
    {
        base.CloseView();
        _btnCancel.gameObject.SetActive(true);
        _btnConfirm.gameObject.SetActive(true);
    }
    private void HandleEvent(bool isOk)
    {       
        CloseView();
        OnBtnClick?.Invoke(isOk);
        OnBtnClick = null;
    }
}
