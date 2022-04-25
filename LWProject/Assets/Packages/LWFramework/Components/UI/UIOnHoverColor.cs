using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
/// <summary>
/// 光标进入退出效果 by陈斌
/// </summary>
public class UIOnHoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
 
    public Color m_HoverColor;
    private Color[] m_DefaultColorArray;
    public MaskableGraphic[] m_MaskableGraphicArray;
    void Start() {

        m_DefaultColorArray = new Color[m_MaskableGraphicArray.Length];
        for (int i = 0; i < m_MaskableGraphicArray.Length; i++)
        {
            m_DefaultColorArray[i] = m_MaskableGraphicArray[i].color;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < m_MaskableGraphicArray.Length; i++)
        {
            m_MaskableGraphicArray[i].color = m_HoverColor;
        }
      

    }

   
    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < m_MaskableGraphicArray.Length; i++)
        {
            m_MaskableGraphicArray[i].color = m_DefaultColorArray[i];
        }
    }

}
