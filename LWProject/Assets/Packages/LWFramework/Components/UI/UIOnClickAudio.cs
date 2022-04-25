using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
/// <summary>
///  点击放大效果by陈斌
/// </summary>
public class UIOnClickAudio : MonoBehaviour,IPointerClickHandler
{
    public AudioClip m_AudioClip;

    void Start() { 
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioSource.PlayClipAtPoint(m_AudioClip, Vector3.zero);
    }


  
}
