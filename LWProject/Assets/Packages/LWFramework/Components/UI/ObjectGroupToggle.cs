using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 组对象Toggle
/// </summary>
[RequireComponent( typeof(Button))]
public class ObjectGroupToggle : MonoBehaviour
{
    /// <summary>
    /// 选中状态下Sprite
    /// </summary>
    public GameObject m_ChooseGroup;
    /// <summary>
    /// 未选中状态下对象
    /// </summary>
    public GameObject m_UnChooseGroup;
    /// <summary>
    /// 是否可以自己取消
    /// </summary>
    [Tooltip("选中状态下,是否可以自己取消")]
    public bool m_SelfUnChoose;
    [SerializeField]
    private bool m_Choose;
    /// <summary>
    /// 状态
    /// </summary>
    public bool Choose {
        get {
            return m_Choose;
        }
        set {
            m_Choose = value;
            m_ChooseStateChange?.Invoke(m_Choose);
            ChangeBtnState();
        }
    }
    private Action<bool> m_ChooseStateChange;
    /// <summary>
    /// 状态改变回调Action
    /// </summary>
    public Action<bool> ChooseStateChange {
        get => m_ChooseStateChange;set => m_ChooseStateChange = value;
    }
    private Button m_Button;
    
    private void Start()
    {
        m_Button = GetComponent<Button>(); 
        m_Button.onClick.AddListener(OnBtnClick);
        ChangeBtnState();
    }

    private void OnBtnClick()
    {
        if (m_SelfUnChoose && Choose) {
            Choose = !Choose;
        }
        else if (!Choose)
        {
            Choose = !Choose;
        }
    }
    void ChangeBtnState() {
        if (m_Choose)
        {
            m_ChooseGroup.SetActive(true);
            m_UnChooseGroup.SetActive(false);
        }
        else {
            m_ChooseGroup.SetActive(false);
            m_UnChooseGroup.SetActive(true);
        }
    }
}
