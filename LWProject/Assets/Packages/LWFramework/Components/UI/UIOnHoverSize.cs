using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
/// <summary>
/// 光标进入退出效果 by陈斌
/// </summary>
public class UIOnHoverSize : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float tweenTime = 0.3f;
    public Vector3 onHoverSize = new Vector3 (1.05f,1.05f,1.05f);
    private RectTransform _rectTransform;
    private Vector3 _restSize;
    void Start() {
        _rectTransform = transform as RectTransform;       
        _restSize = transform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _rectTransform.DOScale(onHoverSize, tweenTime);
    
    }

   
    public void OnPointerExit(PointerEventData eventData)
    {
        _rectTransform.DOScale(_restSize, tweenTime);
       
    }
   
}
