using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
/// <summary>
///  点击放大效果by陈斌
/// </summary>
public class UIOnClickSize : MonoBehaviour,IPointerClickHandler,IPointerDownHandler
{
    public float tweenTime = 0.3f;
    public Vector3 onHoverSize = new Vector3 (1.05f,1.05f,1.05f);
    private RectTransform _rectTransform;
    void Start() {
        _rectTransform = transform as RectTransform;       
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _rectTransform.DOScale(onHoverSize, tweenTime).SetLoops(2, LoopType.Yoyo);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        _rectTransform.localScale = Vector3.one;
    }
}
